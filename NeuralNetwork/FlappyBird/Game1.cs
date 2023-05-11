using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FlappyBird
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private KeyboardState previousKeyboardState;

        private Bird bird;
        private List<Pipe> Pipes;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D upFlap = Content.Load<Texture2D>("birdTextures/yellowbird-upflap");
            Texture2D midFlap = Content.Load<Texture2D>("birdTextures/yellowbird-midflap");
            Texture2D downFlap = Content.Load<Texture2D>("birdTextures/yellowbird-downflap");

            Texture2D pipeTexture = Content.Load<Texture2D>("pipe-green");


            Pipe.InitializeSharedData(pipeTexture, gapHeight: 125, gapWidth: 75, HeightOfScreen: graphics.PreferredBackBufferHeight);
            
            Pipes = new List<Pipe>()
            {
                new Pipe(600, 5),
                new Pipe(900, 5),
                new Pipe(1200, 5),
                new Pipe(1500, 5),
            };

            bird = new Bird(
                flapAnimation: new Animation(
                    animationTextures: new Texture2D[] { downFlap, midFlap, upFlap },
                    timePerFrame: TimeSpan.FromMilliseconds(100)),
                textureScale: 1,
                xPosition: 100,
                screenHeight: graphics.PreferredBackBufferHeight);
        }

        private void ResetPipes()
        {
            Pipes.Clear();
            int pos = 600;

            for(int i = 0; i < Pipes.Count; i++)
            {
                Pipes[i].Reset(pos);
                pos += 300;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();


            bool hasPipeGoneOffscreen = false;
            foreach(Pipe pipe in Pipes)
            {
                if (pipe.Update())
                {
                    hasPipeGoneOffscreen = true;
                }
            }

            if(hasPipeGoneOffscreen)
            {
                Pipes[0].Reset(Pipes[^1].GapPosition.X + 300);
            }

            bird.Update(gameTime.ElapsedGameTime, keyboardState, previousKeyboardState, nearestPipe: Pipes[0]);
            

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            bird.Draw(spriteBatch);

            foreach(Pipe pipe in Pipes)
            {
                pipe.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}