
function assert(expected,actual){
    if(expected!=actual){
        throw expected+"!="+actual;
    }
}
function testEncryption(){
    const ballot={
        Groups:{
            Erststimme:{
                SPD:true
            },
            Zweitstimme:{
                SPD:true
            }
        }
    }
    const  publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAv0FSVWXjUGVzwqLbLo4V8m+l3gMJxQdboE2facaPfDfWJ7gKVcT4vg8HnMPdtH0f1gPMyc8maYeym/WUNIt2zFoKf6qCwCsW9NCBHYjIqa7xQoH0YwRY4F3ctawuxdzqZSHrolCGvFtoo7dDWTi88nLOYzOh2ZsTZUBAF+twAS0h75IjcbwH/IYt1olK4jxNqBzY4OkJcc4rF/oGexeoJUHaUaZvcYV7q5o2jIfE8b+riaeCAtIvAJgbxpqkg7iuDOeBfTpSrUnPdr4HfZwUr2i59nfXYgfZmLRXha9bklx607thqrsToyYHw2qgLyrMm3mjJdd9DSsHE/QFGWVoyQIDAQAB";
    const returnAddress="";
    const ballotId=""
    encryptBallot(ballot, publicKey, returnAddress, ballotId).then((encryptionresult)=>{
        const  encryptedBallotData = "Q9wYKIqdOsbFpY5y3n18wMZnlwW0y4EqiYjFTlmVqTT5WiP9ZniXO6qZYoXEUEizd9ucHvOzcssGQRVh6cAFgJFUsI1Tp4tDau5pQ6dPIzY90kcjfFCna6tnsJ1IPzu6qJbezgiAM/bjI76k+fElzO0qDwDhyN5YGg==";
        assert(encryptedBallotData,encryptionresult)
    
    });
    

}
const testMethods=[testEncryption];

function testAll(){
    for(let t of testMethods){
        t();
    }
}


