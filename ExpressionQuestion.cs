using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraApp1
{
    public class ExpressionQuestion : IQuestion
    {
        private Expression QuestionExpression;
        private Expression AnswerExpression;
        private static Random Random = new Random();
        private string SubmittedAnswer = "No Answer Given";
        private bool AnsweredCorrectly;

        public ExpressionQuestion()
        {
            int numTokenTypes = Random.Next(2, 3);
            QuestionExpression = ExpressionFactory.GenerateRandomExpression(numTokenTypes, true);
            AnswerExpression = QuestionExpression.Simplify();
        }

        string IQuestion.GetQuestion()
        {
            return QuestionExpression.Format();
        }

        string IQuestion.GetSubmittedAnswer()
        {
            return SubmittedAnswer;
        }

        bool IQuestion.SubmitAnswer(string answer)
        {
            Expression expression = Expression.ParseExpression(answer);
            SubmittedAnswer = answer;
            AnsweredCorrectly = expression.Equals(AnswerExpression);

            return AnsweredCorrectly;
        }

        bool IQuestion.WasAnsweredCorrectly()
        {
            return AnsweredCorrectly;
        }

        string IQuestion.GetAnswer()
        {
            return AnswerExpression.Format();
        }
    }
}
 