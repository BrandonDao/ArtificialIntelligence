using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird.Content
{
    public class ParallaxBackground
    {
        Texture2D texture;
        int[] xPositions;
        int xPositionOffset;
        int velocity;

        public ParallaxBackground(Texture2D backgroundTexture, int screenWidth, int velocity)
        {
            texture = backgroundTexture;

            int length = screenWidth / backgroundTexture.Width + 1;
            if(length == 1)
            {
                length = 2;
            }

            xPositions = new int[length];
            for(int i = 0; i < length; i++)
            {

            }

            xPositionOffset = 0;

            this.velocity = velocity;
        }

        public void Update()
        {
            xPositionOffset -= velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var x in xPositions)
            {
                spriteBatch.Draw(texture, new Vector2(x - xPositionOffset, 0), Color.White);
            }
        }
    }
}