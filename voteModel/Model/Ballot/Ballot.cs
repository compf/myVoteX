namespace Model.Ballot;

using System.Runtime.CompilerServices;
using Model.BallotGroup;
using Model.Vote;

public class Ballot
{
    public Dictionary<string, BallotGroup> Groups { get; } = new();


    public bool CheckValidity(BallotValidityChecker ballotValidityChecker, BallotGroupValidityChecker ballotGroupValidityChecker, VoteValidityChecker voteValidityChecker)
    {
        bool checkerResult =  ballotValidityChecker.CheckValidity(this,ballotGroupValidityChecker, voteValidityChecker);
        return  checkerResult;
    }
}
public interface BallotValidityChecker
{
    bool CheckValidity(Ballot ballot, BallotGroupValidityChecker groupValidityChecker, VoteValidityChecker voteValidityChecker);
}
public interface BallotGroupValidatorAggregator
{
    public bool Aggegrate(Dictionary<string,bool> values);
}
public class AllGroupsValidAggregator : BallotGroupValidatorAggregator
{
    public bool Aggegrate(Dictionary<string,bool> values)
    {
        return values.Values.All((it)=>it);
    }
}

public class TwoGroupsValidityChecker : BallotValidityChecker
{
    public BallotGroupValidatorAggregator Aggregator{get;}
    public bool CheckValidity(Ballot ballot, BallotGroupValidityChecker groupValidityChecker, VoteValidityChecker voteValidityChecker)
    {
        int count=ballot.Groups.Count;
        var values=ballot.Groups.ToDictionary((it)=>it.Key, (it)=>groupValidityChecker.CheckValidity(it.Value,voteValidityChecker));
        return Aggregator.Aggegrate(values);
    }
    public TwoGroupsValidityChecker(BallotGroupValidatorAggregator aggregator)
    {
        this.Aggregator=aggregator;
    }
}