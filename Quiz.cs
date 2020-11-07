using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraApp1
{
    class Quiz
    {
        private List<IQuestion> QuizQuestions;
        private int CurrentQuestionIndex = -1;

        public Quiz(List<IQuestion> questions)
        {
            this.QuizQuestions = questions;
        }

        public IQuestion GetCurrentQuestion()
        {
            if (CurrentQuestionIndex >= 0)
            {
                return QuizQuestions[CurrentQuestionIndex];
            } 
            else
            {
                return null;
            }
        }

        public bool GoToNextQuestion()
        {
            if(CurrentQuestionIndex >= QuizQuestions.Count - 1)
            {
                return false;
            }
            else
            {
                CurrentQuestionIndex++;
                return true;
            }
        }

        public string GetScore()
        {
            int correctAnswers = 0;

            foreach (IQuestion question in QuizQuestions)
            {
                if(question.WasAnsweredCorrectly())
                {
                    correctAnswers++;
                }
            }

            return $"{correctAnswers} out of {QuizQuestions.Count}";
        }
    }
}
