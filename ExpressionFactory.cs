using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraApp1
{
    public class ExpressionFactory
    {
        private static Random Random = new Random();
        private static List<char> TokenNames = new List<char> { 'p', 'q', 'k', 'y', 'z' };

        private ExpressionFactory() { }

        public static Expression GenerateRandomExpression(int tokenTypeCount, bool useNamelessToken)
        {
            Expression resultExpression = new Expression();
            SortedSet<char> tokenNamesInUse = new SortedSet<char>();

            //generate one token for each type
            for (int x = 0; x < tokenTypeCount; x++)
            {
                char nextTokenName = GetUnusedTokenName(tokenNamesInUse);
                tokenNamesInUse.Add(nextTokenName);
                resultExpression.AddExpressionToken(
                    new ExpressionToken(nextTokenName, GenerateTokenValue()));
            }

            //if nameless token is in use, generate an instance of that too
            if (useNamelessToken)
            {
                resultExpression.AddExpressionToken(
                    new ExpressionToken(null, GenerateTokenValue()));
            }

            //and add some extra tokens otherwise the question and answer would be the same!
            for (int x = 0; x < Random.Next(1, 4); x++)
            {
                resultExpression.AddExpressionToken(
                    new ExpressionToken(GetRandomTokenInUse(tokenNamesInUse, useNamelessToken), GenerateTokenValue()));
            }

            resultExpression.Shuffle();

            return resultExpression;

        }

        private static char GetUnusedTokenName(SortedSet<char> tokenNamesInUse)
        {
            if (TokenNames.Count == tokenNamesInUse.Count)
            {
                throw new InvalidOperationException("all token names are in use");
            }

            while (true)
            {
                char selection = TokenNames[Random.Next(0, TokenNames.Count)];
                if (!tokenNamesInUse.Contains(selection))
                {
                    return selection;
                }
            }
        }

        private static int GenerateTokenValue()
        {
            return Random.Next(1, 11) - 5;
        }

        private static char? GetRandomTokenInUse(SortedSet<char> tokenNamesInUse, bool useNamelessToken)
        {
            int countPossibleOptions = tokenNamesInUse.Count;
            if (useNamelessToken)
            {
                countPossibleOptions++;
            }

            int index = Random.Next(0, countPossibleOptions);

            if (index == tokenNamesInUse.Count)
            {
                return null;
            }

            return tokenNamesInUse.ElementAt(index);
        }

    }
}
