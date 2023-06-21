using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using NeuralNetworkLibrary;
using NeuralNetworkLibrary.Perceptrons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LineOfBestFitVisualizer
{
    public class Game1 : Game
    {
        public class Line
        {
            public static Line None { get; } = new(Color.Black, 0, 0, 0);

            public Color Color;

            private readonly double slope;
            private readonly int yIntercept;
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

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<Point> points;

        private Line calculatedLine;
        private Line approximatedLine;

        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;

        private int domainMax;
        double Normalize(double x) => x / domainMax;

        private GradientDescentPerceptron perceptron;
        readonly ErrorFunction errorFunc = new(ErrorFunction.MeanSquaredError, ErrorFunction.MeanSquaredErrorDerivative);
        readonly ActivationFunction actFunc = new(ActivationFunction.Identity, ActivationFunction.IdentityDerivative);
        const double defaultLearningRate = 0.1d;
        const int trainingIterations = 1;
        const int fastTrainingIterations = 3;

        int drawCount, updateCount = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 1000
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        GradientDescentPerceptron GetNewPerceptron()
            => new(
                amountOfInputs: 1,
                learningRate: defaultLearningRate / (points.Count == 0 ? 1 : points.Count),
                actFunc, errorFunc);

        void ResetLines()
        {
            calculatedLine = Line.None;
            approximatedLine = Line.None;
        }

        protected override void Initialize()
        {
            domainMax = graphics.PreferredBackBufferWidth;

            TargetElapsedTime /= 2;

            points = new List<Point>();
            ResetLines();

            var datapoints =
                File.ReadAllLines(@"C:\Users\brand\Documents\Github\NeuralNetworks\NeuralNetwork\Snake\AverageFitnesses.txt")
                    .Select((string value) => int.Parse(value)).ToArray();

            int xMax = datapoints.Length;
            int yMin = datapoints.Min();
            int yMax = datapoints.Max();

            for(int i = 0; i < datapoints.Length; i++)
            {
                points.Add(new Point(i * graphics.PreferredBackBufferWidth / xMax, graphics.PreferredBackBufferHeight - ((datapoints[i] - yMin) * graphics.PreferredBackBufferHeight / yMax)));
            }
            
            perceptron = GetNewPerceptron();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private void CalculateLineOfBestFit()
        {
            double xAvg = 0;
            double yAvg = 0;
            double numerator = 0;
            double denominator = 0;

            foreach (Point p in points)
            {
                xAvg += p.X;
                yAvg += p.Y;
            }
            xAvg /= points.Count;
            yAvg /= points.Count;

            foreach (Point p in points)
            {
                numerator += (p.X - xAvg) * (p.Y - yAvg);
                denominator += Math.Pow(p.X - xAvg, 2);
            }
            double slope = numerator / denominator;
            int yIntercept = (int)(yAvg - xAvg * slope);

            calculatedLine = new Line(Color.Red, slope, yIntercept, graphics.PreferredBackBufferWidth);
        }
        private void ApproximateLineOfBestFit(bool willTrainMore)
        {
            var inputs = new double[points.Count][];
            var outputs = new double[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                inputs[i] = new double[] { Normalize(points[i].X) };
                outputs[i] = Normalize(points[i].Y);
            }

            perceptron.TrainFor(inputs, outputs, willTrainMore ? fastTrainingIterations : trainingIterations);

            double x1 = 0;
            double x2 = domainMax;
            double y1 = perceptron.Compute(new double[] { x1 });
            double y2 = perceptron.Compute(new double[] { x2 });

            approximatedLine = new Line(Color.Green, (y1 - y2) / (x1 - x2), yIntercept: (int)(y1 * domainMax), domainMax);
        }

        private void AddOrRemovePoint(MouseState mouseState)
        {
            int size = 20;
            var mouseHitbox = new Rectangle(mouseState.Position.X - size / 2, mouseState.Position.Y - size / 2, size, size);
            bool hasRemoved = false;

            foreach (var point in points)
            {
                if (!mouseHitbox.Contains(point)) continue;

                points.Remove(point);
                hasRemoved = true;
                ResetLines();

                break;
            }

            if (!hasRemoved)
            {
                points.Add(mouseState.Position);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //updateCount++;
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released
                && IsActive && graphics.GraphicsDevice.Viewport.Bounds.Contains(mouseState.Position))
            {

                perceptron = GetNewPerceptron();

                AddOrRemovePoint(mouseState);

                if (points.Count > 1)
                {
                    CalculateLineOfBestFit();
                }
            }

            if (points.Count > 1)
            {
                ApproximateLineOfBestFit(willTrainMore: keyboardState.IsKeyDown(Keys.Space));
            }

            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //drawCount++;
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            foreach (var plot in points)
            {
                spriteBatch.DrawCircle(plot.ToVector2(), 5, 10, Color.White, 5);
            }

            calculatedLine.Draw(spriteBatch);
            approximatedLine.Draw(spriteBatch);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}