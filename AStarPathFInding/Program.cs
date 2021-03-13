using System;
using System.Collections.Generic;
using System.Linq;

// AUTHOR: Ferenc Racz
// DATE: 2020.04.30

namespace AStarPathFInding
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int F { get { return G + H; } set { } }
        public int G { get; set; }
        public int H { get; set; }
        public Node Parent { get; set; }
    }

    class AStar
    {
        private List<List<int>> Map { get; set; }
        private Node StartNode { get; set; }
        private Node EndNode { get; set; }
        private List<Node> OpenNodes { get; set; }
        private List<Node> ClosedNodes { get; set; }
        public bool ShowMapDuringCalculation {get;set;}
        public int SleepBetwenDraws { get; set; }

        public AStar(List<List<int>> map, Node startNode, Node endNode)
        {
            Map = map;
            StartNode = startNode;
            EndNode = endNode;

            OpenNodes = new List<Node>();
            ClosedNodes = new List<Node>();

            ShowMapDuringCalculation = false;
            SleepBetwenDraws = 500;
        }

        private bool NodeIsReachable(int x, int y)
        {
            bool result = false;

            if ((x >= 1) && (x <= Map[0].Count) && (y >= 1) && (y <= Map.Count))
            {
                if (Map[y - 1][x - 1] == 0)
                {
                    result = true;
                }
            }

            return result;
        }

        private List<Node> GetAdjacentNodes(Node currentNode)
        {
            List<Node> adjacentNodes = new List<Node>();

            int x = currentNode.X;
            int y = currentNode.Y;

            if (NodeIsReachable(x - 1, y))
            {
                adjacentNodes.Add(new Node { X = x - 1, Y = y });
            }

            if (NodeIsReachable(x + 1, y))
            {
                adjacentNodes.Add(new Node { X = x + 1, Y = y });
            }

            if (NodeIsReachable(x, y - 1))
            {
                adjacentNodes.Add(new Node { X = x, Y = y - 1 });
            }

            if (NodeIsReachable(x, y + 1))
            {
                adjacentNodes.Add(new Node { X = x, Y = y + 1 });
            }

            return adjacentNodes;
        }

        private int CalculateHeuristic(Node node1, Node node2)
        {
            int result = Math.Abs((node2.X - node1.X) * (node2.X - node1.X)) + Math.Abs((node2.Y - node1.Y) * (node2.Y - node1.Y));

            return result;
        }
        public void CalculatePath()
        {
            // 1.	Add the starting square (or node) to the open list.
            OpenNodes.Add(StartNode);

            bool targetReached = false;
            bool OpenNodesAreEmpty = false;
            bool SearchInProgress;
            do
            {
                if (ShowMapDuringCalculation)
                {
                    ShowMap(null);
                    System.Threading.Thread.Sleep(SleepBetwenDraws);
                }

                // A)	Look for the lowest F cost square on the open list.
                //      We refer to this as the current square.
                //      Node currentNode = OpenNodes.Select(on => on.F).Min();
                Node currentNode = OpenNodes.FirstOrDefault(on => on.F == OpenNodes.Select(o => o.F).Min());

                // B)	Switch it to the closed list.
                OpenNodes.Remove(currentNode);
                ClosedNodes.Add(currentNode);
                targetReached = ClosedNodes.Where(cn => (cn.X == EndNode.X) && (cn.Y == EndNode.Y)).Any();

                // C)	For each of the 8 squares adjacent to this current square:
                List<Node> adjacentNodes = GetAdjacentNodes(currentNode);

                if (!targetReached)
                {
                    foreach (var node in adjacentNodes)
                    {
                        // If it is not walkable or if it is on the closed list, ignore it. 
                        // Otherwise do the following.
                        bool nodeInTheClosedNodes = ClosedNodes.Any(cn => (cn.X == node.X) && (cn.Y == node.Y));
                        if (nodeInTheClosedNodes)
                        {
                            continue;
                        }

                        Node nodeInTheOpenNodes = OpenNodes.FirstOrDefault(cn => (cn.X == node.X) && (cn.Y == node.Y));

                        if (nodeInTheOpenNodes == null)
                        {
                            // If it isn’t on the open list, add it to the open list. 
                            // Make the current square the parent of this square.
                            // Record the F, G, and H costs of the square.
                            node.Parent = currentNode;
                            node.G = Math.Abs(StartNode.X - node.X) + Math.Abs(StartNode.Y - node.Y);
                            node.H = CalculateHeuristic(node, EndNode);
                            node.F = node.G + node.H;

                            OpenNodes.Add(node);
                        }
                        else
                        {
                            // If it is on the open list already, 
                            // check to see if this path to that square is better, using G cost as the measure.
                            // 
                            // A lower G cost means that this is a better path.
                            // If so, change the parent of the square to the current square, 
                            // and recalculate the G and F scores of the square. 
                            // 
                            // If you are keeping your open list sorted by F score,
                            // you may need to resort the list to account for the change.

                            if (node.G < nodeInTheOpenNodes.G)
                            {
                                nodeInTheOpenNodes.Parent = currentNode;
                                nodeInTheOpenNodes.G = Math.Abs(StartNode.X - node.X) + Math.Abs(StartNode.Y - node.Y);
                                nodeInTheOpenNodes.H = CalculateHeuristic(node, EndNode);
                                nodeInTheOpenNodes.F = node.G + node.H;
                            }
                        }
                    }
                }
                // D) Stop when you:
                //  -Add the target square to the closed list, 
                //      in which case the path has been found,
                //  -Fail to find the target square, and the open list is empty.
                //       In this case, there is no path.

                OpenNodesAreEmpty = OpenNodes.Count == 0;
                SearchInProgress = !OpenNodesAreEmpty && !targetReached;
            } while (SearchInProgress);
        }

        public List<Node> GetPath()
        {
            List<Node> path = new List<Node>();

            //  -Fail to find the target square -> In this case, there is no path.
            bool targetInTheClosedNodes = ClosedNodes.Any(cn => (cn.X == EndNode.X) && (cn.Y == EndNode.Y));
            if (targetInTheClosedNodes)
            {
                //  3.Save the path. 
                //      Working backwards from the target square, 
                //      go from each square to its parent square until you reach the starting square. 
                //      That is your path.

                bool startNodeReached = false;
                Node currentNode = ClosedNodes.FirstOrDefault(cn => (cn.X == EndNode.X) && (cn.Y == EndNode.Y));
                do
                {
                    path.Add(currentNode);
                    if (!((currentNode.X == StartNode.X) && (currentNode.Y == StartNode.Y)))
                    {
                        currentNode = ClosedNodes.FirstOrDefault(cn => (cn.X == currentNode.Parent.X) && (cn.Y == currentNode.Parent.Y));
                    }
                    startNodeReached = path.Where(cn => (cn.X == StartNode.X) && (cn.Y == StartNode.Y)).Any();
                } while (!startNodeReached);
            }

            return path;
        }

        public void ShowMap(List<Node> path)
        {
            Console.Clear();

            // ░▒▓­▄■▀O0°*
            string wallCharacter = "█";
            string roomCharacter = " ";
            string pathCharacter = ".";
            string openNodeCharacter = "O";
            string closedNodeCharacter = "*";
            string startNodeCharacter = "S";
            string endNodeCharacter = "E";

            for (int y = 0; y < Map.Count; y++)
            {
                for (int x = 0; x < Map[0].Count; x++)
                {
                    string selectedCharacter = string.Empty;

                    bool isPath = path == null ? false : path.Any(p => (p.X == x + 1) && (p.Y == y + 1));
                    bool isOpenNode = OpenNodes.Any(p => (p.X == x + 1) && (p.Y == y + 1));
                    bool isClosedNode = ClosedNodes.Any(p => (p.X == x + 1) && (p.Y == y + 1));
                    bool isStartNode = StartNode.X == x + 1 && StartNode.Y == y + 1;
                    bool isEndNode = EndNode.X == x + 1 && EndNode.Y == y + 1;

                    if (Map[y][x] == 0)
                    {
                        selectedCharacter = roomCharacter;
                    }
                    else
                    {
                        selectedCharacter = wallCharacter;
                    }

                    if (isOpenNode)
                    {
                        selectedCharacter = openNodeCharacter;
                    }

                    if (isClosedNode)
                    {
                        selectedCharacter = closedNodeCharacter;
                    }

                    if (isPath)
                    {
                        selectedCharacter = pathCharacter;
                    }

                    if (isStartNode)
                    {
                        selectedCharacter = startNodeCharacter;
                    }

                    if (isEndNode)
                    {
                        selectedCharacter = endNodeCharacter;
                    }

                    Console.Write(selectedCharacter);
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<List<int>> map = new List<List<int>> {
                new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                new List<int> { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                new List<int> { 1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
                new List<int> { 1, 0, 1, 0, 1, 0, 0, 1, 0, 1},
                new List<int> { 1, 0, 1, 0, 1, 1, 0, 1, 0, 1},
                new List<int> { 1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
                new List<int> { 1, 0, 1, 0, 1, 1, 0, 1, 0, 1},
                new List<int> { 1, 0, 1, 0, 1, 0, 0, 0, 0, 1},
                new List<int> { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            };

            Node startNode = new Node { X = 2, Y = 3 };
            Node endNode = new Node { X = 9, Y = 5 };

            AStar aStar = new AStar(map, startNode, endNode);
            //aStar.ShowMapDuringCalculation = true;
            //aStar.SleepBetwenDraws = 500;

            aStar.CalculatePath();
            List<Node> path = aStar.GetPath();

            aStar.ShowMap(path);

            if (path.Any())
            {
            }
            else
            {
                Console.WriteLine("The path can't be found between the start and end Node!");
            }

            Console.ReadLine();
        }
    }
}

