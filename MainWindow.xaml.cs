using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgebraApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Quiz Quiz;
        private IQuestion CurrentQuestion;

        public MainWindow()
        {
            InitializeComponent();

            Quiz = QuizFactory.GetAlgebraQuiz(30);
            RenderNextQuestion();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender == bSubmit)
            {
                ProcessSubmit();
            } 
            else if(sender == bNext)
            {
                RenderNextQuestion();
            }
        }

        private void ProcessSubmit()
        {
            var submittedAnswer = tAnswer.Text.Trim();

            if(submittedAnswer.Length == 0)
            {
                MessageBox.Show("You must submit an answer");
                return;
            }

            bool correct;

            try
            {
                correct = CurrentQuestion.SubmitAnswer(submittedAnswer);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Your answer is invalid, try again: {e.Message}");
                return;
            }

            lAnswer.Content = CurrentQuestion.GetAnswer();

            if (correct)
            {
                lMessage.Foreground = Brushes.Green;
                lMessage.Content = "Correct!";
            } 
            else
            {
                lMessage.Foreground = Brushes.Red;
                lMessage.Content = "Incorrect";
            }

            bNext.IsEnabled = true;
            bSubmit.IsEnabled = false;
        }

        private void RenderNextQuestion()
        {
            if (!Quiz.GoToNextQuestion())
            {
                EndQuiz();
                return;
            }

            CurrentQuestion = Quiz.GetCurrentQuestion();

            lQuestion.Content = CurrentQuestion.GetQuestion();

            tAnswer.Text = "";
            tAnswer.Focus();

            lAnswer.Content = "";
            lMessage.Content = "";
            bSubmit.IsEnabled = true;
            bNext.IsEnabled = false;
        }

        private void EndQuiz()
        {
            MessageBox.Show($"Quiz Finished - your score was {Quiz.GetScore()}");
            System.Environment.Exit(1);
        }
    }
}
