using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QuadtreeApp
{
    class Program
    {
        static List<Point> Points = new List<Point>();
        static int quadTreeWidth = 80;
        static int quadTreeHeight = 25;
        static int NumberOfPoints = 500;
        static int QuadCapacity = 4;
        static QuadTree qt;
        static Random r;

        static int draws = 0;
        static Stopwatch watch = new System.Diagnostics.Stopwatch();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            GeneratePoints();

            double elapsedTime = 0;
            do
            {
                watch = System.Diagnostics.Stopwatch.StartNew();
                MovePoints();
                GenerateQuadTree();
                Draw();
                watch.Stop();

                elapsedTime += watch.ElapsedMilliseconds;
                if (elapsedTime  >= 1000)
                {
                    double fps = (draws / (elapsedTime / 1000.0));

                    Console.SetCursorPosition(0, 26);
                    Console.WriteLine(String.Format("{0:0.##}", fps));

                    System.Threading.Thread.Sleep(100);

                    draws = 0;
                    elapsedTime = 0;
                }
            }
            while (!Console.KeyAvailable);

           // Console.ReadLine();
        }

        static void GeneratePoints()
        {
            r = new Random();

            for (int i = 1; i <= NumberOfPoints; i++)
            {
                Point Vector = new Point(r.Next(0, 3) - 1, r.Next(0, 3) - 1);

                int x = r.Next(1, quadTreeWidth + 1);
                int y = r.Next(1, quadTreeHeight + 1);

                Point point = new Point(x, y);
                point.Id = i;
                point.Vector = Vector;
                point.TopLeftBounday = new Point(1, 1);
                point.BottomRightBounday = new Point(quadTreeWidth, quadTreeHeight);

                Points.Add(point);
            }
        }

        static void GenerateQuadTree()
        {
            qt = new QuadTree(new Point(0, 0), new Point(quadTreeWidth, quadTreeHeight), QuadCapacity, 0);

            //Console.WriteLine("Adding points to QT...");
            foreach (var point in Points)
            {
                qt.AddPoint(point);
            }
            //Console.WriteLine();
        }

        static void Draw()
        {
            //we increase the cont of draws in order to count fps 
            draws++; 

            Console.Clear();

            foreach (var selectedPoint in Points)
            {
                Console.ForegroundColor = ConsoleColor.White;

                List<Point> comparingPoints = new List<Point>();

                bool useQuadTree = true;
                if (useQuadTree)
                {
                    //Use this part to use QuadTree compares (where subset of point is compared with everything)
                    comparingPoints = qt.QueryPointsInRange(selectedPoint, selectedPoint);
                }
                else
                {
                    // Use this part to use the old general compares (where everything is compared with everything)
                    comparingPoints = Points;
                }

                foreach (var allPoint in comparingPoints)
                {
                    if ((selectedPoint.X == allPoint.X) && 
                        (selectedPoint.Y == allPoint.Y) && 
                        selectedPoint.Id != allPoint.Id)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.SetCursorPosition(selectedPoint.X, selectedPoint.Y);
                    Console.Write("x");
                }
            }
        }

        static void MovePoints()
        {
            foreach (var point in Points)
            {
                point.Move();
            }
        }
    }
}
