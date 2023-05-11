using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FlappyBird
{
    public class Bird
    {
        public Point Position => position;
        public bool IsDead { get; set; }
         
        private const int GravityAcceleration = -3;
        private const int JumpVelocity = 9;
        private const int MaxVelocity = 12;

        private readonly TimeSpan gravityApplicationTime;
        private TimeSpan gravityApplicationTimer;

        private readonly Animation flapAnimation;
        private Texture2D currentTexture;

        private readonly int screenHeight;
        private readonly int width;
        private readonly int height;

        private Point position;
        private int yVelocity;

        private Rectangle hitbox;

        public Bird(Animation flapAnimation, float textureScale, int xPosition, int screenHeight)
        {
            gravityApplicationTime = TimeSpan.FromMilliseconds(100);
            gravityApplicationTimer = TimeSpan.Zero;

            this.flapAnimation = flapAnimation;

            this.screenHeight = screenHeight;
            width = (int)(flapAnimation.AnimationTextures[0].Width * textureScale);
            height = (int)(flapAnimation.AnimationTextures[0].Height * textureScale);

            position = new Point(xPosition, (screenHeight / 2) - (height / 2));
            yVelocity = 0;

            hitbox = new Rectangle(Position.X, Position.Y, width, height);
        }

        public void Update(TimeSpan elapsedGameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState, Pipe nearestPipe)
        {
            if (IsDead) return;

            gravityApplicationTimer += elapsedGameTime;

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                flapAnimation.Start();
                yVelocity = JumpVelocity;
            }
            else if (gravityApplicationTimer > gravityApplicationTime)
            {
                gravityApplicationTimer = TimeSpan.Zero;
                yVelocity += GravityAcceleration;
            }

            if (Math.Abs(yVelocity) > MaxVelocity)
            {
                yVelocity = Math.Sign(yVelocity) * MaxVelocity;
            }

            position.Y -= yVelocity;
            if (position.Y < 0)
            {
                position.Y = 0;
                yVelocity = 0;
            }
            else if (position.Y + height > screenHeight)
            {
                position.Y = (screenHeight / 2) - (height / 2);
                IsDead = true;
            }

            if(hitbox.Intersects(nearestPipe.TopHithox) || hitbox.Intersects(nearestPipe.BottomHitbox))
            {
                position.Y = (screenHeight / 2) - (height / 2);
                IsDead = true;
            }

            hitbox.Y = position.Y;

            flapAnimation.Update(elapsedGameTime, ref currentTexture);
        }

        public void Draw(SpriteBatch spriteBatch)
            => spriteBatch.Draw(currentTexture, hitbox, IsDead ? Color.Red : Color.White);
    }
}
