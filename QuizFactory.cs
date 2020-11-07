using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraApp1
{
    class QuizFactory
    {
        public static Quiz GetAlgebraQuiz(int questionCount)
        {
            List<IQuestion> questions = new List<IQuestion>();

            for(int x=0; x<questionCount; x++)
            {
                questions.Add(new ExpressionQuestion());
            }

            return new Quiz(questions);
        }
    }
}
