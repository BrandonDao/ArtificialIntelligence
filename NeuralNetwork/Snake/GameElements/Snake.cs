using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Snake.GameElements
{
    public class Snake
    {
        public enum Directions
        {
            Up,
            Down,
            Left,
            Right
        }

        public bool IsDead;
        public Directions TargetDirection;
        public Directions ActualDirection;

        private readonly int cellSize;
        private readonly int boardSize;
        private readonly Texture2D texture;

        private readonly TimeSpan timePerFrame;
        private TimeSpan updateTimer;

        private Point headPosition;
        private List<Point> bodyPositions;

        public Snake(Texture2D texture, int cellSize, int boardSize, int movementsPerSecond)
        {
            this.texture = texture;
            this.cellSize = cellSize;
            this.boardSize = boardSize;
            timePerFrame = TimeSpan.FromMilliseconds(1000f / movementsPerSecond);
            updateTimer = TimeSpan.Zero;

            bodyPositions = new List<Point>();
            Reset();

            TargetDirection = Directions.Right;
            ActualDirection = Directions.Right;
        }

        public void Reset()
        {
            IsDead = false;
            headPosition = new Point(boardSize / 2);

            bodyPositions.Clear();
            bodyPositions.Add(new Point(headPosition.X - 1, headPosition.Y));
            bodyPositions.Add(new Point(headPosition.X - 2, headPosition.Y));
            bodyPositions.Add(new Point(headPosition.X - 3, headPosition.Y));
        }

        public void Update(TimeSpan elapsedGameTime, KeyboardState keyboardState, Food food)
        {
            updateTimer += elapsedGameTime;

            if (keyboardState.IsKeyDown(Keys.W) && ActualDirection != Directions.Down)
            {
                TargetDirection = Directions.Up;
            }
            else if (keyboardState.IsKeyDown(Keys.S) && ActualDirection != Directions.Up)
            {
                TargetDirection = Directions.Down;
            }

            if (keyboardState.IsKeyDown(Keys.A) && ActualDirection != Directions.Right)
            {
                TargetDirection = Directions.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D) && ActualDirection != Directions.Left)
            {
                TargetDirection = Directions.Right;
            }

            if (updateTimer < timePerFrame)
            {
                return;
            }

            ActualDirection = TargetDirection;
            updateTimer = TimeSpan.Zero;

            Point previous = headPosition;

            switch (ActualDirection)
            {
                case Directions.Up: headPosition.Y--; break;
                case Directions.Down: headPosition.Y++; break;
                case Directions.Left: headPosition.X--; break;
                case Directions.Right: headPosition.X++; break;
            }

            if (headPosition.X < 0 || headPosition.X >= boardSize || headPosition.Y < 0 || headPosition.Y >= boardSize)
            {
                IsDead = true;
            }

            Point temp;
            for (int i = 0; i < bodyPositions.Count; i++)
            {
                temp = bodyPositions[i];
                bodyPositions[i] = previous;
                previous = temp;

                if (headPosition == bodyPositions[i])
                {
                    IsDead = true;
                }
            }

            if (headPosition == food.Position)
            {
                bodyPositions.Add(headPosition);
                headPosition = food.Position;

                food.Respawn(boardSize, cellSize, headPosition, bodyPositions);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var headRectangle = new Rectangle(headPosition.X * cellSize, headPosition.Y * cellSize, cellSize, cellSize);
            spriteBatch.Draw(texture, headRectangle, Color.White);

            foreach (Point position in bodyPositions)
            {
                var bodyRectangle = new Rectangle(position.X * cellSize, position.Y * cellSize, cellSize, cellSize);
                spriteBatch.Draw(texture, bodyRectangle, Color.White);
            }
        }
    }
}