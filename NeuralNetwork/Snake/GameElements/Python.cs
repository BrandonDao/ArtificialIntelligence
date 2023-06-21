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

        private const int MaxMovesSinceLastEating = 75;
        private int movesSinceLastEating;

        public LinkedList<Point> Positions { get; private set; }
        public Point HeadPosition => Positions.First.Value;

        private readonly double[] inputs;

        private const int outputCount = 2;
        private double[] outputs;
        //private readonly Directions[] correlatedOutputs = new Directions[outputCount]
        //{
        //    Directions.Up,
        //    Directions.Down,
        //    Directions.Left,
        //    Directions.Right
        //};

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

            int inputCount = 6; // food distance, wall distance in 4 directions
            //int inputCount = 4 + (board.Length * board.Length); // head position + food position + every position on the board

            Network = new NeuralNetwork(ActivationFunction.TanH, ErrorFunction.MeanSquaredError,
                neuronsPerLayer: new int[] { inputCount, 3, outputCount });

            inputs = new double[inputCount];
            outputs = new double[outputCount];
        }

        public void Reset(double[][] board)
        {
            Score = 0;
            movesSinceLastEating = 0;
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

            //Score++;
            movesSinceLastEating++;

            inputs[0] = -(HeadPosition.X - food.Position.X) / (double)boardSize;
            inputs[1] = -(HeadPosition.Y - food.Position.Y) / (double)boardSize;

            int topWallY = HeadPosition.Y - 1;
            for (; topWallY >= 0 && board[HeadPosition.X][topWallY] != Cell.Wall; topWallY--) { }
            inputs[2] = (HeadPosition.Y - topWallY) / (double)boardSize;

            int bottomWallY = HeadPosition.Y + 1;
            for (; bottomWallY < boardSize && board[HeadPosition.X][bottomWallY] != Cell.Wall; bottomWallY++) { }
            inputs[3] = (bottomWallY - HeadPosition.Y) / (double)boardSize;

            int leftWallX = HeadPosition.X - 1;
            for (; leftWallX >= 0 && board[leftWallX][HeadPosition.Y] != Cell.Wall; leftWallX--) { }
            inputs[4] = (HeadPosition.X - leftWallX) / (double)boardSize;

            int rightWallX = HeadPosition.X + 1;
            for (; rightWallX < boardSize && board[rightWallX][HeadPosition.Y] != Cell.Wall; rightWallX++) { }
            inputs[5] = (rightWallX - HeadPosition.X) / (double)boardSize;

            outputs = Network.Compute(inputs);

            if (Math.Abs(outputs[0]) > Math.Abs(outputs[1]))
            {
                if (outputs[0] < 0)
                {
                    Direction = Directions.Left;
                }
                else
                {
                    Direction = Directions.Right;
                }
            }
            else
            {
                if (outputs[1] > 0)
                {
                    Direction = Directions.Down;
                }
                else
                {
                    Direction = Directions.Up;
                }
            }

            // double max = outputs[0];
            //Direction = correlatedOutputs[0];

            //for (int i = 1; i < outputs.Length; i++)
            //{
            //    if (outputs[i] > max)
            //    {
            //        max = outputs[i];
            //        Direction = correlatedOutputs[i];
            //    }
            //}

            updateTimer = TimeSpan.Zero;

            Point newHeadPos = HeadPosition;
            board[newHeadPos.X][newHeadPos.Y] = Cell.Wall;

            switch (Direction)
            {
                case Directions.Up: newHeadPos.Y--; break;
                case Directions.Down: newHeadPos.Y++; break;
                case Directions.Left: newHeadPos.X--; break;
                case Directions.Right: newHeadPos.X++; break;
            }

            Score -= HeadPosition.DistanceFrom(food.Position) + 4;

            //if ((food.Position.X < HeadPosition.X && Direction == Directions.Left)
            //    || (food.Position.X > HeadPosition.X && Direction == Directions.Right)
            //    || (food.Position.Y < HeadPosition.Y && Direction == Directions.Up)
            //    || (food.Position.Y > HeadPosition.Y && Direction == Directions.Down))
            //{
            //    Score += 1;
            //}
            //else if (Positions.Count < 10
            //    && (food.Position.X < HeadPosition.X && Direction == Directions.Right
            //    || (food.Position.X > HeadPosition.X && Direction == Directions.Left)
            //    || (food.Position.Y < HeadPosition.Y && Direction == Directions.Down)
            //    || (food.Position.Y > HeadPosition.Y && Direction == Directions.Up)))
            //{
            //    Score -= 2;
            //}

            if (movesSinceLastEating > MaxMovesSinceLastEating)
            {
                IsDead = true;
                return;
            }

            //if(newHeadPos.X < 0)
            //{
            //    newHeadPos.X = boardSize - 1;
            //}
            //else if(newHeadPos.X >= boardSize)
            //{
            //    newHeadPos.X = 0;
            //}
            //else if(newHeadPos.Y < 0)
            //{
            //    newHeadPos.Y = boardSize - 1; ;
            //}
            //else if(newHeadPos.Y >= boardSize)
            //{
            //    newHeadPos.Y = 0;
            //}


            if (newHeadPos.X < 0 || newHeadPos.X >= boardSize || newHeadPos.Y < 0 || newHeadPos.Y >= boardSize
            || board[newHeadPos.X][newHeadPos.Y] == Cell.Wall)
            {
                if (newHeadPos == Positions.First.Next.Value && Score > 0)
                {
                    Score /= 10;
                }

                IsDead = true;
                return;
            }

            if (newHeadPos == food.Position)
            {
                food.Respawn(board, cellSize, Color);
                movesSinceLastEating = 0;
                Score += 1000;
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