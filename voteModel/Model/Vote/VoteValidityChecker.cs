namespace Model.Vote;
public interface VoteValidityChecker
{
    bool CheckValidity(BallotVote vote);
}

public class SimpleVoteValidityChecker : VoteValidityChecker
{
    public bool CheckValidity(BallotVote vote)
    {
        return true;
    }
}