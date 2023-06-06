using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.NetworkElements;
using System;

namespace Snake.GameElements
{
    public class Food
    {
        public Point Position => position;

        private readonly Texture2D texture;
        private readonly Point drawOffset;

        private Point position;
        private Rectangle hitbox;
        private Color color;

        public Food(Texture2D texture, double[][] board, int cellSize, Point drawOffset, Color color)
        {
            this.texture = texture;
            this.drawOffset = drawOffset;

            Respawn(board, cellSize, color);
        }

        public void Respawn(double[][] board, int cellSize, Color color)
        {
            int boardSize = board.GetLength(0);

            do
            {
                position = new Point(Random.Shared.Next(0, boardSize), Random.Shared.Next(0, boardSize));

            } while (board[position.X][position.Y] != Cell.Empty);

            hitbox = new Rectangle(drawOffset.X + position.X * cellSize, drawOffset.Y + position.Y * cellSize, cellSize, cellSize);
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
            => spriteBatch.Draw(texture, hitbox, color);
    }
}