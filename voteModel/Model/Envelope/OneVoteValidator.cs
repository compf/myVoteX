using Model.BallotGroup;
using Model.Vote;

namespace Model.Envelope;

public class OneVoteBallotGroupValidilityChecker : BallotGroupValidityChecker
{
    public bool CheckValidity(BallotGroup.BallotGroup group, VoteValidityChecker voteValidityChecker)
    {
        bool allValid=group.Votes.Values.All((it)=>voteValidityChecker.CheckValidity(it));
        int count =group.Votes.Values.Sum((it)=>it.GetVoteValue());
        return allValid && count==1;
    }
}