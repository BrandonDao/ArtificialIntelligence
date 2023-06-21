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
            board[position.X][position.Y] = Cell.Empty;
            int boardSize = board.GetLength(0);

            //do
            //{

            //    do
            //    {
            //        do
            //        {
            //            position = new Point(Random.Shared.Next(0, boardSize), Random.Shared.Next(0, boardSize));
            //        } while (board[position.X][position.Y] != Cell.Wall);

            //        position.X += Random.Shared.Next(0, 2) == 1 ? -1 : 1;
            //        position.Y += Random.Shared.Next(0, 2) == 1 ? -1 : 1;

            //    } while (!(position.X >= 0 && position.X < board.Length && position.Y >= 0 && position.Y < board[0].Length));
            //} while (board[position.X][position.Y] != Cell.Empty);


            do
            {
                position = new Point(Random.Shared.Next(0, boardSize), Random.Shared.Next(0, boardSize));

            } while (board[position.X][position.Y] != Cell.Empty);

            board[position.X][position.Y] = Cell.Food;

            hitbox = new Rectangle(drawOffset.X + position.X * cellSize, drawOffset.Y + position.Y * cellSize, cellSize, cellSize);
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
            => spriteBatch.Draw(texture, hitbox, color);
    }
}