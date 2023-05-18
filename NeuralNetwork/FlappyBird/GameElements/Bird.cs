using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace FlappyBird.GameElements
{
    [DebuggerDisplay("Y: {Position.Y}")]
    public class Bird
    {
        public Point Position => position;
        public bool IsDead { get; set; }

        protected const int GravityAcceleration = -3;
        protected const int JumpVelocity = 7;
        protected const int MaxVelocity = 12;

        protected readonly TimeSpan gravityApplicationTime;
        protected TimeSpan gravityApplicationTimer;

        protected readonly Animation flapAnimation;
        protected Texture2D currentTexture;

        protected readonly int screenHeight;
        protected readonly int width;
        protected readonly int height;

        protected Point position;
        protected int yVelocity;

        protected Rectangle hitbox;

        public Bird(Animation flapAnimation, float textureScale, int xPosition, int screenHeight)
        {
            gravityApplicationTime = TimeSpan.FromMilliseconds(100);
            gravityApplicationTimer = TimeSpan.Zero;

            this.flapAnimation = flapAnimation;
            currentTexture = flapAnimation.AnimationTextures[0];

            this.screenHeight = screenHeight;
            width = (int)(flapAnimation.AnimationTextures[0].Width * textureScale);
            height = (int)(flapAnimation.AnimationTextures[0].Height * textureScale);

            position = new Point(xPosition, screenHeight / 2 - height / 2);
            yVelocity = 0;

            hitbox = new Rectangle(Position.X, Position.Y, width, height);
        }

        public virtual void Update(TimeSpan elapsedGameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState, Pipe closestPipe)
        {
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
                position.Y = screenHeight / 2 - height / 2;
                IsDead = true;
            }

            if (hitbox.Intersects(closestPipe.TopHithox) || hitbox.Intersects(closestPipe.BottomHitbox))
            {
                position.Y = screenHeight / 2 - height / 2;
                IsDead = true;
            }

            hitbox.Y = position.Y;

            flapAnimation.Update(elapsedGameTime, ref currentTexture);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead) return;

            spriteBatch.Draw(currentTexture, hitbox, Color.White);
        }
    }
}
