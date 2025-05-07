namespace Model.Vote;

public class BooleanVote : BallotVote
{
    public bool Value { get; set; }

 

    public override int GetVoteValue()
    {
        return Value ? 1 : 0;
    }
}