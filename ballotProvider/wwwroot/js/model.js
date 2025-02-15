function str2ab(str) {
    const buf = new ArrayBuffer(str.length);
    const bufView = new Uint8Array(buf);
    for (let i = 0, strLen = str.length; i < strLen; i++) {
        bufView[i] = str.charCodeAt(i);
    }
    return buf;
}

async function importRsaKey(pem) {
    // fetch the part of the PEM string between header and footer
    let splitted = pem.trim().split("\n");
    const pemHeader = splitted[0];
    const pemFooter = splitted[splitted.length - 1];
    if(pemHeader.startsWith("-"))
    {
        splitted = splitted.slice(1);
    }
    if(pemFooter.startsWith("-"))
    {
        splitted = splitted.slice(0, splitted.length - 1);
    }

    let pemContents = splitted.join("\n");
    console.log("\"", pemContents, "\"");
    // base64 decode the string to get the binary data
    const binaryDerString = window.atob(pemContents);
    // convert from a binary string to an ArrayBuffer
    const binaryDer = str2ab(binaryDerString);

    return await window.crypto.subtle.importKey(
        "spki",
        binaryDer,
        {
            name: "RSA-OAEP",
            hash: "SHA-256",
        },
        true,
        ["encrypt", "wrapKey"],
    );
}

function arrayBufferToBase64(buffer) {
    let binary = '';
    let bytes = new Uint8Array(buffer);
    let len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return btoa(binary);
}



async function encryptBallot(ballot, publicKey, returnAddress, ballotId){
    let ballotString = (JSON.stringify(ballot))
    let encryptionResult=await encrypt(ballotString)
    let body = {
            EncryptedBallotData: encryptionResult.encrypted,
            EncryptedKey: encryptionResult.wrappedKey,
            Ivs: encryptionResult.iv,
            ReturnAddress: returnAddress,
            PublicKey: publicKey,
            BallotId: ballotId
        }
    return body;
}
async function encrypt(text) {
    let aesKey = await window.crypto.subtle.generateKey({
        name: "AES-GCM",
        length: 256
    }, true, ["encrypt", "decrypt", "wrapKey", "unwrapKey"]);


    let rsaKey = await importRsaKey(publicKey);

    let encryptedKey = await window.crypto.subtle.wrapKey("raw", aesKey, rsaKey, "RSA-OAEP");

    let iv = window.crypto.getRandomValues(new Uint8Array(12));
    let encrypted = await window.crypto.subtle.encrypt({
        name: "AES-GCM",
        iv: iv,
    }, aesKey, new TextEncoder().encode(text));
    return {
        wrappedKey: arrayBufferToBase64(encryptedKey),
        encrypted: arrayBufferToBase64(encrypted),
        iv: arrayBufferToBase64(iv)
    }
}
