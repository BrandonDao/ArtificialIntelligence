using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.NetworkElements;
using System;
using System.Text;
using System.Text.Json;

namespace Snake
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private const int HabitatDisplayLength = 10;
        private const int HabitatCount = HabitatDisplayLength * HabitatDisplayLength;
        private const int HabitatSize = 8;
        private const int BorderSize = 1;
        private const int WindowSize = 800;
        private const int CellSize = 950 / (HabitatDisplayLength * HabitatSize + HabitatDisplayLength * BorderSize) - 1;

        private const int MovementsPerSecond = 10;

        private const double MutationRate = 0.5f;
        private const double min = -1;
        private const double max = 1;

        private NaturalSelection naturalSelection;
        private Habitat[] Habitats;

        private KeyboardState previousKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WindowSize;
            graphics.PreferredBackBufferHeight = WindowSize;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D blankTexture = new(GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });

            Habitats = new Habitat[HabitatCount];


            int x = 0;
            int y = 0;
            for (int i = 0; i < HabitatCount; i++)
            {
                Point drawOffset = new(
                    (x * HabitatSize * CellSize) + (x * BorderSize),
                    (y * HabitatSize * CellSize) + (y * BorderSize));

                Habitats[i] = new Habitat(HabitatSize, CellSize, drawOffset, blankTexture, blankTexture, blankTexture, MovementsPerSecond);

                x++;
                if (x == HabitatDisplayLength)
                {
                    x = 0;
                    y++;
                }
            }

            naturalSelection = new NaturalSelection(Random.Shared, Habitats, MutationRate, min, max);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();


            for (int i = 0; i < (keyboardState.IsKeyDown(Keys.Space) ? 100 : 1); i++)
            {
                bool isAllDead = true;

                foreach (var board in Habitats)
                {
                    board.Update(gameTime.ElapsedGameTime);

                    if (!board.IsSnakeDead)
                    {
                        isAllDead = false;
                    }
                }

                if (isAllDead || (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter)))
                {
                    foreach (var board in Habitats)
                    {
                        board.Reset();
                    }
                    naturalSelection.Select();
                }
            }

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            StringBuilder builder = new();

            /*
             * habitat count
             *      draw offset x
             *      draw offset y
             * 
             * random
             * mutator
             * 
             * activation func
             * error func
             * 
             * layer count
             *      input layer neuron count
             *          bias
             *          dendrite count
             *              weight
             *      hidden layer neuron count
             *          ...
             *      output layer neuron count
             */

            builder.Append(HabitatCount);
            foreach(var habitat in naturalSelection.Habitats)
            {
                builder.Append(habitat.DrawOffset.X);
                builder.Append(habitat.DrawOffset.Y);
            }

            builder.Append();


            builder.Append(JsonSerializer.Serialize(naturalSelection.Random));
            builder.Append(JsonSerializer.Serialize(naturalSelection.Mutator));


            base.OnExiting(sender, args);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();

            foreach (var board in Habitats)
            {
                board.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}