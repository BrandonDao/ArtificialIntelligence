using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlappyBird.GameElements
{
    public class Pipe
    {
        public static int GapWidth { get; set; }
        public static int GapHeight { get; set; }
        public static Texture2D Texture { get; set; }
        public Point GapPosition => centerGapPos;

        public Rectangle TopHithox;
        public Rectangle BottomHitbox;

        private static Random random;
        private static int screenHeight;

        Point centerGapPos;
        int velocity;

        public Pipe(int xPosition, int velocity)
        {
            Reset(xPosition);

            this.velocity = velocity;
        }

        public static void InitializeSharedData(Texture2D texture, int gapHeight, int gapWidth, int HeightOfScreen)
        {
            Texture = texture;

            random = new Random();

            screenHeight = HeightOfScreen;

            GapWidth = gapWidth;
            GapHeight = gapHeight;
        }

        public void Reset(int xPosition)
        {
            centerGapPos.X = xPosition;
            centerGapPos.Y = random.Next(GapHeight, screenHeight - GapHeight);

            int halfWidth = GapWidth / 2;
            int halfHeight = GapHeight / 2;

            TopHithox = new Rectangle(
                x: centerGapPos.X - halfWidth,
                y: centerGapPos.Y - halfHeight - Texture.Height,
                width: GapWidth,
                height: Texture.Height);

            BottomHitbox = new Rectangle(
                x: centerGapPos.X - halfWidth,
                y: centerGapPos.Y + halfHeight,
                width: GapWidth,
                height: Texture.Height);
        }
        public void Update()
        {
            centerGapPos.X -= velocity;
            TopHithox.X -= velocity;
            BottomHitbox.X -= velocity;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, BottomHitbox, Color.White);

            spritebatch.Draw(Texture,
                destinationRectangle: TopHithox,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.FlipVertically,
                layerDepth: 0
                );
        }
    }
}