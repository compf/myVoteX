using System.ComponentModel;

namespace Model;

public class Ballot
{
    public Dictionary<string, BallotGroup> Groups { get; } = new();

        public virtual bool IsValid(){
        return true;
    }

    public bool CheckValidity(BallotValidityChecker? checker)
    {
        bool checkerResult = checker?.CheckValidity(this) ?? true;
        return IsValid() && checkerResult;
    }
}


public interface BallotEnvelope{
    public Ballot? GetBallot();
    public class BallotEnvelopeData{
    public string PublicKey { get; set; }
    public string ReturnAddress { get; set; }

    public string BallotId{get;set;}
}
    public bool CanObtainBallot()
    {
        return GetBallot() != null;
    }

}





public class SimpleBallot:BallotEnvelope{
    public Ballot Ballot { get; set; }

    public Ballot? GetBallot()
    {
        return Ballot;
    }
      public string PublicKey { get; set; }
    public string ReturnAddress { get; set; }

    public string BallotId{get;set;}
  
}


public record  EncryptedBallot(
    string? Ivs,
    string? EncryptedBallotData,
    string? EncryptedKey


) : BallotEnvelope{

    public Ballot? GetBallot()
    {
        return null;
    }
      public string PublicKey { get; init; }
    public string ReturnAddress { get; init; }

    public string BallotId{get;set;}
}


public class BallotGroup
{
    public Dictionary<string, BallotVote> Votes { get; } = new();

    public virtual bool IsValid(){
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

    public abstract bool IsValid ();
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