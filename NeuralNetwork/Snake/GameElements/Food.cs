using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Snake.GameElements
{
    public class Food
    {
        public static Random Random { get; set; }

        public Point Position => position;

        private readonly Texture2D texture;
        
        private Point position;
        private Rectangle hitbox;

        public Food(Texture2D texture, int boardSize, int cellSize, List<Point> snakePositions)
        {
            this.texture = texture;

            Respawn(boardSize, cellSize, snakePositions);
        }

        public void Respawn(int boardSize, int cellSize, List<Point> snakePositions)
        {
            do
            {
                position = new Point(Random.Next(0, boardSize), Random.Next(0, boardSize));

            } while (snakePositions.Contains(position));

            hitbox = new Rectangle(position.X * cellSize, position.Y * cellSize, cellSize, cellSize);
        }

        public void Draw(SpriteBatch spriteBatch)
            => spriteBatch.Draw(texture, hitbox, Color.White);
    }
}