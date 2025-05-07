using Model.Vote;

namespace Model.BallotGroup;
public class BallotGroup
{
    public Dictionary<string, BallotVote> Votes { get; } = new();

    public virtual bool IsValid()
    {
        return true;
    }


}

public interface BallotGroupValidityChecker
{
    bool CheckValidity(BallotGroup group, VoteValidityChecker voteValidityChecker);
}