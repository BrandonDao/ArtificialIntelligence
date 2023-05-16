using FlappyBird.GameElements;
using Microsoft.Xna.Framework.Input;
using NeuralNetworkLibrary;
using NeuralNetworkLibrary.NetworkStructure;
using System;

namespace FlappyBird.NetworkElements
{
    public class Pigeon : Bird
    {
        public NeuralNetwork Network { get; set; }
        public int Score { get; set; }

        private readonly double[] inputs;

        public Pigeon(Animation flapAnimation, float textureScale, int xPosition, int screenHeight)
            : base(flapAnimation, textureScale, xPosition, screenHeight)
        {
            int inputCount = 3;

            Network = new NeuralNetwork(ActivationFunction.TanH, ErrorFunction.MeanSquaredError, inputCount, 3, 1);
            inputs = new double[inputCount];
        }

        public override void Update(TimeSpan elapsedGameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState, Pipe closestPipe)
        {
            if (IsDead) return;

            Score++;

            inputs[0] = (closestPipe.GapPosition.X - Position.X) / 1000f; // x distance to pipe
            inputs[1] = (closestPipe.GapPosition.Y - Position.Y) / 800f; // y distance
            inputs[2] = yVelocity;

            bool willJump = Network.Compute(inputs)[0] > .5;


            gravityApplicationTimer += elapsedGameTime;

            if (willJump)
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
    }
}