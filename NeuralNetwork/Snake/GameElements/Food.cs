using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Snake.GameElements
{
    public class Food
    {
        public static Random Random { get; set; }

        public Point Position => position;

        private readonly Texture2D texture;
        
        private Point position;
        private Rectangle hitbox;
        private Color color;

        public Food(Texture2D texture, int boardSize, int cellSize, Point headPosition, List<Point> bodyPositions)
        {
            this.texture = texture;

            Respawn(boardSize, cellSize, headPosition, bodyPositions);
        }

        public void Respawn(int boardSize, int cellSize, Point headPosition, List<Point> bodyPositions)
        {
            do
            {
                position = new Point(Random.Next(0, boardSize), Random.Next(0, boardSize));

            } while (bodyPositions.Contains(position) || position == headPosition);

            hitbox = new Rectangle(position.X * cellSize, position.Y * cellSize, cellSize, cellSize);
            color = new Color((uint)Random.Shared.Next())
            {
                A = 255
            };
        }

        public void Draw(SpriteBatch spriteBatch)
            => spriteBatch.Draw(texture, hitbox, color);
    }
}