using System;

namespace Data
{
    public class PieceEvent1 : IPieceEvent
    {
        public Guid Id { get; } = Guid.NewGuid();

        public object Payload { get; set; }
    }
}
