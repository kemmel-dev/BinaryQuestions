using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryQuestions
{
    class Program
    {
        static BTTree tree;
        static void Main(string[] args)
        {
            //There's no need to ask for the initial data when it already exists
            if (File.Exists("serialized.bin"))
                tree = new BTTree();
            else
                startNewGame();

            Console.WriteLine("\nStarting the \"20 Binary Questions\" Game!\nThink of an object, person or animal.");
            tree.query(); //play one game
            while(playAgain())
            {
                Console.WriteLine("\nThink of an object, person or animal.");
                Console.WriteLine();
                tree.query(); //play one game
            }
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

        static void TraversePreOrder(BTNode rootNode)
        {
            if (rootNode != null)
            {
                Console.WriteLine(rootNode.GetMessage());
                Console.WriteLine(Evaluate(rootNode));
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

        static bool playAgain()
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

        static void startNewGame()
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
