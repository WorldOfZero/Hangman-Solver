using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangmanSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentResult = "";
            Console.WriteLine("Hangman Solver");
            Console.WriteLine("How many characters are in the word?");
            int characterCount = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < characterCount; ++i)
            {
                currentResult += "_";
            }

            var search = new TextSearcher("dictionary.txt");
            search.FilterCount(characterCount);
            Console.WriteLine("Current Word: {0} | The current best guess is: {1}", currentResult, search.CalculateGuess(currentResult));

            while (currentResult.Contains("_"))
            {
                Console.WriteLine("What was your guess?");
                string guess = Console.ReadLine();
                if (guess.Length != 1)
                {
                    Console.WriteLine("Are you sure?");
                    continue;
                }
                Console.WriteLine("What is the current word result? (Underscores for unknown)");
                string result = Console.ReadLine();
                if (result.Length != characterCount)
                {
                    Console.WriteLine("Are you sure?");
                    continue;
                }

                currentResult = result;
                search.Guess(guess[0], result);
                Console.WriteLine("Currently there are {0} choices including {1}",
                    search.PossibleSolutions.Count(), StringifyGuesses(search.PossibleSolutions));
                Console.WriteLine("Current Word: {0} | The current best guess is: {1}", currentResult, search.CalculateGuess(currentResult));

                if (search.HasSolution())
                {
                    Console.WriteLine("Solution Found! {0}", search.Solution());
                }
            }

            Console.ReadLine();
        }

        private static Random r = new Random();
        private static string StringifyGuesses(IEnumerable<string> searchPossibleSolutions)
        {
            var somePossibilities = searchPossibleSolutions.OrderBy(obj => r.Next()).Take(4); //4 possible random solutions
            StringBuilder builder = new StringBuilder();
            foreach (var possibility in somePossibilities)
            {
                builder.Append(possibility);
                builder.Append(", ");
            }
            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }
    }
}