namespace  Model.Crypto;

using System.Security.Cryptography;
public interface ICryptoGenerator{
    RSA CreateRSA(string publicKey);
}