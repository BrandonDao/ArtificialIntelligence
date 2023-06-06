using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.GameElements;
using System;
using System.Diagnostics;

namespace Snake.NetworkElements
{
    // best enum
    public static class Cell
    {
        public const double Wall = 0d;
        public const double Empty = 1d;
        public const double Food = 0.5d;
    }

    [DebuggerDisplay("Score: {Python.Score} IsDead: {IsSnakeDead}")]
    public class Habitat
    {
        public int Size { get; private set; }
        public int CellSize { get; private set; }
        public double[][] Board { get; private set; }

        public Point DrawOffset { get; set; }

        public bool IsSnakeDead => Python.IsDead;
        public Python Python { get; private set; }
        public Food Food { get; private set; }

        private Rectangle drawDestination;
        private Texture2D texture;

        public Habitat(int boardSize, int cellSize, Point drawOffset, Texture2D boardTexture, Texture2D foodTexture, Texture2D snakeTexture, int movementsPerSecond)
        {
            Size = boardSize;
            CellSize = cellSize;

            DrawOffset = drawOffset;

            Board = new double[boardSize][];
            for (int x = 0; x < boardSize; x++)
            {
                Board[x] = new double[boardSize];

                for (int y = 0; y < boardSize; y++)
                {
                    Board[x][y] = Cell.Empty;
                }
            }

            texture = boardTexture;
            drawDestination = new Rectangle(drawOffset.X, drawOffset.Y, Size * CellSize, Size * CellSize);

            Python = new Python(Board, CellSize, DrawOffset, snakeTexture, movementsPerSecond);
            Food = new Food(foodTexture, Board, cellSize, DrawOffset, Python.Color);
        }

        public void Reset()
        {
            Python.Reset(Board);
            Food.Respawn(Board, CellSize, Python.Color);
        }

        public void Update(TimeSpan elapsedGameTime)
            => Python.Update(Board, elapsedGameTime, Food);

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Python.IsDead) return;

            spriteBatch.Draw(texture, drawDestination, Color.Black);

            Python.Draw(spriteBatch);
            Food.Draw(spriteBatch);
        }
    }
}