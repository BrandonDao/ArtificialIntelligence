using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Perceptron;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineOfBestFitVisualizer
{
    public class Line
    {
        public Color Color;

        private double slope;
        private int yIntercept;
        private Vector2 p1;
        private Vector2 p2;

        public Line(Color color, double slope, int yIntercept, int domainMax)
        {
            this.slope = slope;
            this.yIntercept = yIntercept;
            Color = color;

            p1 = new Vector2(0, FindY(0));
            p2 = new Vector2(domainMax, FindY(domainMax));
        }

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.DrawLine(p1, p2, Color, 4);
        public int FindY(int x) => (int)((x * slope) + yIntercept);
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Random random;
        private HillClimbingPerceptron perceptron;

        private int domainMax;

        private Point[] plots;
        private Point[] normalizedPlots;
        private double xMin;
        private double xNormalizedMin;
        private double xMax;
        private double xNormalizedMax;
        private double yMin;
        private double yNormalizedMin;
        private double yMax;
        private double yNormalizedMax;

        private Line calculatedLine;
        private Line approximatedLine;

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
            random = new Random(10);
            domainMax = graphics.PreferredBackBufferWidth;

            perceptron = new HillClimbingPerceptron(random, amountOfInputs: 1, initialBias: 0, mutationAmount: 5d, ErrorFunc);

            plots = Array.Empty<Point>();
            normalizedPlots = Array.Empty<Point>();
            xMin = 0;
            xNormalizedMin = 0;
            xMax = 0;
            xNormalizedMax = 0;

            yMin = 0;
            yNormalizedMin = 0;
            yMax = 0;
            yNormalizedMax = 0;


            calculatedLine = new Line(Color.Black, 0, 0, domainMax);
            approximatedLine = new Line(Color.Black, 0, 0, domainMax);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private double Normalize(double x, double min, double max, double nMin, double nMax)
            => (x - min) / (max - min) * (nMax - nMin) + nMin;

        private void CalculateLineOfBestFit()
        {
            double xAvg = 0;
            double yAvg = 0;
            double numerator = 0;
            double denominator = 0;

            foreach (Point p in plots)
            {
                xAvg += p.X;
                yAvg += p.Y;
            }
            xAvg /= plots.Count;
            yAvg /= plots.Count;

            foreach (Point p in plots)
            {
                numerator += (p.X - xAvg) * (p.Y - yAvg);
                denominator += Math.Pow(p.X - xAvg, 2);
            }
            double slope = numerator / denominator;
            int yIntercept = (int)(yAvg - xAvg * slope);

            calculatedLine = new Line(Color.Red, slope, yIntercept, graphics.PreferredBackBufferWidth);
        }
        private double ErrorFunc(double actual, double expected) => Math.Pow(actual - expected, 2);
        private void ApproximateLineOfBestFit()
        {
            var inputs = new double[plots.Count][];
            var outputs = new double[plots.Count];

            for (int i = 0; i < plots.Count; i++)
            {
                inputs[i] = new double[1] { plots[i].X };
                outputs[i] = plots[i].Y;
            }

            double currentError = perceptron.GetError(inputs, outputs);

            perceptron.Train(inputs, outputs, currentError);

            double x1 = 0;
            double x2 = domainMax;
            double y1 = perceptron.Compute(new double[] { x1 });
            double y2 = perceptron.Compute(new double[] { x2 });

            approximatedLine = new Line(Color.Green, (y1 - y2) / (x1 - x2), (int)y1, domainMax);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (!IsActive || !graphics.GraphicsDevice.Viewport.Bounds.Contains(mouseState.Position)) return;

                var temp = new Point[plots.Length + 1];

                plots.CopyTo(temp, 0);
                temp[^1] = mouseState.Position;

                plots = new Point[temp.Length];
                temp.CopyTo(plots, 0);

                normalizedPlots = new Point[temp.Length];

                if (plots.Length > 1)
                {
                    CalculateLineOfBestFit();
                }
            }

            if (plots.Length > 1)
            {
                ApproximateLineOfBestFit();
            }

            if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
            {
                plots = Array.Empty<Point>();
                calculatedLine = new Line(Color.Black, 0, 0, graphics.PreferredBackBufferWidth);
                approximatedLine = new Line(Color.Black, 0, 0, graphics.PreferredBackBufferWidth);
            }

            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            calculatedLine.Draw(spriteBatch);
            approximatedLine.Draw(spriteBatch);

            foreach (var plot in plots)
            {
                spriteBatch.DrawCircle(plot.ToVector2(), 5, 10, Color.White, 5);
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}