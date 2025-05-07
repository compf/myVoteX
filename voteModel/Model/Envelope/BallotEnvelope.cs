namespace Model.Envelope;



public interface BallotEnvelope
{
   
        public string PublicKey { get; init; }
        public string ReturnAddress { get; init; }

        public string BallotId { get; init; }

}