using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Snake.GameElements
{
    public class GameBoard
    {
        public int Size { get; private set; }
        public int CellSize { get; private set; }

        private Texture2D texture;
        private Rectangle drawDestination;

        private Snake Snake;
        private Food Food;

        public GameBoard(int boardSize, int cellSize, Point drawOffset, Texture2D boardTexture, Texture2D foodTexture)
        {
            Size = boardSize;
            CellSize = cellSize;

            texture = boardTexture;
            drawDestination = new Rectangle(drawOffset.X * cellSize * boardSize, drawOffset.Y * cellSize * boardSize, Size * CellSize, Size * CellSize);

            Food = new Food(foodTexture, boardSize, cellSize, new List<Point>());
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, drawDestination, Color.White);
            
            Food.Draw(spriteBatch);
        }
    }
}