using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SharedLibrary;
using SharedLibrary.Agents;
using SharedLibrary.Frontiers;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace EightPuzzle
{
    internal class Visualizer : MonoGame.Forms.NET.Controls.MonoGameControl
    {
        private const int gapSize = 10;
        private int tileSize;

        private Texture2D blankTexture;
        private Rectangle boardRect;
        private EightPuzzleState endState;
        private EightPuzzleEnvironment environment;
        private PlanningAgent<EightPuzzleState> eightPuzzleAgent;
        private LinkedList<EightPuzzleState> displayStates;
        private LinkedListNode<EightPuzzleState> currentDisplayState;

        public TimeSpan AnimationDelay { get; set; }
        public bool IsPaused { get; set; }


        private TimeSpan animationTimer;

        protected override void Initialize()
        {
            blankTexture = new Texture2D(Editor.GraphicsDevice, width: 1, height: 1);
            blankTexture.SetData([Color.White]);

            tileSize = (Editor.GraphicsDevice.Viewport.Width - (gapSize << 2)) / 3;
            boardRect = new(0, 0, Editor.GraphicsDevice.Viewport.Width, Editor.GraphicsDevice.Viewport.Height);

            endState = new(new int[,]
            {
                { 1,2,3 },
                { 4,5,6 },
                { 7,8,0 }
            },
            new Point(2, 2));

            displayStates = [];
            environment = new EightPuzzleEnvironment();

            LoadTiles("426370158");

            while (!eightPuzzleAgent.MakeMove((state) => state == environment.GoalState)) ;

            for (var a = eightPuzzleAgent.GetFinishedState(); a != null; a = a.Predecessor)
            {
                displayStates.AddFirst(a.State);
            }
            Reset();

            AnimationDelay = TimeSpan.FromMilliseconds(250);
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsPaused) return;
            
            animationTimer += gameTime.ElapsedGameTime;

            if (animationTimer > AnimationDelay)
            {
                animationTimer = TimeSpan.Zero;

                if (currentDisplayState.Next == null) return;

                currentDisplayState = currentDisplayState.Next;
            }

        }

        protected override void Draw()
        {
            Editor.spriteBatch.Begin();

            Editor.spriteBatch.Draw(blankTexture, boardRect, Color.LightGray);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var destRect = new Rectangle(x * tileSize + (x + 1) * gapSize, y * tileSize + (y + 1) * gapSize, tileSize, tileSize);

                    Editor.spriteBatch.Draw(blankTexture, destRect, Color.White);

                    if (currentDisplayState.Value.Board[y, x] == 0) continue;

                    Editor.spriteBatch.DrawCircle(
                        destRect.Center.ToVector2(),
                        radius: tileSize * 0.45f,
                        sides: currentDisplayState.Value.Board[y, x] + 2,
                        color: Color.Black,
                        thickness: 3);
                }
            }

            Editor.spriteBatch.End();
        }

        public void GoBack()
        {
            if (currentDisplayState.Previous == null) return;

            currentDisplayState = currentDisplayState.Previous;
        }
        public void GoForward()
        {
            if (currentDisplayState.Next == null) return;

            currentDisplayState = currentDisplayState.Next;
        }
        public void Reset()
        {
            currentDisplayState = displayStates.First;
        }
        public void LoadTiles(string input)
        {
            var tiles = new int[3,3];
            var emptyTile = Point.Zero;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    tiles[r, c] = input[r * 3 + c] - '0';

                    if (input[r * 3 + c] == '0')
                    {
                        emptyTile = new Point(r, c);
                    }
                }
            }
            eightPuzzleAgent = new(
                startingState: new(tiles, emptyTile),
                frontier: new PriorityQueueFrontier<EightPuzzleState>(),
                environment: environment,
                getScore: (AgentData<EightPuzzleState> curr, HashSet<EightPuzzleState> visited, Movement<EightPuzzleState>.Result result)
                    => curr.CumulativeCost + result.Cost + EightPuzzleEnvironment.DistanceFromSolved(result.SuccessorState));
        }
    }
}