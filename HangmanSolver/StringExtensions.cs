using System;
using System.Collections.Generic;
using System.Text;

namespace HangmanSolver
{
    static class StringExtensions
    {
        private const char wildcard = '_';

        public static bool MatchesString(this string origin, string match)
        {
            for (int i = 0; i < origin.Length; ++i)
            {
                if (match[i] == wildcard)
                {
                    continue;
                }
                else if (match[i] != origin[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
