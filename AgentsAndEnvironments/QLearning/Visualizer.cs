using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QLearning.AgentSide;
using QLearning.EnvironmentSide;
using SharedLibrary;
using SharedLibrary.States;
using System.Runtime.CompilerServices;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace QLearning
{
    internal class Visualizer : MonoGame.Forms.NET.Controls.MonoGameControl
    {
        private MouseEnvironment environment;
        private MouseAgent mouse;

        private Texture2D blankTexture;

        private (Rectangle, Color)[,] drawMap;

        private int tileSize;

        private TimeSpan updateStopwatch;
        private TimeSpan updateDuration;

        protected override void Initialize()
        {
            environment = new();
            mouse = new(environment);
            
            blankTexture = new Texture2D(Editor.GraphicsDevice, width: 1, height: 1);
            blankTexture.SetData([Color.White]);

            tileSize = Math.Max(Editor.GraphicsDevice.Viewport.Width, Editor.GraphicsDevice.Viewport.Height) / MouseEnvironment.Width;

            updateDuration = TimeSpan.FromMilliseconds(100);

            drawMap = new (Rectangle, Color)[MouseEnvironment.Width, MouseEnvironment.Height];
            for (int x = 0; x < MouseEnvironment.Width; x++)
            {
                for (int y = 0; y < MouseEnvironment.Height; y++)
                {
                    drawMap[x,y] = new (
                        new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize),
                        new Point(x, y) == environment.WallPosition ? Color.Black :
                            (new Point(x, y) == environment.CheesePosition ? Color.Gold : 
                                (new Point(x,y) == environment.FirePosition ? Color.Red : Color.LightGray)));
                }
            }

            var startingState = new MouseState(new Point(0, 2), MouseState.Types.Unknown);

            environment.RegisterAgent(mouse, startingState, new StateToken<IState>(startingState));

            //while (!mouse.MakeMove((a) => false)) ;
        }

        protected override void Update(GameTime gameTime)
        {
            updateStopwatch += gameTime.ElapsedGameTime;

            if (updateStopwatch < updateDuration) return;

            updateStopwatch = TimeSpan.Zero;

            mouse.MakeMove((a) => false);
        }

        protected override void Draw()
        {
            Editor.spriteBatch.Begin();

            for (int x = 0; x < MouseEnvironment.Width; x++)
            {
                for (int y = 0; y < MouseEnvironment.Height; y++)
                {
                    Editor.spriteBatch.Draw(blankTexture, drawMap[x, y].Item1, drawMap[x, y].Item2);
                }
            }

            ((MouseState)environment.AgentToStateToken[mouse].State).Position.Deconstruct(out int mouseX, out int mouseY);

            Editor.spriteBatch.Draw(
                blankTexture,
                new Rectangle((mouseX * tileSize) + (tileSize >> 2), (mouseY * tileSize) + (tileSize >> 2), tileSize >> 1, tileSize >> 1),
                Color.Blue);

            Editor.spriteBatch.End();
        }
    }
}
