using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.GameElements;
using System;
using System.Collections.Generic;

namespace Snake
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Random random;
        private GameBoard gameBoard;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            random = new Random('s' + 'n' + 'a' + 'k' + 'e');
            Food.Random = random;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D blankTexture = new(GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });

            Texture2D boardTexture = new(GraphicsDevice, 1, 1);
            boardTexture.SetData(new Color[] { Color.Black });


            gameBoard = new GameBoard(16, 10, new Point(2, 1), boardTexture, blankTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            gameBoard.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}