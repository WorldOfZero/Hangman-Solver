using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HangmanSolver
{
    class TextSearcher
    {
        private IEnumerable<string> possibleGuesses;

        public IEnumerable<string> PossibleSolutions
        {
            get { return possibleGuesses; }
        }

        public TextSearcher(String file)
        {
            possibleGuesses = LoadFile(file);
        }

        private List<string> LoadFile(string file)
        {
            var words = File.ReadAllLines(file);
            return new List<string>(words);
        }

        public void FilterCount(int wordSize)
        {
            possibleGuesses = possibleGuesses.Where(word => word.Length == wordSize).ToList();
        }

        public void Guess(char guess, string result)
        {
            if (result.Contains(guess))
            {
                Match(result);
            }
            else
            {
                RemoveWithCharacter(guess);
            }
        }

        private void RemoveWithCharacter(char removeWithCharacter)
        {
            possibleGuesses = possibleGuesses.Where(word => !word.Contains(removeWithCharacter)).ToList();
        }

        private void Match(string partialWord)
        {
            possibleGuesses = possibleGuesses.Where(word => word.MatchesString(partialWord)).ToList();
        }

        public char CalculateGuess(string currentWord)
        {
            var availableCharacters = new Dictionary<char, int>();
            foreach (var possibleGuess in possibleGuesses)
            {
                foreach (var c in possibleGuess.Distinct())
                {
                    if (availableCharacters.ContainsKey(c))
                    {
                        var value = availableCharacters[c];
                        availableCharacters.Remove(c);
                        availableCharacters.Add(c, value + 1);
                    }
                    else
                    {
                        availableCharacters.Add(c, 1);
                    }
                }
            }

            foreach (var c in currentWord)
            {
                if (availableCharacters.ContainsKey(c))
                {
                    availableCharacters.Remove(c);
                }
            }

            int targetValue = possibleGuesses.Count() / 2;
            char bestChar = '_';
            int bestValue = 0;
            foreach (var entry in availableCharacters)
            {
                if (bestValue == 0)
                {
                    bestChar = entry.Key;
                    bestValue = entry.Value;
                    continue;
                }

                int currentDifference = Math.Abs(targetValue - bestValue);
                int entryDifference = Math.Abs(targetValue - entry.Value);

                if (entryDifference < currentDifference)
                {
                    bestChar = entry.Key;
                    bestValue = entry.Value;
                }
            }

            return bestChar;
        }

        public bool HasSolution()
        {
            return possibleGuesses.Count() == 1;
        }

        public string Solution()
        {
            return possibleGuesses.First();
        }
    }
}
