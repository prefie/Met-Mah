using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EscapeFromMetMah;
using FluentAssertions;
using MetMah.Additionally;
using MetMah.Creature;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class CreaturesOnLevelSpecification
    {
        private Level level;
        private Dialogue dialogue;

        [SetUp]
        public void SetUp()
        {
            level = new Level(" S   \r\nTTTTT\r\n C   \r\nTTTLT\r\n  PLB\r\nTTTTT");
            dialogue = new Dialogue("", new string[] { "" }, 0);
        }

        [Test]
        public void ActStudent_ShouldReturnOffsetOnX()
        {
            var point = new Point(1, 0);
            var student = level.GetCreatures(point.X, point.Y).FirstOrDefault();
            student.Should().NotBeNull();
            for (int i = 0; i < 1000; i++)
            {
                var action = student.Act(level, point.X, point.Y);
                action.DeltaX.Should().BeInRange(-1, 1);
                level.RemoveCreature(point.X, point.Y, student.GetType());
                point.X += action.DeltaX;
                point.Y += action.DeltaY;
                level.AddCreature(point.X, point.Y, student);
            }
            level.GetCreatures(point.X, point.Y).Any(x => x is Student).Should().BeTrue();
        }

        [Test]
        public void ActCleverStudent_ShouldReturnOffsetToPlayer_WhenPlayerFound()
        {
            var point = new Point(1, 2);
            var cleverStudent = level.GetCreatures(point.X, point.Y).FirstOrDefault();
            cleverStudent.Should().NotBeNull();
            var answer = new Point[] { new Point(1, 2), new Point(2, 2), new Point(3, 2),
                new Point(3, 3), new Point(3, 4), new Point(2, 4)};
            for (int i = 1; i < 6; i++)
            {
                var action = cleverStudent.Act(level, point.X, point.Y);
                level.RemoveCreature(point.X, point.Y, cleverStudent.GetType());
                point.X += action.DeltaX;
                point.Y += action.DeltaY;
                point.Should().Be(answer[i]);
                level.AddCreature(point.X, point.Y, cleverStudent);
            }
            level.GetCreatures(point.X, point.Y).Any(x => x is CleverStudent).Should().BeTrue();
        }

        [Test]
        public void ActCleverStudent_ShouldReturnZeroOffset_WhenPlayerNotFound()
        {
            var testLevel = new Level(" C   \r\nTTTTT\r\n   P \r\nTTTTT");
            var point = new Point(1, 0);
            var cleverStudent = testLevel.GetCreatures(point.X, point.Y).FirstOrDefault();
            cleverStudent.Should().NotBeNull();
            for (int i = 0; i < 3; i++)
                CheckOffset(cleverStudent, point.X, point.Y, 0, 0);
        }

        [Test]
        public void ActPlayer_ShouldReturnNotZeroOffset_WhenKeyPressed()
        {
            level.KeyPressed = Keys.Left;
            var point = new Point(2, 4);
            var player = level.GetCreatures(point.X, point.Y).FirstOrDefault();
            player.Should().NotBeNull();
            for (int i = 0; i < 2; i++)
            {
                var actions = player.Act(level, point.X, point.Y);
                level.RemoveCreature(point.X, point.Y, typeof(Player));
                point.X += actions.DeltaX;
                point.Y += actions.DeltaY;
                level.AddCreature(point.X, point.Y, player);
            }
            point.Should().Be(new Point(0, 4));
        }

        [Test]
        public void ActStaticCreatures_ShouldReturnZeroOffset_Always()
        {
            var beer = level.GetCreatures(4, 4).First();
            var stairs = level.GetCreatures(3, 3).First();
            var terrain = level.GetCreatures(1, 1).First();
            for (int i = 0; i < 1000; i++)
            {
                CheckOffset(beer, 4, 4, 0, 0);
                CheckOffset(stairs, 3, 3, 0, 0);
                CheckOffset(terrain, 1, 1, 0, 0);
            }
        }

        [Test]
        public void IsConflict_ShouldReturnTrue_WhenConflictStudentAndPython() =>
            CheckConflict(new Student(dialogue), new Python());

        [Test]
        public void IsConflict_SholdReturnTrue_WhenConflictCleverStudentAndPython() =>
            CheckConflict(new CleverStudent(dialogue), new Python());

        [Test]
        public void IsConflict_SholdReturnTrue_WhenConflictPlayerAndStudent() =>
            CheckConflict(new Student(dialogue), new Player());

        [Test]
        public void IsConflict_SholdReturnTrue_WhenConflictPlayerAndCleverStudent() =>
            CheckConflict(new CleverStudent(dialogue), new Player());

        [Test]
        public void IsConflict_SholdReturnTrue_WhenConflictPlayerAndBeer() =>
            CheckConflict(new Beer(), new Player());

        [Test]
        public void GetStatus_ShouldReturnActiveForAll_AfterInitialization()
        {
            for (int x = 0; x < level.Width; x++)
                for (int y = 0; y < level.Width; y++)
                    level.GetCreatures(x, y).All(z => z.GetStatus() == Status.Active).Should().BeTrue();
        }

        [Test]
        public void GetStatusStudent_ShouldReturnInactive_AfterConflictWithPlayer()
        {
            var student = new Student(dialogue);
            student.IsConflict(new Player());
            student.GetStatus().Should().Be(Status.Inactive);
        }

        [Test]
        public void GetStatusStudent_ShouldReturnInactive_AfterConflictWithPython()
        {
            var student = new Student(dialogue);
            student.IsConflict(new Python());
            student.GetStatus().Should().Be(Status.Inactive);
        }

        [Test]
        public void MoveStudent_ShouldReturnFall_WhenBottomEmpty()
        {
            var testLevel = new Level("S\r\n \r\n \r\nT");
            var student = testLevel.GetCreatures(0, 0).First();
            student.Act(testLevel, 0, 0).DeltaY.Should().Be(1);
        }

        [Test]
        public void MoveCleverStudent_ShouldReturnFall_WhenBottomEmpty()
        {
            var testLevel = new Level("C\r\n \r\nT");
            var cleverStudent = testLevel.GetCreatures(0, 0).First();
            cleverStudent.Act(testLevel, 0, 0).DeltaY.Should().Be(1);
        }

        [Test]
        public void MovePlayer_ShouldReturnFall_WhenBottomEmpty()
        {
            var testLevel = new Level("P\r\n \r\n \r\nT");
            var player = testLevel.GetCreatures(0, 0).First();
            player.Act(testLevel, 0, 0).DeltaY.Should().Be(1);
        }

        [Test]
        public void MovePlayer_ShouldGetPythonInCell_WhenActivateSuperpower()
        {
            level.KeyPressed = Keys.R;
            var student = level.GetCreatures(2, 4).First();
            student.Act(level, 2, 4);
            level.GetCreatures(2, 4).Any(x => x is Python).Should().BeTrue();
        }

        private void CheckOffset(ICreature creature, int x, int y, int deltaX, int deltaY)
        {
            var action = creature.Act(level, x, y);
            action.DeltaX.Should().Be(deltaX);
            action.DeltaY.Should().Be(deltaY);
        }

        private void CheckConflict(ICreature creature1, ICreature creature2)
        {
            creature1.IsConflict(creature2).Should().BeTrue();
            creature2.IsConflict(creature1).Should().BeTrue();
        }
    }
}
