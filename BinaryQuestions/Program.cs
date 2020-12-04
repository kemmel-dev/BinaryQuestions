using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace BinaryQuestions
{
    class Program
    {
        static BTTree tree;

        private static List<BTNode> leafs = new List<BTNode>();

        static int calls;

        static void Main(string[] args)
        {
            //There's no need to ask for the initial data when it already exists
            if (File.Exists("serialized.bin"))
                tree = new BTTree();
            else
                StartNewGame();

            Console.WriteLine("Output Minimax maximized: " + MiniMax(tree.GetRootNode(), true) + " (with " + calls + " calls)");
            calls = 0;
            Console.WriteLine("Output Minimax + AB Pruning maximized: " + MiniMaxABPruning(tree.GetRootNode(), true, int.MinValue, int.MaxValue) + " (with " + calls + " calls)");


            foreach (BTNode node in leafs)
            {
                Console.WriteLine(node.GetMessage());
            }

            Console.WriteLine("\nStarting the \"20 Binary Questions\" Game!\nThink of an object, person or animal.");
            tree.Query(); //play one game
            while(PlayAgain())
            {
                Console.WriteLine("\nThink of an object, person or animal.");
                Console.WriteLine();
                tree.Query(); //play one game
            }
        }

        static int MiniMax(BTNode rootNode, bool isMax)
        {
            calls++;
            if (!rootNode.IsQuestion())
            {
                return Evaluate(rootNode);
            }

            if (isMax)
            {
                return Math.Max(MiniMax(rootNode.GetNoNode(), false), MiniMax(rootNode.GetYesNode(), false));
            }
            else
            {
                return Math.Min(MiniMax(rootNode.GetNoNode(), true), MiniMax(rootNode.GetYesNode(), true));
            }
        }

    static int MiniMaxABPruning(BTNode rootNode, bool isMax, int alpha, int beta)
    {
        calls++;
        if (!rootNode.IsQuestion())
        {
            return Evaluate(rootNode);
        }

        BTNode[] children = new BTNode[2];
        children[0] = rootNode.GetNoNode();
        children[1] = rootNode.GetYesNode();

        if (isMax)
        {
            int bestValue = int.MinValue;
            int value;

            foreach (BTNode child in children)
            {
                value = MiniMaxABPruning(child, false, alpha, beta);
                bestValue = Math.Max(bestValue, value);
                alpha = Math.Max(alpha, bestValue);

                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestValue;
        }
        else
        {
            int bestValue = int.MaxValue;
            int value;

            foreach (BTNode child in children)
            {
                value = MiniMaxABPruning(child, true, alpha, beta);
                bestValue = Math.Min(bestValue, value);
                beta = Math.Min(beta, bestValue);

                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestValue;
        }
    }


        struct Maximum
        {
            public Maximum(bool exists, BTNode node)
            {
                Exists = exists;
                Node = node;
            }

            public bool Exists { get; }
            public BTNode Node { get; }
        }

        static Maximum GetMaxNode(BTNode node1, BTNode node2)
        {
            int value1 = Evaluate(node1);
            int value2 = Evaluate(node2);

            if (value1 > value2)
            {
                return new Maximum(true, node1);
            }
            else if (value1 < value2)
            {
                return new Maximum(true, node2);
            }
            return new Maximum(false, null);
        }

        static int Evaluate(BTNode node)
        {
            string message = node.GetMessage();
            if (message != null)
            {
                return message.Length;
            }
            throw new ArgumentException("Tried to evaluate a null string");
        }

        static void GetLeafs(BTNode rootNode)
        {
            if (rootNode != null)
            {
                if (! rootNode.IsQuestion())
                {
                    leafs.Add(rootNode);
                }
                GetLeafs(rootNode.GetNoNode());
                GetLeafs(rootNode.GetYesNode());
            }
        }

        static void TraversePreOrder(BTNode rootNode)
        {
            if (rootNode != null)
            {
                Console.WriteLine(rootNode.GetMessage());
                TraversePreOrder(rootNode.GetNoNode());
                TraversePreOrder(rootNode.GetYesNode());
            }
        }

        static void TraverseInOrder(BTNode rootNode)
        {
            if (rootNode != null)
            {
                TraverseInOrder(rootNode.GetNoNode());
                Console.WriteLine(rootNode.GetMessage());
                TraverseInOrder(rootNode.GetYesNode());
            }
        }

        static void TraversePostOrder(BTNode rootNode)
        {
            if (rootNode != null)
            {
                TraversePostOrder(rootNode.GetNoNode());
                TraversePostOrder(rootNode.GetYesNode());
                Console.WriteLine(rootNode.GetMessage());
            }
        }

        static bool PlayAgain()
        {
            Console.Write("\nPlay Another Game? ");
            char inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
            while (inputCharacter != 'y' && inputCharacter != 'n')
            {
                Console.WriteLine("Incorrect input please enter again: ");
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }
            if (inputCharacter == 'y')
                return true;
            else
                return false;
        }

        static void StartNewGame()
        {
            Console.WriteLine("No previous knowledge found!");
            Console.WriteLine("Initializing a new game.\n");
            Console.WriteLine("Enter a question about an object, person or animal: ");
            string question = Console.ReadLine();
            Console.Write("Enter a possible guess (an object, person or animal) if the response to this question is Yes: ");
            string yesGuess = Console.ReadLine();
            Console.Write("Enter a possible guess (an object, person or animal) if the response to this question is No: ");
            string noGuess = Console.ReadLine();

            tree = new BTTree(question, yesGuess, noGuess);
        }
    }
}
