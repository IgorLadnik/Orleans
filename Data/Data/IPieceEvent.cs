using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IPieceEvent
    {
        Guid Id { get; }
        object Payload { get; }
    }
}
