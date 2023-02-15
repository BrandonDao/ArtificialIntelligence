using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineOfBestFitVisualizer
{
    public struct Line
    {
        private Vector2 p1;
        private Vector2 p2;

        public Line(double slope, int yIntercept, int screenWidth)
        {
            p1 = new Vector2(0, yIntercept);
            p2 = new Vector2(screenWidth, (int)(screenWidth * slope + yIntercept));
        }

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.DrawLine(p1, p2, Color.White, 4);
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<Point> plots;
        private Line calculatedLine;

        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            plots = new List<Point>();
            calculatedLine = new Line(-100, -100, graphics.PreferredBackBufferWidth);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private void CalculateLineOfBestFit()
        {
            double numerator = 0;
            double denominator = 0;

            double xAvg = plots.AsEnumerable().Average((Point p) => p.X);
            double yAvg = plots.AsEnumerable().Average((Point p) => p.Y);

            foreach (var point in plots)
            {
                numerator += (point.X - xAvg) * (point.Y - yAvg);
                denominator += Math.Pow(point.X - xAvg, 2);
            }

            double slope = numerator / denominator;
            int yIntercept = (int)(yAvg - xAvg * slope);
            
            calculatedLine = new Line(slope, yIntercept, graphics.PreferredBackBufferWidth);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                plots.Add(mouseState.Position);

                if (plots.Count > 1)
                {
                    CalculateLineOfBestFit();
                }
            }

            if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
            {
                plots.Clear();
                calculatedLine = new Line(-100, -100, graphics.PreferredBackBufferWidth);
            }

            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            foreach (var plot in plots)
            {
                spriteBatch.DrawCircle(plot.ToVector2(), 5, 10, Color.White, 5);
            }

            calculatedLine.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}