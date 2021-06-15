using System.Collections.Generic;

namespace TreeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BinarySearchTree btree = new BinarySearchTree();

            btree.Insert(btree.root, 10);
            btree.Insert(btree.root, 5);
            btree.Insert(btree.root, 2);
            btree.Insert(btree.root, 15);
            btree.Insert(btree.root, 8);

            btree.Insert(btree.root, 1);
            btree.Insert(btree.root, 9);

            Node nextNode = btree.NextNode(btree.root, btree.root.Left);
            Node prevNode = btree.PreviousNode(btree.root, btree.root.Left.Left);

            Node searchNode = btree.Search(btree.root, 15);

            Node parentNode = btree.GetParentNode(btree.root, btree.root.Left);

            List<int?> inorder = btree.Inorder(btree.root);
            List<int?> preorder = btree.Preorder(btree.root);
            List<int?> postorder = btree.Postorder(btree.root);
            Node min = btree.Minimum(btree.root);
            Node max = btree.Maximum(btree.root);

            //btree.Delete(btree.root, 5);

            foreach (int? item in inorder)
            {
                System.Console.Write("{0}, ", item);
            }

            System.Console.ReadLine();
        }
    }
}
