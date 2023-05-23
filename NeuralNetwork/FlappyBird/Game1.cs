using FlappyBird.GameElements;
using FlappyBird.NetworkElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlappyBird
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private KeyboardState previousKeyboardState;

        private Pigeon[] birds;
        private List<Pipe> Pipes;
        private int closestPipeIndex;

        private PigeonTrainer trainer;

        private bool isFast;

        private enum GameStates
        {
            Waiting,
            Playing,
        }

        private GameStates gameState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameState = GameStates.Waiting;

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
                new Pipe(400, 5),
                new Pipe(800, 5),
                new Pipe(1200, 5),
                new Pipe(1600, 5),
            };

            birds = new Pigeon[100];

            for (int i = 0; i < birds.Length; i++)
            {
                birds[i] = (new Pigeon(
                    flapAnimation: new Animation(
                        animationTextures: new Texture2D[] { downFlap, midFlap, upFlap },
                        timePerFrame: TimeSpan.FromMilliseconds(100)),
                    textureScale: 1,
                    xPosition: 100,
                    screenHeight: graphics.PreferredBackBufferHeight));
            }

            trainer = new PigeonTrainer(new Random('c' + 'a' + 't'), birds, mutationRate: .5f, min: 0, max: 1);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

            isFast = keyboardState.IsKeyDown(Keys.Space);

            for (int i = 0; i < (isFast ? 25 : 1); i++)
            {
                switch (gameState)
                {
                    case GameStates.Waiting:
                        if (true /*keyboardState.IsKeyDown(Keys.Space)*/)
                        {
                            closestPipeIndex = 0;
                            gameState = GameStates.Playing;
                            foreach (var bird in birds)
                            {
                                bird.Score = 0;
                                bird.Update(gameTime.ElapsedGameTime, keyboardState, previousKeyboardState, Pipes[closestPipeIndex]);
                            }
                        }
                        break;

                    case GameStates.Playing:
                        foreach (var pipe in Pipes)
                        {
                            pipe.Update();
                        }

                        Rectangle closestPipeHitbox = Pipes[closestPipeIndex].TopHithox;
                        if (closestPipeHitbox.X + closestPipeHitbox.Width < 0)
                        {
                            Pipes[closestPipeIndex].Reset(xPosition: 1560);

                            closestPipeIndex++;
                            if (closestPipeIndex == Pipes.Count)
                            {
                                closestPipeIndex = 0;
                            }
                        }

                        bool isAnythingAlive = false;

                        foreach (var bird in birds)
                        {
                            bird.Update(gameTime.ElapsedGameTime, keyboardState, previousKeyboardState, Pipes[closestPipeIndex]);

                            if (!bird.IsDead)
                            {
                                isAnythingAlive = true;
                            }
                        }

                        if (!isAnythingAlive)
                        {
                            gameState = GameStates.Waiting;

                            trainer.Train();

                            Pipes[0].Reset(400);
                            Pipes[1].Reset(800);
                            Pipes[2].Reset(1200);
                            Pipes[3].Reset(1600);
                        }
                        break;
                }
            }

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);

            foreach (Pipe pipe in Pipes)
            {
                pipe.Draw(spriteBatch);
            }

            foreach (var bird in birds)
            {
                bird.Draw(spriteBatch);
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}