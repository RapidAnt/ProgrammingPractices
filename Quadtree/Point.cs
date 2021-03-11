using System;
using System.Collections.Generic;
using System.Text;

namespace QuadtreeApp
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point Vector { get; set; }
        public Point TopLeftBounday { get; set; }
        public Point BottomRightBounday { get; set; }

        public int Id { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move()
        {
            X += Vector.X;
            Y += Vector.Y;

            if (X > BottomRightBounday.X)
            {
                X = BottomRightBounday.X;
                Vector.X = Vector.X * -1;
            }

            if (X < TopLeftBounday.X)
            {
                X = TopLeftBounday.X;
                Vector.X = Vector.X * -1;
            }

            if (Y > BottomRightBounday.Y)
            {
                Y = BottomRightBounday.Y;
                Vector.Y = Vector.Y * -1;
            }

            if (Y < TopLeftBounday.Y)
            {
                Y = TopLeftBounday.Y;
                Vector.Y = Vector.Y * -1;
            }
        }
    }
}
