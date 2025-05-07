namespace Model.Envelope;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Model.BallotGroup;
using Model.Vote;
using Model.Ballot;

public record EncryptedBallot(
    string? Ivs,
    string? EncryptedBallotData,
    string? EncryptedKey,
    string PublicKey,
    string ReturnAddress,
    string BallotId


) : BallotEnvelope
{


    public Ballot Decrypt(Crypto.ICryptoGenerator generator)
    {
        string decrypted = DecryptBallotData(generator);
        var json=JsonObject.Parse(decrypted);
       Ballot ballot =new Ballot();
        foreach(var group in json["Groups"].AsObject())
        {
            BallotGroup ballotGroup=new BallotGroup();
            foreach(var vote in group.Value["Votes"].AsObject())
            {
                if(vote.Value.AsObject()["Value"].GetValue<bool>()){
                    ballotGroup.Votes.Add(vote.Key,new BooleanVote(){Value=true});
                }
            }
            ballot.Groups.Add(group.Key,ballotGroup);
        }
        return ballot;
    }
    public string DecryptBallotData(Crypto.ICryptoGenerator generator)
    {
        EncryptedBallot encryptedBallot = this;
        RSA rsa = generator.CreateRSA(encryptedBallot.PublicKey);
        Console.WriteLine("Decrypting with key: " + encryptedBallot.PublicKey);

        // Unwrap the AES key
        byte[] wrappedKey = Convert.FromBase64String(encryptedBallot.EncryptedKey);
        byte[] aesKey = rsa.Decrypt(wrappedKey, RSAEncryptionPadding.OaepSHA256);

        // Decode the IV
        byte[] iv = Convert.FromBase64String(encryptedBallot.Ivs);

        // Decrypt the data
        using (AesGcm aes = new AesGcm(aesKey))
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBallot.EncryptedBallotData);
            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];

            byte[] ciphertext = new byte[encryptedBytes.Length - AesGcm.TagByteSizes.MaxSize];

            // Extract the tag from the end of the encrypted data
            Array.Copy(
                encryptedBytes,
                encryptedBytes.Length - AesGcm.TagByteSizes.MaxSize,
                tag,
                0,
                AesGcm.TagByteSizes.MaxSize
            );
            Array.Copy(
                encryptedBytes,
                0,
                ciphertext,
                0,
                encryptedBytes.Length - AesGcm.TagByteSizes.MaxSize
            );

            decryptedBytes = new byte[ciphertext.Length];
            aes.Decrypt(iv, ciphertext, tag, decryptedBytes);
            string decrypted = Encoding.UTF8.GetString(decryptedBytes);
            return decrypted;
        }
    }
}