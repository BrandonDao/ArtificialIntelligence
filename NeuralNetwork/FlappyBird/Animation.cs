using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlappyBird
{
    public class Animation
    {
        public Texture2D[] AnimationTextures { get; set; }
        private int animationIndex;
        private bool hasFinishedAnimation;

        public TimeSpan Timer { get; set; }
        public TimeSpan TimePerFrame { get; set; }

        public Animation(Texture2D[] animationTextures, TimeSpan timePerFrame)
        {
            AnimationTextures = animationTextures;

            Timer = TimeSpan.Zero;
            TimePerFrame = timePerFrame;
        }

        public void Start()
        {
            animationIndex = 0;
            Timer = TimeSpan.Zero;
            hasFinishedAnimation = false;
        }


        public void Update(TimeSpan elapsedGameTime, ref Texture2D currentTexture)
        {
            if (hasFinishedAnimation) return;



            currentTexture = AnimationTextures[animationIndex];
            
            Timer += elapsedGameTime;

            if(Timer > TimePerFrame)
            {
                Timer = TimeSpan.Zero;
                animationIndex++;

                hasFinishedAnimation = animationIndex == AnimationTextures.Length;
            }
        }
    }
}
