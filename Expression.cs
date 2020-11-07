using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AlgebraApp1
{
    public class Expression: IEquatable<Expression>
    {
        private List<ExpressionToken> ExpressionTokens;
        private readonly Random Random = new Random();

        public Expression()
        {
            ExpressionTokens = new List<ExpressionToken>();
        }

        public void AddExpressionToken(ExpressionToken token)
        {
            ExpressionTokens.Add(token);
        }

        public void AddAllExpressionTokens(List<ExpressionToken> tokens)
        {
            ExpressionTokens.AddRange(tokens);
        }

        public void Shuffle()
        {
            ExpressionTokens = ExpressionTokens.OrderBy(token => Random.Next()).ToList();
        }

        public String Format()
        {
            int index = 0;
            StringBuilder result = new StringBuilder();

            while (index < ExpressionTokens.Count)
            {
                ExpressionToken thisToken = ExpressionTokens[index];
                if (index == 0)
                {
                    if (thisToken.Value >= 0)
                    {
                        result.Append(thisToken.FormatElement());
                    }
                    else
                    {
                        result.Append("-" + thisToken.FormatElement());
                    }
                }
                else
                {
                    if (thisToken.Value >= 0)
                    {
                        result.Append(" + " + thisToken.FormatElement());
                    }
                    else
                    {
                        result.Append(" - " + thisToken.FormatElement());
                    }
                }
                index++;
            }

            return result.ToString();
        }

        public Expression Simplify()
        {
            Expression simplifiedExpression = new Expression();
            List<ExpressionToken> simplifiedTokens = new List<ExpressionToken>();

            foreach (ExpressionToken token in ExpressionTokens)
            {
                //already have a token of this name?
                ExpressionToken matchedToken = GetTokenWithMatchingName(simplifiedTokens, token.Name);

                if(matchedToken != null)
                {
                    matchedToken.Value += token.Value;
                }
                else
                {
                    simplifiedTokens.Add(new ExpressionToken(token.Name, token.Value));
                }
            }

            //remove any zeroes
            for(int x = simplifiedTokens.Count-1; x>=0; x--)
            {
                ExpressionToken token = simplifiedTokens[x];
                if (token.Value == 0)
                {
                    simplifiedTokens.Remove(token);
                }
            }
 
            //if everything was zero, answer is 0
            if(simplifiedTokens.Count == 0)
            {
                simplifiedTokens.Add(new ExpressionToken(null, 0));
            }

            simplifiedExpression.AddAllExpressionTokens(simplifiedTokens);
            return simplifiedExpression;
        }

        private ExpressionToken GetTokenWithMatchingName(List<ExpressionToken> list, char? name)
        {
            foreach (ExpressionToken token in list)
            {
                char? tokenName = token.Name;

                if(tokenName == null && name == null)  //if there is no name
                {
                    return token;
                }
                else if(tokenName == name)  //if there is a name and it matches
                {
                    return token;
                } 
            }
            return null;
        }

        public static Expression ParseExpression(string expression)
        {
            List<ExpressionToken> parsedTokens = new List<ExpressionToken>();

            char? parsedOperator = null;
            string parsedValue = "";
            char? parsedName = null;

            for(int index=0; index < expression.Length; index ++)
            {
                char thisChar = expression[index];          

                if(thisChar == '+' || thisChar == '-')   //plus or minus operator
                {
                    //could denote end of previous Expression token
                    if(parsedValue.Length > 0 || parsedName != null)
                    {
                        parsedTokens.Add(ToExpressionToken(parsedOperator, parsedValue, parsedName));
                        //reset parse state for next ExpressionToken
                        parsedOperator = null;
                        parsedValue = "";
                        parsedName = null;
                    }

                    if(parsedOperator != null)
                    {
                        throw new ArgumentException("Cannot have two consecutive operators");
                    } 
                    else
                    {
                        parsedOperator = thisChar;
                    }
                }
                else if (thisChar == ' ') //ignore whitespace
                {
                    continue;
                }
                else if(int.TryParse(thisChar+"", out _))
                {
                    if(parsedName != null)
                    {
                        throw new ArgumentException("Cannot have another number after token name");
                    }
                    parsedValue += thisChar;
                }
                else if(ExpressionToken.IsValidName(thisChar))
                {
                    if(parsedName == null)
                    {
                        parsedName = thisChar;
                    } 
                    else
                    {
                        throw new ArgumentException("Cannot have token names with multiple characters");
                    }
                }
                else
                {
                    throw new ArgumentException($"Didn't understand '{thisChar}'");
                }

                //if no more characters then need to add final token
                if (index == expression.Length-1)
                {
                    parsedTokens.Add(ToExpressionToken(parsedOperator, parsedValue, parsedName));
                }
            }

            Expression parsedExpression = new Expression();
            parsedExpression.AddAllExpressionTokens(parsedTokens);
            return parsedExpression;
        }

        private static ExpressionToken ToExpressionToken(char? parsedOperator, string parsedValue, char? parsedName)
        {
            if (parsedName == null && parsedValue == null)
            {
                throw new ArgumentException("ExpressionToken must have a name or a value");
            }

            char operatorChar = parsedOperator ?? '+';
            int value = (parsedValue.Length > 0) ? int.Parse(parsedValue) : 1;

            if(operatorChar == '-')
            {
                value *= -1;
            }

            return new ExpressionToken(parsedName, value);
        }

        public bool Equals([AllowNull] Expression other)
        {
            List<ExpressionToken> tokensToMatch = new List<ExpressionToken>();
            tokensToMatch.AddRange(other.ExpressionTokens);

            if (this.ExpressionTokens.Count != other.ExpressionTokens.Count)
            {
                return false;
            }

            while (tokensToMatch.Count > 0)
            {
                for (int x = 0; x < tokensToMatch.Count; x++)
                {
                    ExpressionToken token = tokensToMatch[x];
                    if (ExpressionTokens.Contains(token))
                    {
                        tokensToMatch.Remove(token);
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
