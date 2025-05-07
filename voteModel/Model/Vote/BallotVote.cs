namespace Model.Vote;

public abstract class BallotVote
{

    public bool CheckValidity(VoteValidityChecker? checker)
    {
        bool checkerResult = checker?.CheckValidity(this) ?? true;
        return  checkerResult;
    }
    public abstract int GetVoteValue();
}
