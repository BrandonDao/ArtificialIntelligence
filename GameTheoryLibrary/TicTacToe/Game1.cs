using GameTheoryLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static TicTacToe.Board;

namespace TicTacToe
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private const int CellSize = 250;
        private const int BorderSize = 5;

        //private MiniMaxTree<TicTacToeGameState> tree;
        private TicTacToeGameState originalGameState;
        private TicTacToeGameState currentGameState;
        private MonteCarloTree<TicTacToeGameState>.GameStateComparer gameStateComparer;
        private Texture2D blankTexture;
        private Texture2D XTexture;
        private Texture2D OTexture;

        private Color hoverColor;
        private Color gameNotOverColor;
        private Color gameOverColor;
        private Point scaledPos;

        private bool isGameOver;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            int boardSize = 3 * CellSize + 2 * BorderSize;
            graphics.PreferredBackBufferWidth = boardSize;
            graphics.PreferredBackBufferHeight = boardSize;
        }

        protected override void Initialize()
        {
            var testBoard = new Board()
            {
                //LowRight = CellType.X,
                
                //Mid = CellType.O,
                //MidLeft = CellType.O,

                //TopLeft = CellType.X,

            };

            originalGameState = new(testBoard, isMin: true);
            currentGameState = originalGameState;

            gameStateComparer = new();

            //tree = new(currentGameState);
            //tree.GenerateTree();

            //tree.FindProblem();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { new Color(217, 213, 207) });

            gameNotOverColor = Color.MidnightBlue;
            gameNotOverColor.A = 25;
            gameOverColor = Color.Red;
            gameOverColor.A = 25;

            hoverColor = gameNotOverColor;

            XTexture = Content.Load<Texture2D>("torquepope");
            OTexture = Content.Load<Texture2D>("buffpope");
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                currentGameState = originalGameState;
                hoverColor = gameNotOverColor;
                isGameOver = false;
            }

            var mouseState = Mouse.GetState();
            scaledPos = mouseState.Position / new Point(CellSize + BorderSize);
            
            if (isGameOver) return;

            if (mouseState.LeftButton == ButtonState.Pressed && !(scaledPos.X < 0 || scaledPos.X > 2 || scaledPos.Y < 0 || scaledPos.Y > 2))
            {
                var newBoard = new Board(currentGameState.Board);
                 newBoard.board[scaledPos.Y] |= (CellType)((int)CellType.X << (scaledPos.X << 1));

                var possibleMoves = currentGameState.GetChildren();
                for (int i = 0; i < possibleMoves.Length; i++)
                {
                    var possibleMove = possibleMoves[i];

                    if (newBoard != possibleMove.Board) continue;

                    var temp = currentGameState;
                    currentGameState = possibleMove;
                    currentGameState.Parent = temp;

                    if (currentGameState.IsTerminal)
                    {
                        isGameOver = true;
                        hoverColor = gameOverColor;
                        return;
                    }

                    var bestAIMove = MonteCarloTree<TicTacToeGameState>.Search(currentGameState, iterations: 10_000, Random.Shared, gameStateComparer);
                    temp = currentGameState;
                    currentGameState = bestAIMove;
                    currentGameState.Parent = temp;

                    if (currentGameState.IsTerminal)
                    {
                        isGameOver = true;
                        hoverColor = gameOverColor;
                        return;
                    }
                    break;
                }
            }

            //if (mouseState.LeftButton == ButtonState.Pressed && !(scaledPos.X < 0 || scaledPos.X > 2 || scaledPos.Y < 0 || scaledPos.Y > 2))
            //{
            //    var newBoard = new Board(currentGameState.Board);
            //    newBoard.board[scaledPos.Y] |= (CellType)((int)CellType.X << (scaledPos.X << 1));

            //    var possibleMoves = currentGameState.BackingChildren;
            //    foreach(var possibleMove in possibleMoves)
            //    {
            //        if(newBoard == possibleMove.Board)
            //        {
            //            currentGameState = possibleMove;

            //            if(currentGameState.IsTerminal)
            //            {
            //                isGameOver = true;
            //                hoverColor = gameOverColor;
            //                return;
            //            }

            //            currentGameState = currentGameState.BackingChildren[^1];
            //            if (currentGameState.IsTerminal)
            //            {
            //                isGameOver = true;
            //                hoverColor = gameOverColor;
            //                return;
            //            }
            //            break;
            //        }
            //    }
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            spriteBatch.Begin();

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    CellType mask = (CellType)(0b11 << (c << 1));
                    
                    var texture = blankTexture;
                    CellType cellType = (CellType)((int)(currentGameState.Board.board[r] & mask) >> (c << 1));
                    
                    switch(cellType)
                    {
                        case CellType.X: texture = XTexture; break;
                        case CellType.O: texture = OTexture; break;
                        default: break;
                    }

                    spriteBatch.Draw(texture, new Rectangle(c * CellSize + c * BorderSize, r * CellSize + r * BorderSize, CellSize, CellSize), Color.White);

                    //
                }
            }

            spriteBatch.Draw(
                blankTexture,
                new Rectangle(scaledPos.X * CellSize + scaledPos.X * BorderSize,
                    scaledPos.Y * CellSize + scaledPos.Y * BorderSize,
                    CellSize,
                    CellSize),
                hoverColor);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}