using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

string GetPrivateKeyPath(string publicKey)
{
    return System.IO.Path.Combine("ballotCounter", "pki", "ballotcounter.pem.key");
}
RSA LoadKey(string publicKey)
{
    string path = GetPrivateKeyPath(publicKey);
    RSA rsa = RSA.Create();
    rsa.ImportFromPem(System.IO.File.ReadAllText(path));
    return rsa;
}

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

EncryptedBallot b = System.Text.Json.JsonSerializer.Deserialize<EncryptedBallot>(
    System.IO.File.ReadAllText("out.json")
);
//var decryptedBallot =b.DecryptBallotData()
//Console.WriteLine(decryptedBallot);
