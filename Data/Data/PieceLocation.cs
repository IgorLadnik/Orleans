using System;

namespace Data
{
    //[Serializable]
    public struct PieceLocation
    {
        private char _x;
        private short _y;

        public PieceLocation(string xy)
        {
            _x = xy[0];
            _y = short.Parse(new string(xy[1], 1));
        }

        public override string ToString() => $"{_x}{_y}";

        public static bool operator ==(PieceLocation a, PieceLocation b) =>
                a._x == b._x && a._y == b._y;
        public static bool operator !=(PieceLocation a, PieceLocation b) =>
                a._x != b._x || a._y != b._y;
    }
}
