using System;

namespace GameUtils
{
    public class Point : IEquatable<Point>
    {
        private readonly int _x;
        private readonly int _y;

        // public fields
        public int X => _x;
        public int Y => _y;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            
            if (ReferenceEquals(this, other)) 
                return true;
            
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (ReferenceEquals(this, obj)) 
                return true;
            
            if (obj.GetType() != this.GetType()) 
                return false;
            
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
