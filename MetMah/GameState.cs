using MetMah.Additionally;
using MetMah.Creature;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MetMah
{
    public class GameState
    {
        public Level CurrentLevel { get; private set; }
        public int IndexCurrentLevel { get; private set; }
        private Level[] Levels;
        public List<CreatureAction> Actions { get; private set; }
        public bool IsGameOver { get; private set; }
        public Dialogue CurrentDialogue { get; private set; }
        public bool IsDialogueActivated => CurrentDialogue != null;
        public int PatienceScale { get; private set; }
        public int WidthCurrentLevel => CurrentLevel.Width;
        public int HeightCurrentLevel => CurrentLevel.Height;
        public GameStage Stage { get; private set; }

        public event Action<GameStage> StageChanged;

        public GameState(IEnumerable<Level> levels)
        {
            Stage = GameStage.NotStarted;
            Levels = levels.ToArray();
            CurrentLevel = Levels[0];
            Actions = new List<CreatureAction>();
            PatienceScale = CurrentLevel.Height * CurrentLevel.Width * 2;
        }

        public GameState()
        {
            Initialize();
        }

        public void ChoiceCharacter() => ChangeStage(GameStage.ChoiceCharacter);

        public void Start() => ChangeStage(GameStage.Play);

        public void SetKeyPressed(Keys key) => CurrentLevel.KeyPressed = key;

        public void BeginAct()
        {
            if (IsDialogueActivated)
                return;
            Actions.Clear();
            for (var x = 0; x < CurrentLevel.Width; x++)
                for (var y = 0; y < CurrentLevel.Height; y++)
                {
                    var creatures = CurrentLevel.GetCreatures(x, y).ToArray();
                    if (creatures == null) continue;

                    for (int i = 0; i < creatures.Length; i++)
                    {
                        if (creatures[i] == null) continue;
                        var command = creatures[i].Act(CurrentLevel, x, y);

                        if (x + command.DeltaX < 0 || x + command.DeltaX >= CurrentLevel.Width || y + command.DeltaY < 0 ||
                            y + command.DeltaY >= CurrentLevel.Height)
                            throw new Exception($"The object {creatures[i].GetType()} falls out of the game field");

                        Actions.Add(
                            new CreatureAction
                            {
                                Command = command,
                                Creature = creatures[i],
                                Location = new Point(x, y),
                                TargetLogicalLocation = new Point(x + command.DeltaX, y + command.DeltaY)
                            });
                        creatures = CurrentLevel.GetCreatures(x, y).ToArray();
                    }
                }
        }

        public void EndAct()
        {
            if (PatienceScale-- <= 0)
            {
                IsGameOver = true;
                ChangeStage(GameStage.Finished);
            }
            if (IsDialogueActivated)
            {
                PatienceScale -= 2;
                var index = (int)CurrentLevel.KeyPressed - 49;
                if (index < 0 || index >= CurrentDialogue.CountAnswers)
                    return;
                if (CurrentDialogue.IsCorrectAnswer(index))
                {
                    ChangeStage(GameStage.Play);
                    SetKeyPressed(Keys.None);
                    CurrentDialogue = null;
                }
                else
                {
                    PatienceScale -= 50;
                    SetKeyPressed(Keys.None);
                }
                return;
            }
            var creaturesPerLocation = GetCandidatesPerLocation();
            for (var x = 0; x < CurrentLevel.Width; x++)
                for (var y = 0; y < CurrentLevel.Height; y++)
                    CurrentLevel.SetCreatures(x, y, SelectWinnerCandidatePerLocation(creaturesPerLocation, x, y));

            if (CurrentLevel.IsOver)
            {
                if (IndexCurrentLevel + 1 < Levels.Length)
                {
                    IndexCurrentLevel += 1;
                    CurrentLevel = Levels[IndexCurrentLevel];
                    PatienceScale = CurrentLevel.Height * CurrentLevel.Width * 2;
                }
                else
                {
                    IsGameOver = true;
                    ChangeStage(GameStage.Finished);
                }
            }

            if (PatienceScale < 0)
                PatienceScale = 0;
        }

        private List<ICreature> SelectWinnerCandidatePerLocation(List<ICreature>[,] creatures, int x, int y)
        {
            var candidates = creatures[x, y];
            var aliveCandidates = candidates.ToList();
            foreach (var candidate in candidates)
                foreach (var rival in candidates)
                    if (rival != candidate && candidate.IsConflict(rival))
                        ResolvingConflict(aliveCandidates, candidate, rival);

            return aliveCandidates;
        }

        private void ResolvingConflict(List<ICreature> aliveCandidates, ICreature candidate, ICreature rival)
        {
            if (candidate is Beer && rival is Player)
            {
                aliveCandidates.Remove(candidate);
                CurrentLevel.CountBeer--;
            }

            if (candidate is Student && rival is Player)
            {
                CurrentDialogue = (candidate as Student).Dialogue;
                ChangeStage(GameStage.ActivatedDialogue);
            }

            if (candidate is CleverStudent && rival is Player)
            {
                CurrentDialogue = (candidate as CleverStudent).Dialogue;
                ChangeStage(GameStage.ActivatedDialogue);
            }
        }

        private List<ICreature>[,] GetCandidatesPerLocation()
        {
            var creatures = new List<ICreature>[CurrentLevel.Width, CurrentLevel.Height];
            for (var x = 0; x < CurrentLevel.Width; x++)
                for (var y = 0; y < CurrentLevel.Height; y++)
                    creatures[x, y] = new List<ICreature>();
            foreach (var e in Actions)
            {
                var x = e.TargetLogicalLocation.X;
                var y = e.TargetLogicalLocation.Y;
                var creature = e.Creature;
                creatures[x, y].Add(creature);
            }

            return creatures;
        }

        private void ChangeStage(GameStage stage)
        {
            this.Stage = stage;
            StageChanged?.Invoke(stage);
        }

        public void Initialize()
        {
            var levels = new List<Level>();
            string str = @"
           TTTTT  SB  CTTTTT
  P           L LTTTTTLTTTTT
TTTLTTTTL  TTTLTT     L  TTT
   L S  LLB   L       L  TTT
TTTTTTTTLTTTTTTTTTTTTLL  TTT
        L            TTTTTTT
      S L S            B TTT
 B   LTTTTTTTTTTTTTTTTTTTTTT
TTTLTT                      
   LTTTTTTTTTTTTTTTTTTTTTTTT
   L                        
                       BBB  
TTTTTTTTTTTTTTTTTTTTTTTTTTTT";
            var str1 = @"
P     S      B   
TTTTTTTTTTTTTTTTT";
            var level = new Level(str);
            var level1 = new Level(str1);
            levels.Add(level);
            levels.Add(level1);
            Levels = levels.ToArray();

            CurrentLevel = Levels[0];
            IndexCurrentLevel = 0;
            Actions = new List<CreatureAction>();
            IsGameOver = false;
            CurrentDialogue = null;
            PatienceScale = CurrentLevel.Height * CurrentLevel.Width * 2;

            ChangeStage(GameStage.NotStarted);
        }
    }
}
