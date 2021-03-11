using System;
using System.Collections.Generic;
using System.Text;

namespace QuadtreeApp
{
    class QuadTree
    {
        private int Depth { get; set; }
        private Point TopLeft { get; set; }
        private Point BottomRight { get; set; }

        private List<Point> Points { get; set; } = new List<Point>();

        private QuadTree TopLeftQuad { get; set; }
        private QuadTree TopRightQuad { get; set; }
        private QuadTree BottomLeftQuad { get; set; }
        private QuadTree BottomRightQuad { get; set; }

        private int Capacity { get; set; }

        public QuadTree(Point TopLeft, Point BottomRight, int Capacity, int Depth)
        {
            this.TopLeft = TopLeft;
            this.BottomRight = BottomRight;
            this.Capacity = Capacity;
            this.Depth = Depth;
        }

        public void AddPoint(Point point)
        {
            if (!PointInRange(point, TopLeft, BottomRight))
            {
                return;
            }

            if (Points.Count < Capacity)
            {
                Points.Add(point);
            }
            else
            {
                // If this not have not childrens we need to create them
                if (TopLeftQuad == null)
                {
                    SplitQuadTree();
                }

                // Try to add the point to one of the children of the current nod
                TopLeftQuad.AddPoint(point);
                TopRightQuad.AddPoint(point);
                BottomLeftQuad.AddPoint(point);
                BottomRightQuad.AddPoint(point);
            }
        }

        public bool PointInRange(Point point, Point topLeft, Point bottomRight)
        {
            return point.X >= topLeft.X && point.X <= bottomRight.X && point.Y >= topLeft.Y && point.Y <= bottomRight.Y;
        }

        public bool QuadIntersectsRange(Point topLeft, Point bottomRight)
        {
            return topLeft.X >= TopLeft.X && topLeft.X <= bottomRight.X ||
                    bottomRight.X >= TopLeft.X && bottomRight.X <= bottomRight.X &&
                    topLeft.Y >= TopLeft.Y && topLeft.Y <= bottomRight.Y ||
                    bottomRight.Y >= TopLeft.Y && bottomRight.Y <= bottomRight.Y;
        }

        private void SplitQuadTree()
        {
            int width = BottomRight.X - TopLeft.X + 1;
            int height = BottomRight.Y - TopLeft.Y + 1;

            int xMin = TopLeft.X;
            int xMid = xMin + width / 2 - 1;
            int xMax = BottomRight.X;
            if (xMid < xMin)
            {
                xMid = xMin;
            }
            if (xMid > xMax)
            {
                xMid = xMax;
            }

            int yMin = TopLeft.Y;
            int yMid = yMin + height / 2 - 1;
            int yMax = BottomRight.Y;
            if (yMid < yMin)
            {
                yMid = yMin;
            }
            if (yMid > yMax)
            {
                yMid = yMax;
            }

            TopLeftQuad = new QuadTree(
                                new Point(xMin, yMin),
                                new Point(xMid, yMid),
                                Capacity,
                                Depth + 1);
            TopRightQuad = new QuadTree(
                                new Point(xMid + 1, yMin),
                                new Point(xMax, yMid),
                                Capacity,
                                Depth + 1);
            BottomLeftQuad = new QuadTree(
                                new Point(xMin, yMid + 1),
                                new Point(xMid, yMax),
                                Capacity,
                                Depth + 1);
            BottomRightQuad = new QuadTree(
                                new Point(xMid + 1, yMid + 1),
                                new Point(xMax, yMax),
                                Capacity,
                                Depth + 1);
        }


        public void Display()
        {
            DisplayArea();
            DisplayPoints();
        }

        public void DisplayArea()
        {
            for (int y = TopLeft.Y; y <= BottomRight.Y; y++)
            {
                Console.SetCursorPosition(TopLeft.X, y);
                Console.Write("│");
                Console.SetCursorPosition(BottomRight.X, y);
                Console.Write("│");
            }

            for (int x = TopLeft.X; x <= BottomRight.X; x++)
            {
                Console.SetCursorPosition(x, TopLeft.Y);
                Console.Write("─");
                Console.SetCursorPosition(x, BottomRight.Y);
                Console.Write("─");
            }

            Console.SetCursorPosition(TopLeft.X, TopLeft.Y); Console.Write("┌");
            Console.SetCursorPosition(BottomRight.X, TopLeft.Y); Console.Write("┐");
            Console.SetCursorPosition(TopLeft.X, BottomRight.Y); Console.Write("└");
            Console.SetCursorPosition(BottomRight.X, BottomRight.Y); Console.Write("┘");

            TopLeftQuad?.DisplayArea();
            TopRightQuad?.DisplayArea();
            BottomLeftQuad?.DisplayArea();
            BottomRightQuad?.DisplayArea();
        }

        public void DisplayPoints()
        {
            foreach (Point point in Points)
            {
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write("X");
            }

            TopLeftQuad?.DisplayPoints();
            TopRightQuad?.DisplayPoints();
            BottomLeftQuad?.DisplayPoints();
            BottomRightQuad?.DisplayPoints();
        }

        public List<Point> QueryPointsInRange(Point topLeft, Point bottomRight)
        {
            List<Point> resultPoints = new List<Point>();

            if (QuadIntersectsRange(topLeft, bottomRight))
            {
                foreach (Point point in Points)
                {
                    if (PointInRange(point, topLeft, bottomRight))
                    {
                        resultPoints.Add(point);
                    }
                }

                if (TopLeftQuad != null)
                {
                    resultPoints.AddRange(TopLeftQuad.QueryPointsInRange(topLeft, bottomRight));
                    resultPoints.AddRange(TopRightQuad.QueryPointsInRange(topLeft, bottomRight));
                    resultPoints.AddRange(BottomLeftQuad.QueryPointsInRange(topLeft, bottomRight));
                    resultPoints.AddRange(BottomRightQuad.QueryPointsInRange(topLeft, bottomRight));
                }
            }

            return resultPoints;
        }

        public bool Search(Point point)
        {
            bool result = false;

            if (!PointInRange(point, TopLeft, BottomRight))
            {
                return result;
            }

            foreach (Point p in Points)
            {
                result = result || (point.X == p.X) && (point.Y == p.Y);
            }

            if ((result == false) && (TopLeftQuad != null))
            {
                result = TopLeftQuad.Search(point);
            }

            if ((result == false) && (TopRightQuad != null))
            {
                result = TopRightQuad.Search(point);
            }

            if ((result == false) && (BottomLeftQuad != null))
            {
                result = BottomLeftQuad.Search(point);
            }

            if ((result == false) && (BottomRightQuad != null))
            {
                result = BottomRightQuad.Search(point);
            }

            return result;
        }
    }

}
