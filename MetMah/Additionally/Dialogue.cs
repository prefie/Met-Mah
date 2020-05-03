using System;
using System.Collections.Generic;

namespace MetMah.Additionally
{
    public class Dialogue
    {
        public readonly string Text;
        private readonly string[] answers;
        private readonly string correctAnswer;

        public int CountAnswers => answers.Length;

        public Dialogue(string text, string[] answers, string correctAnswer)
        {
            Text = text;
            this.answers = answers;
            this.correctAnswer = correctAnswer;
        }

        public Dialogue(string text, string[] answers, int index)
        {
            Text = text;
            this.answers = answers;
            correctAnswer = answers[index];
        }

        public IEnumerable<string> GetAnswers()
        {
            foreach (var e in answers)
                yield return e;
        }

        public bool IsCorrectAnswer(string answer) => answer == correctAnswer;
        public bool IsCorrectAnswer(int index)
        {
            if (index < 0 || index >= answers.Length)
                throw new ArgumentException();
            return answers[index] == correctAnswer;
        }
    }
}
