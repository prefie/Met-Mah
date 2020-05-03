using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using MetMah.Additionally;

namespace Tests
{
    [TestFixture]
    class AdditionalSpecification
    {
        [Test]
        public void IsCorrectAnswer_ShouldReturnTrue_WhenAnswerCorrect()
        {
            var dialogue = new Dialogue("Do you love me?", new[] { "No", "Yes" }, "Yes");
            dialogue.IsCorrectAnswer(1).Should().BeTrue();
            dialogue.IsCorrectAnswer("Yes").Should().BeTrue();
        }

        [Test]
        public void IsCorrectAnswer_ShouldReturnFalse_WhenAnswerIncorrect()
        {
            var dialogue = new Dialogue("Do you love me?", new[] { "Yes", "No" }, 0);
            dialogue.IsCorrectAnswer(1).Should().BeFalse();
            dialogue.IsCorrectAnswer("No").Should().BeFalse();
        }

        [Test]
        public void GetAnswers_ShouldReturnAnswers()
        {
            var answers = new[] { "Yes", "No", "Maybe" };
            var dialogue = new Dialogue("Do you love me", answers, 0);
            dialogue.GetAnswers().Should().BeEquivalentTo(answers);
        }

        [Test]
        public void SinglyLinkedList_ShouldCorrectEnumerate()
        {
            var answer = new List<int> { 3, 2, 1 };
            var oneElement = new SinglyLinkedList<int>(1);
            var twoElement = new SinglyLinkedList<int>(2, oneElement);
            var threeElement = new SinglyLinkedList<int>(3, twoElement);
            var result = new List<int>();
            foreach (var e in threeElement)
                result.Add(e);
            Assert.AreEqual(result, answer);
        }

        [Test]
        public void SingltLinkedList_ShouldCorrectReverse()
        {
            var answer = new List<string> { "1", "2", "3" };
            var oneElement = new SinglyLinkedList<string>("1");
            var twoElement = new SinglyLinkedList<string>("2", oneElement);
            var threeElement = new SinglyLinkedList<string>("3", twoElement);
            threeElement = threeElement.Reverse();
            var result = new List<string>();
            foreach (var e in threeElement)
                result.Add(e);
            Assert.AreEqual(result, answer);
        }
    }
}
