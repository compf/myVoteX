using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Model;

public class Ballot
{
    public Dictionary<string, BallotGroup> Groups { get; } = new();

    public virtual bool IsValid()
    {
        return true;
    }

    public bool CheckValidity(BallotValidityChecker? checker)
    {
        bool checkerResult = checker?.CheckValidity(this) ?? true;
        return IsValid() && checkerResult;
    }
}


public interface BallotEnvelope
{
    public Ballot? GetBallot();
    public class BallotEnvelopeData
    {
        public string PublicKey { get; set; }
        public string ReturnAddress { get; set; }

        public string BallotId { get; set; }
    }
    public bool CanObtainBallot()
    {
        return GetBallot() != null;
    }

}


public class VerySimpleBallot 
{
   public Dictionary<string, BallotGroup> Groups { get; set;} = new();
}


public class SimpleBallot : Ballot, BallotEnvelope
{
    public Ballot? GetBallot()
    {
        return null;
    }
    public string PublicKey { get; set; }
    public string ReturnAddress { get; set; }

    public string BallotId { get; set; }

    public SimpleBallot(string publicKey, string returnAddress, string ballotId)
    {
        PublicKey = publicKey;
        ReturnAddress = returnAddress;
        BallotId = ballotId;
    }

}


public record EncryptedBallot(
    string? Ivs,
    string? EncryptedBallotData,
    string? EncryptedKey,
    string PublicKey,
    string ReturnAddress,
    string BallotId


) : BallotEnvelope
{

    public Ballot? GetBallot()
    {
        return null;
    }

    public Ballot Decrypt(ICryptoGenerator generator)
    {
        string decrypted = DecryptBallotData(generator);
        var json=JsonObject.Parse(decrypted);
        SimpleBallot ballot=new SimpleBallot(this.PublicKey,this.ReturnAddress,this.BallotId);
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
    public string DecryptBallotData(ICryptoGenerator generator)
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


public class BallotGroup
{
    public Dictionary<string, BallotVote> Votes { get; } = new();

    public virtual bool IsValid()
    {
        return true;
    }

    public bool CheckValidity(BallotGroupValidtyChecker? checker)
    {
        bool checkerResult = checker?.CheckValidity(this) ?? true;
        return IsValid() && checkerResult;
    }
}
public interface BallotValidityChecker
{
    bool CheckValidity(Ballot ballot);
}

public interface BallotGroupValidtyChecker
{
    bool CheckValidity(BallotGroup group);
}
public interface VoteValidityChecker
{
    bool CheckValidity(BallotVote vote);
}

public abstract class BallotVote
{

    public abstract bool IsValid();
    public bool CheckValidity(VoteValidityChecker? checker)
    {
        bool checkerResult = checker?.CheckValidity(this) ?? true;
        return IsValid() && checkerResult;
    }
    public abstract int GetVoteValue();
}

public class BooleanVote : BallotVote
{
    public bool Value { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override int GetVoteValue()
    {
        return Value ? 1 : 0;
    }
}