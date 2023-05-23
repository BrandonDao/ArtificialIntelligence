using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Snake.GameElements
{
    public class GameBoard
    {
        public int Size { get; private set; }
        public int CellSize { get; private set; }
        public bool IsSnakeDead => snake.IsDead;

        private readonly Texture2D texture;
        private readonly Rectangle drawDestination;

        private Snake snake;
        private Food food;

        public GameBoard(int boardSize, int cellSize, Texture2D boardTexture, Texture2D foodTexture, Texture2D snakeHeadTexture)
        {
            Size = boardSize;
            CellSize = cellSize;

            texture = boardTexture;
            drawDestination = new Rectangle(0, 0, Size * CellSize, Size * CellSize);

            food = new Food(foodTexture, boardSize, cellSize, new Point(-1), new List<Point>());
            snake = new Snake(snakeHeadTexture, cellSize, Size, movementsPerSecond: 10);
        }

        public void Reset() => snake.Reset();
        public void Update(TimeSpan elapsedGameTime, KeyboardState keyboardState)
        {
            if(!snake.IsDead) snake.Update(elapsedGameTime, keyboardState, food);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, drawDestination, Color.White);

            snake.Draw(spriteBatch);
            food.Draw(spriteBatch);
        }
    }
}