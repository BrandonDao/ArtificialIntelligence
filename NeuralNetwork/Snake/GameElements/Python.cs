using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeuralNetworkLibrary;
using NeuralNetworkLibrary.NetworkStructure;
using Snake.GameElements;
using System;
using System.Collections.Generic;

namespace Snake.NetworkElements
{
    public class Python
    {
        public enum Directions
        {
            Up = 1,
            Down = -1,
            Left = 2,
            Right = -2
        }

        public bool IsDead { get; set; }
        public Directions Direction { get; set; }
        public NeuralNetwork Network { get; set; }
        public int Score { get; private set; }

        private const int startingBodySize = 4;

        private readonly Point drawOffset;
        private readonly int cellSize;
        private readonly int boardSize;
        private readonly Texture2D texture;
        public Color Color { get; private set; }

        private readonly TimeSpan timePerFrame;
        private TimeSpan updateTimer;

        public LinkedList<Point> Positions { get; private set; }

        private readonly double[] inputs;

        private const int outputCount = 4;
        private double[] outputs;
        private readonly Directions[] correlatedOutputs = new Directions[outputCount]
        {
            Directions.Up,
            Directions.Down,
            Directions.Left,
            Directions.Right
        };

        public Python() { }
        public Python(double[][] board, int cellSize, Point drawOffset, Texture2D texture, int movementsPerSecond)
        {
            this.texture = texture;
            this.cellSize = cellSize;
            this.drawOffset = drawOffset;
            boardSize = board.GetLength(0);

            timePerFrame = TimeSpan.FromMilliseconds(1000f / movementsPerSecond);
            updateTimer = TimeSpan.Zero;

            Positions = new LinkedList<Point>();
            Reset(board);

            Direction = Directions.Right;

            int inputCount = 4 + (board.Length * board.Length); // head position + food position + every position on the board

            Network = new NeuralNetwork(ActivationFunction.TanH, ErrorFunction.MeanSquaredError, neuronsPerLayer: new int[] { inputCount, 80, outputCount });
            Score = 0;

            inputs = new double[inputCount];
            outputs = new double[outputCount];
        }

        public void Reset(double[][] board)
        {
            IsDead = false;

            foreach (var pos in Positions)
            {
                board[pos.X][pos.Y] = Cell.Empty;
            }

            Positions.Clear();
            Point headPosition = new(boardSize / 2);

            for (int i = 0; i < startingBodySize; i++)
            {
                Positions.AddLast(new Point(headPosition.X - i, headPosition.Y));
                board[Positions.Last.Value.X][Positions.Last.Value.Y] = Cell.Wall;
            }

            Color = new Color((uint)Random.Shared.Next()) { A = 255 };
        }


        public void Update(double[][] board, TimeSpan elapsedGameTime, Food food)
        {
            if (IsDead) return;

            updateTimer += elapsedGameTime;

            if (updateTimer < timePerFrame) return;

            Score++;

            inputs[0] = Positions.First.Value.X / (double)boardSize;
            inputs[1] = Positions.First.Value.Y / (double)boardSize;
            inputs[3] = food.Position.X / (double)boardSize;
            inputs[4] = food.Position.Y / (double)boardSize;

            for (int x = 0; x < board.Length; x++)
            {
                for (int y = 0; y < board.Length; y++)
                {
                    inputs[5 + x + y] = board[x][y];
                }
            }

            outputs = Network.Compute(inputs);

            double max = outputs[0];
            Direction = correlatedOutputs[0];

            for (int i = 1; i < outputs.Length; i++)
            {
                if (outputs[i] > max)
                {
                    max = outputs[i];
                    Direction = correlatedOutputs[i];
                }
            }

            updateTimer = TimeSpan.Zero;

            Point newHeadPos = Positions.First.Value;
            board[newHeadPos.X][newHeadPos.Y] = Cell.Wall;

            switch (Direction)
            {
                case Directions.Up:     newHeadPos.Y--; break;
                case Directions.Down:   newHeadPos.Y++; break;
                case Directions.Left:   newHeadPos.X--; break;
                case Directions.Right:  newHeadPos.X++; break;
            }


            if (newHeadPos.X < 0 || newHeadPos.X >= boardSize || newHeadPos.Y < 0 || newHeadPos.Y >= boardSize
                || board[newHeadPos.X][newHeadPos.Y] == Cell.Wall)
            {
                IsDead = true;
                return;
            }

            if (board[newHeadPos.X][newHeadPos.Y] == Cell.Food)
            {
                food.Respawn(board, cellSize, Color);
                //Score += ;
            }
            else
            {
                Positions.RemoveLast();
                board[Positions.Last.Value.X][Positions.Last.Value.Y] = Cell.Empty;
            }

            Positions.AddFirst(newHeadPos);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Point position in Positions)
            {
                var bodyRectangle = new Rectangle(drawOffset.X + position.X * cellSize, drawOffset.Y + position.Y * cellSize, cellSize, cellSize);
                spriteBatch.Draw(texture, bodyRectangle, Color);
            }
        }
    }
}