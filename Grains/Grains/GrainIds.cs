using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class GrainIds
    {
        public static readonly Guid TestGrainId = Guid.Parse("5B4DFADE-D577-4DA8-96FA-AA8AAA4BD0F2");
        public static readonly Guid GameGrainId = Guid.Parse("778DA50A-B632-475C-8A3F-8D510310518E");

        public static readonly Guid StreamId = Guid.Parse("CFAEFEAD-FB3C-4478-98CB-92900CDCE4E3");

        internal const string StreamNamespace = "HaloStreamingNamespace";
    }
}
