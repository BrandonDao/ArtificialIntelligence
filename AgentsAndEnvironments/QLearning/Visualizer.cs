using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QLearning.AgentSide;
using QLearning.EnvironmentSide;
using SharedLibrary;
using SharedLibrary.States;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace QLearning
{
    internal class Visualizer : MonoGame.Forms.NET.Controls.MonoGameControl
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MouseAgent Mouse { get; private set; }

        public TimeSpan UpdateDuration { get; set; }
        private TimeSpan updateStopwatch;

        private Texture2D blankTexture;

        private MouseEnvironment environment;
        private (Rectangle destRect, MouseState state, Color color)[,] drawMap;

        private int tileSize;
        private const int gapSize = 2;

        public bool IsShowingQValues { get; set; } = false;
        public bool IsRunning = false;
        public bool SkipRendering = false;
        public int SkipRenderAmount = 0;

#pragma warning restore CS8618
        protected override void Initialize()
        {
            Editor.RemoveDefaultComponents();

            var startingState = new MouseState(new Point(0, 0), MouseState.Types.Unknown);
            environment = new();
            Mouse = new(environment, startingState);

            blankTexture = new Texture2D(Editor.GraphicsDevice, width: 1, height: 1);
            blankTexture.SetData([Color.White]);

            tileSize = ((Math.Min(Editor.GraphicsDevice.Viewport.Width, Editor.GraphicsDevice.Viewport.Height) - gapSize) / Math.Max(MouseEnvironment.Width, MouseEnvironment.Height)) - gapSize;

            UpdateDuration = TimeSpan.FromMilliseconds(300);

            drawMap = new (Rectangle, MouseState, Color)[MouseEnvironment.Width, MouseEnvironment.Height];

            for (int x = 0; x < MouseEnvironment.Width; x++)
            {
                for (int y = 0; y < MouseEnvironment.Height; y++)
                {
                    Point position = new(x, y);
                    MouseState state = new(position,
                        position == environment.WallPosition ? MouseState.Types.Unknown :
                            (position == environment.CheesePosition ? MouseState.Types.Cheese :
                                (position == environment.FirePosition ? MouseState.Types.Fire : MouseState.Types.Empty)));

                    drawMap[x, y] = new(
                        new Rectangle(x * tileSize + (x + 1) * gapSize, y * tileSize + (y + 1) * gapSize, tileSize, tileSize),
                        state,
                        state.Type switch
                        {
                            MouseState.Types.Unknown => Color.Black,
                            MouseState.Types.Empty => Color.LightGray,
                            MouseState.Types.Fire => Color.Red,
                            MouseState.Types.Cheese => Color.Gold,
                            _ => throw new NotImplementedException()
                        });
                }
            }


            environment.RegisterAgent(Mouse, startingState, new StateToken<IState>(startingState));
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsRunning) return;

            for (int i = 0; i < (SkipRendering ? SkipRenderAmount : 1); i++)
            {
                updateStopwatch += gameTime.ElapsedGameTime;

                if (updateStopwatch < UpdateDuration) return;

                updateStopwatch = TimeSpan.Zero;

                Mouse.MakeMove((a) => false);
            }
        }

        protected override void Draw()
        {
            Editor.GraphicsDevice.Clear(Color.Black);
            Editor.spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            ((MouseState)environment.AgentToStateToken[Mouse].State).Position.Deconstruct(out int mouseX, out int mouseY);

            for (int x = 0; x < MouseEnvironment.Width; x++)
            {
                for (int y = 0; y < MouseEnvironment.Height; y++)
                {
                    Editor.spriteBatch.Draw(blankTexture, drawMap[x, y].destRect, drawMap[x, y].color);

                    if (drawMap[x, y].state.Type is MouseState.Types.Cheese or MouseState.Types.Fire)
                    {
                        Editor.spriteBatch.Draw(blankTexture, drawMap[x, y].destRect, drawMap[x, y].color);

                        if (x == mouseX && y == mouseY)
                        {
                            Editor.spriteBatch.Draw(
                                blankTexture,
                                new Rectangle((mouseX * tileSize) + (tileSize >> 2), (mouseY * tileSize) + (tileSize >> 2), tileSize >> 1, tileSize >> 1),
                                Color.Blue);
                        }
                        continue;
                    }

                    if (x == mouseX && y == mouseY)
                    {
                        Editor.spriteBatch.Draw(
                            blankTexture,
                            new Rectangle((mouseX * tileSize) + (tileSize >> 2), (mouseY * tileSize) + (tileSize >> 2), tileSize >> 1, tileSize >> 1),
                            Color.Blue);
                    }

                    //if (x is < 0 or >= MouseEnvironment.Width || y is < 0 or >= MouseEnvironment.Height) continue;

                    //if (!Mouse.stateToAllMovements.TryGetValue(drawMap[x, y].state, out HashSet<MouseAgentMovement>? movements)) continue;

                    //foreach(var movement in movements)
                    //{
                    //    string text = Math.Round(Mouse.QMap[movement], 0).ToString();

                    //    Vector2 textPosition = movement.Direction switch
                    //    {
                    //        MouseAgentMovement.Directions.Up => drawMap[x,y].destRect.Center.ToVector2() + new Vector2(-Editor.Font.MeasureString(text).X / 2, -tileSize / 3),
                    //        MouseAgentMovement.Directions.Down => drawMap[x, y].destRect.Center.ToVector2() + new Vector2(-Editor.Font.MeasureString(text).X / 2 , tileSize / 3),
                    //        MouseAgentMovement.Directions.Left => drawMap[x, y].destRect.Center.ToVector2() + new Vector2(-tileSize / 3, 0),
                    //        MouseAgentMovement.Directions.Right => drawMap[x, y].destRect.Center.ToVector2() + new Vector2(tileSize / 3, 0),
                    //        _ => throw new NotImplementedException(),
                    //    };

                    //    Editor.spriteBatch.DrawString(Editor.Font, text, textPosition, Color.Black);
                    //}

                    //float alpha = ( + 1000) / 2000;
                    //Editor.spriteBatch.Draw(blankTexture, drawMap[pos.X, pos.Y].destRect, Color.Lerp(Color.Black, Color.White, alpha));
                }
            }

            Editor.spriteBatch.End();
        }
    }
}
