using System.Collections.Generic;

namespace TreeApp
{
    public class BinarySearchTree
    {
        public Node root { get; set; }

        public BinarySearchTree()
        {
            root = new Node();
        }
        public void Insert(Node node, int data)
        {
            if (node.Data == null)
            {
                node.Data = data;
            }
            else
            {
                if (data < node.Data)
                {
                    if (node.Left == null)
                    {
                        node.Left = new Node();
                    }
                    Insert(node.Left, data);
                }
                else
                {
                    if (node.Right == null)
                    {
                        node.Right = new Node();
                    }
                    Insert(node.Right, data);
                }
            }
        }

        public void Delete(Node rootNode, int nodeValue)
        {
            Node selectedNode = Search(rootNode, nodeValue);

            if (selectedNode.Left == null && selectedNode.Right == null)
            {
                Node parentNode = GetParentNode(rootNode, selectedNode);

                if (parentNode.Left.Data == selectedNode.Data)
                {
                    parentNode.Left = null;
                }
                else
                {
                    parentNode.Right = null;
                }
            }
            else
            {
                if ((selectedNode.Left == null && selectedNode.Right != null) ||
                    (selectedNode.Left != null && selectedNode.Right == null))
                {
                    Node parentNode = GetParentNode(rootNode, selectedNode);
                    Node childNode = null;

                    if (selectedNode.Left == null && selectedNode.Right != null)
                    {
                        childNode = selectedNode.Right;
                    }
                    else
                    {
                        childNode = selectedNode.Left;
                    }

                    if (parentNode.Left.Data == selectedNode.Data)
                    {
                        parentNode.Left = childNode;
                    }
                    else
                    {
                        parentNode.Right = childNode;
                    }
                }
                else
                {
                    // This Node have left and right child as well.
                    
					/*
                    Node nextNode = NextNode(root, selectedNode);
                    Node nextNodeParent = GetParentNode(rootNode, nextNode);

                    Node selectedNodeParent = GetParentNode(rootNode, selectedNode);

                    int? nextNodeValue = nextNode.Data;
                    int? selectedNodeValue = selectedNode.Data;

                    if (selectedNodeParent.Left.Data == selectedNode.Data)
                    {
                        selectedNodeParent.Left.Data = nextNodeValue;
                    }
                    else
                    {
                        selectedNodeParent.Right.Data = nextNodeValue;
                    }

                    if (nextNodeParent.Left.Data == nextNode.Data)
                    {
                        nextNodeParent.Left.Data = selectedNodeValue;
                    }
                    else
                    {
                        nextNodeParent.Right.Data = selectedNodeValue;
                    }

                    //Delete(nextNode, nodeValue); VS Delete(rootNode, nodeValue);
                    Delete(nextNode, nodeValue);
                    // nextNode esetén a parentet nem fogja találni
                    // rootNode esetén viszont magát az értéket nem fogja találni a search(), mert meg van sértve a keresőfa az értékek felcserélése miatt
                    // ráadásúl a root jelenleg nem törölhető semmiképp, meert annak nincs parentje, ezért a progi elhasal.
					*/
                }
            }
        }

        //It is called as Successor
        public Node NextNode(Node rootNode, Node selectedNode)
        {
            Node nextNode = null;

            if (selectedNode.Right != null)
            {
                nextNode = Minimum(selectedNode.Right);
            }
            else
            {
                bool selectedNodeIsActual = false;

                while (rootNode != null && !selectedNodeIsActual)
                {
                    if (selectedNode.Data < rootNode.Data)
                    {
                        nextNode = rootNode;
                        rootNode = rootNode.Left;
                    }
                    else
                    {
                        if (selectedNode.Data > rootNode.Data)
                        {
                            rootNode = rootNode.Right;
                        }
                        else
                        {
                            selectedNodeIsActual = true;
                        }
                    }
                }
            }

            return nextNode;
        }

        //It is called as predecessor
        public Node PreviousNode(Node rootNode, Node selectedNode)
        {
            Node nextNode = null;

            if (selectedNode.Left != null)
            {
                nextNode = Maximum(selectedNode.Left);
            }
            else
            {
                bool selectedNodeIsActual = false;

                while (rootNode != null && !selectedNodeIsActual)
                {
                    if (selectedNode.Data < rootNode.Data)
                    {
                        rootNode = rootNode.Left;
                    }
                    else
                    {
                        if (selectedNode.Data > rootNode.Data)
                        {
                            nextNode = rootNode;
                            rootNode = rootNode.Right;
                        }
                        else
                        {
                            selectedNodeIsActual = true;
                        }
                    }
                }
            }

            return nextNode;
        }

        public List<int?> Inorder(Node node)
        {
            List<int?> list = new List<int?>();

            if (node.Left != null)
            {
                list.AddRange(Inorder(node.Left));
            }

            list.Add(node.Data);

            if (node.Right != null)
            {
                list.AddRange(Inorder(node.Right));
            }

            return list;
        }

        public List<int?> Preorder(Node node)
        {
            List<int?> list = new List<int?>();

            list.Add(node.Data);

            if (node.Left != null)
            {
                list.AddRange(Preorder(node.Left));
            }

            if (node.Right != null)
            {
                list.AddRange(Preorder(node.Right));
            }

            return list;
        }

        public List<int?> Postorder(Node node)
        {
            List<int?> list = new List<int?>();

            if (node.Left != null)
            {
                list.AddRange(Postorder(node.Left));
            }

            if (node.Right != null)
            {
                list.AddRange(Postorder(node.Right));
            }

            list.Add(node.Data);

            return list;
        }

        public Node Minimum(Node node)
        {
            if (node.Left != null)
            {
                return Minimum(node.Left);
            }
            else
            {
                return node;
            }
        }

        public Node Maximum(Node node)
        {
            if (node.Right != null)
            {
                return Maximum(node.Right);
            }
            else
            {
                return node;
            }
        }

        public Node Search(Node rootNode, int searchValue)
        {
            Node resultNode = null;

            if (rootNode != null)
            {
                if (rootNode.Data == searchValue)
                {
                    resultNode = rootNode;
                }
                else
                {
                    if (searchValue < rootNode.Data)
                    {
                        resultNode = Search(rootNode.Left, searchValue);
                    }
                    else
                    {
                        resultNode = Search(rootNode.Right, searchValue);
                    }
                }
            }

            return resultNode;
        }

        public Node GetParentNode(Node rootNode, Node selectedNode)
        {
            Node parentNode = null;

            bool selectedNodeIsActual = false;

            while (rootNode != null && !selectedNodeIsActual)
            {
                if (selectedNode.Data < rootNode.Data)
                {
                    parentNode = rootNode;
                    rootNode = rootNode.Left;
                }
                else
                {
                    if (selectedNode.Data > rootNode.Data)
                    {
                        parentNode = rootNode;
                        rootNode = rootNode.Right;
                    }
                    else
                    {
                        selectedNodeIsActual = true;
                    }
                }
            }

            return parentNode;
        }
    }
}
