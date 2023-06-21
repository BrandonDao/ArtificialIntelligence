using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.NetworkElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snake
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private const int HabitatDisplayLength = 10;
        private const int HabitatCount = HabitatDisplayLength * HabitatDisplayLength;
        private const int HabitatSize = 16;
        private const int BorderSize = 1;
        private const int CellSize = 2;
        private const int WindowSize = HabitatDisplayLength * HabitatSize * CellSize + HabitatDisplayLength * BorderSize;

        private const int MovementsPerSecond = 5;
        private int updateSpeed = 1;

        private const double MutationRate = 0.5f;
        private const double Min = -1;
        private const double Max = 1;

        private const bool WillLoadFromFile = true;

        private NaturalSelection naturalSelection;
        private Habitat[] Habitats;

        private KeyboardState previousKeyboardState;

        private List<int> topNetworkAverageFitnesses;

        private readonly TimeSpan speedChangeDelay = TimeSpan.FromMilliseconds(150);
        private TimeSpan speedChangeTimer;

        private Color backgroundColor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = WindowSize,
                PreferredBackBufferHeight = WindowSize
            };

            Window.Title = "Speed: " + updateSpeed.ToString();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Habitats = new Habitat[HabitatCount];
            topNetworkAverageFitnesses = new List<int>();

            backgroundColor = new Color(10, 10, 10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D blankTexture = new(GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });

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

            if (WillLoadFromFile)
            {
                string[] info = File.ReadAllLines(@"C:\Users\brand\Documents\Github\NeuralNetworks\NeuralNetwork\Snake\SavedNetworks.txt");

                int infoIndex = 0;
                foreach (var habitat in Habitats)
                {
                    for (int layerIndex = 1; layerIndex < habitat.Python.Network.Layers.Length; layerIndex++)
                    {
                        foreach (var neuron in habitat.Python.Network.Layers[layerIndex].Neurons)
                        {
                            neuron.Bias = double.Parse(info[infoIndex]);
                            infoIndex++;

                            foreach (var dendrite in neuron.Dendrites)
                            {
                                dendrite.Weight = double.Parse(info[infoIndex]);
                                infoIndex++;
                            }
                        }
                    }
                }
            }

            naturalSelection = new NaturalSelection(Random.Shared, Habitats, MutationRate, Min, Max, !WillLoadFromFile);
        }

        protected override void Update(GameTime gameTime)
        {
            speedChangeTimer += gameTime.ElapsedGameTime;
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

            if(keyboardState.IsKeyDown(Keys.Enter))
            {
                updateSpeed = 1;
                Window.Title = "Speed: 1";
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && speedChangeTimer > speedChangeDelay)
            {
                updateSpeed += keyboardState.IsKeyDown(Keys.RightShift) ? 10 : 1;
                Window.Title = "Speed: " + updateSpeed.ToString();
                speedChangeTimer = TimeSpan.Zero;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && speedChangeTimer > speedChangeDelay)
            {
                updateSpeed -= keyboardState.IsKeyDown(Keys.RightShift) ? 10 : 1;
                speedChangeTimer = TimeSpan.Zero;

                if (updateSpeed < 1)
                {
                    updateSpeed = 1;
                }

                Window.Title = "Speed: " + updateSpeed.ToString();
            }

            for (int i = 0; i < updateSpeed; i++)
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

                if (isAllDead)
                {
                    naturalSelection.Select();

                    int sum = 0;
                    for (int topNetIndex = 0; topNetIndex < NaturalSelection.TopSurvivalThreshold; topNetIndex++)
                    {
                        sum += naturalSelection.Habitats[topNetIndex].Python.Score;
                    }

                    topNetworkAverageFitnesses.Add((int)(sum / (NaturalSelection.TopSurvivalThreshold * naturalSelection.Habitats.Length)));

                    foreach (var board in Habitats)
                    {
                        board.Reset();
                    }
                }
            }

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            StringBuilder builder = new();

            foreach (var habitat in Habitats)
            {
                for (int layerIndex = 1; layerIndex < habitat.Python.Network.Layers.Length; layerIndex++)
                {
                    foreach (var neuron in habitat.Python.Network.Layers[layerIndex].Neurons)
                    {
                        builder.AppendLine(neuron.Bias.ToString());

                        foreach (var dendrite in neuron.Dendrites)
                        {
                            builder.AppendLine(dendrite.Weight.ToString());
                        }
                    }
                }
            }

            File.WriteAllText(@"C:\Users\brand\Documents\Github\NeuralNetworks\NeuralNetwork\Snake\SavedNetworks.txt", builder.ToString());

            builder.Clear();

            foreach (var datapoint in topNetworkAverageFitnesses)
            {
                builder.AppendLine(datapoint.ToString());
            }

            string previousData = File.ReadAllText(@"C:\Users\brand\Documents\Github\NeuralNetworks\NeuralNetwork\Snake\AverageFitnesses.txt");
            builder.Insert(0, previousData);

            File.WriteAllText(@"C:\Users\brand\Documents\Github\NeuralNetworks\NeuralNetwork\Snake\AverageFitnesses.txt", builder.ToString());


            base.OnExiting(sender, args);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            //GraphicsDevice.Clear(Color.Black);
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