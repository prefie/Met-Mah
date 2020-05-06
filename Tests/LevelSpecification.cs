using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using MetMah.Creature;
using MetMah.Additionally;

namespace Tests
{
    [TestFixture]
    class LevelSpecification
    {
        private Level level;
        private Dialogue dialogue;

        [SetUp]
        public void SetUp()
        {
            level = new Level("   \r\nTTL\r\nCSB\r\nTTB");
            dialogue = new Dialogue("", new string[] { "" }, 0);
        }

        [Test]
        public void GetCreature_ShouldReturnAddedCreature_AfterInitialization()
        {
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 4; y++)
                    level.GetCreatures(0, 1).Should().HaveCount(1);

            level.GetCreatures(0, 1).Should().AllBeOfType(typeof(Terrain));
            level.GetCreatures(2, 1).Should().AllBeOfType(typeof(Stairs));
            level.GetCreatures(0, 2).Should().AllBeOfType(typeof(CleverStudent));
            level.GetCreatures(1, 2).Should().AllBeOfType(typeof(Student));
            level.GetCreatures(2, 2).Should().AllBeOfType(typeof(Beer));
        }

        [Test]
        public void GetCreature_ShouldReturnAddedCreature_AfterAddCreatureInEmptyCell()
        {
            var player = new Player();
            var student = new Student(dialogue);
            level.AddCreature(0, 0, player);
            level.GetCreatures(0, 0).Should().HaveCount(1).And.AllBeOfType(typeof(Player));
            level.AddCreature(0, 0, student);
            level.GetCreatures(0, 0).Should().HaveCount(2).And.BeEquivalentTo(player, student);
        }

        [Test]
        public void GetCreature_ShouldReturnCreature_AfterAddCreatureInNotEmptyCell()
        {
            var student = new Student(dialogue);
            level.AddCreature(2, 1, student);
            level.GetCreatures(2, 1).Should().HaveCount(2).And.BeEquivalentTo(student, new Stairs());
        }

        [Test]
        public void CheckCreature_ShouldReturnTrue_WhenCreatureInCell()
        {
            level.CheckCreature(2, 2, typeof(Beer)).Should().BeTrue();
        }

        [Test]
        public void CheckCreature_ShouldReturnFalse_WhenCreatureNotInCell()
        {
            level.CheckCreature(0, 0, typeof(Player)).Should().BeFalse();
        }

        [Test]
        public void GetCreature_ShouldBeEmpty_AfterRemoveCreature()
        {
            level.RemoveCreature(0, 1, typeof(Terrain));
            level.GetCreatures(0, 1).Should().BeEmpty();
        }

        [Test]
        public void CountBeer_ShouldReturnZero_IfBeerNotFound()
        {
            var testLevel = new Level("   \r\nTTT");
            testLevel.CountBeer().Should().Be(0);
        }

        [Test]
        public void CountBeer_ShouldReturnCorrectCount_WhenBeerOnMap()
        {
            level.CountBeer().Should().Be(2);
        }

        [Test]
        public void IsOver_ShouldReturnFalse_WhenBeerOnMap()
        {
            level.IsOver.Should().BeFalse();
        }

        [Test]
        public void IsOver_ShouldReturnTrue_IfBeerNotFound()
        {
            level.RemoveCreature(2, 2, typeof(Beer));
            level.RemoveCreature(2, 3, typeof(Beer));
            level.IsOver.Should().BeTrue();
        }

        [Test]
        public void SetCreatures_ShouldAddCreaturesInCell_WhenCellEmpty()
        {
            var newCreatures = new List<ICreature> { new Student(dialogue), new Terrain(), new Beer() };
            level.SetCreatures(0, 0, newCreatures);
            level.GetCreatures(0, 0).Should().BeEquivalentTo(newCreatures);
        }

        [Test]
        public void SetCreatures_ShouldReplaceAllCreaturesInCell()
        {
            var newCreatures = new List<ICreature> { new Player(), new Beer(), new Stairs() };
            level.SetCreatures(0, 1, newCreatures);
            level.GetCreatures(0, 1).Should().BeEquivalentTo(newCreatures);
        }
    }
}
