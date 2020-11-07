using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AlgebraApp1
{
    public class ExpressionToken: IEquatable<ExpressionToken>
    {
        public char? Name { get; }
        public int Value { get; set; }

        private static readonly string ValidNames = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public ExpressionToken(char? tokenName, int value)
        {
            this.Name = tokenName;
            this.Value = value;
        }

        public String FormatElement()
        {
            if(Name == null)
            {
                return Math.Abs(Value).ToString();   // eg 5
            }
            else
            {
                if(Value != 1 && Value != -1)
                {
                    return Math.Abs(Value).ToString() + Name;  // eg 5k
                }
                else
                {
                    return Name+""; //eg k
                }
                
            }
        }

        public static bool IsValidName(char name)
        {
            return ValidNames.Contains(name);
        }

        public bool Equals([AllowNull] ExpressionToken other)
        {
            if ((other.Name == null && Name == null)
                || other.Name == Name)
            {
                if (other.Value == Value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
