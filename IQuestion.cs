using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraApp1
{
    interface IQuestion
    {
        string GetQuestion();

        bool SubmitAnswer(String answer);

        string GetSubmittedAnswer();

        bool WasAnsweredCorrectly();

        string GetAnswer();
    }
}
