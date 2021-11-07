using System;

namespace Grains
{
    public class GrainIds
    {
        public static readonly Guid TestGrainId = Guid.Parse("5B4DFADE-D577-4DA8-96FA-AA8AAA4BD0F2");
        public static readonly Guid GameGrainId = Guid.Parse("778DA50A-B632-475C-8A3F-8D510310518E");
        public static readonly Guid DeviceGrainId = Guid.Parse("4E40F0A9-53BE-41FC-9D89-DE8AD97BB11B");

        public static readonly Guid StreamId = Guid.Parse("CFAEFEAD-FB3C-4478-98CB-92900CDCE4E3");

        public const string StreamNamespace = "HaloStreamingNamespace";
    }
}
