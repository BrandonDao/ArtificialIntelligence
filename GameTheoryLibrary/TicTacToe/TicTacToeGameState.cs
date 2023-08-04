using GameTheoryLibrary;
using System;
using System.Collections.Generic;
using static TicTacToe.Board;

namespace TicTacToe
{
    public class TicTacToeGameState : IGameState<TicTacToeGameState>
    {
        public int Score { get; set; }
        public bool IsTerminal { get; private set; }
        public Board Board { get; private set; }
        private TicTacToeGameState[] children;
        private bool isMin;

        public TicTacToeGameState(Board board, bool isMin)
        {
            Board = board;
            this.isMin = isMin;
            EvaluateBoard();
        }

        public void EvaluateBoard()
        {
            // empty board check
            if (Board.board[0] == CellType.Empty && Board.board[1] == CellType.Empty && Board.board[2] == CellType.Empty)
            {
                IsTerminal = false;
                Score = 0;
                return;
            }
            
            IsTerminal = true;

            // row checks
            if (Board.board[0] == CellType.XWinAcross || Board.board[1] == CellType.XWinAcross || Board.board[2] == CellType.XWinAcross)
            {
                Score = isMin ? 1 : -1;
                return;
            }
            if (Board.board[0] == CellType.OWinAcross || Board.board[1] == CellType.OWinAcross || Board.board[2] == CellType.OWinAcross)
            {
                Score = isMin ? -1 : 1;
                return;
            }

            // column checks
            int count;
            for (int c = 0; c < 3; c++)
            {
                count = 0;
                CellType mask = (CellType)(0b11 << (c << 1));

                for (int r = 0; r < 3; r++)
                {
                    count += (byte)(Board.board[r] & mask);
                }

                if (count == 0b0001_0101)
                {
                    Score = isMin ? 1 : -1;
                    return;
                }
                if (count == 0b0010_1010)
                {
                    Score = isMin ? -1 : 1;
                    return;
                }
            }

            // diagonal top/left -> bottom/right
            count = 0;
            for (int rc = 0; rc < 3; rc++)
            {
                CellType mask = (CellType)(0b11 << (rc << 1));
                count += (byte)(Board.board[rc] & mask);
            }
            if (count == 0b0001_0101)
            {
                Score = isMin ? 1 : -1;
                return;
            }
            if (count == 0b0010_1010)
            {
                Score = isMin ? -1 : 1;
                return;
            }

            // diagonal top/right -> bottom/left
            count = 0;
            for (int rc = 0; rc < 3; rc++)
            {
                CellType mask = (CellType)(0b11 << (rc << 1));
                count += (byte)(Board.board[2 - rc] & mask);
            }
            if (count == 0b0001_0101)
            {
                Score = isMin ? 1 : -1;
                return;
            }
            if (count == 0b0010_1010)
            {
                Score = isMin ? -1 : 1;
                return;
            }

            // draw check
            count = 0;
            for (int c = 0; c < 3; c++)
            {
                CellType mask = (CellType)(0b11 << (c << 1));

                for (int r = 0; r < 3; r++)
                {
                    if((Board.board[r] & mask) != CellType.Empty)
                    {
                        count++;
                    }
                }
            }
            if(count == 9)
            {
                Score = 0;
                return;
            }

            // assume game is not over
            IsTerminal = false;
            Score = 0;
        }

        public TicTacToeGameState[] GetChildren()
        {
            if (IsTerminal) throw new InvalidOperationException("why???");
            if (children != null) return children;

            List<TicTacToeGameState> newChildren = new(9);
            CellType move = isMin ? CellType.X : CellType.O;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    CellType mask = (CellType)(0b11 << (c << 1));

                    if ((Board.board[r] & mask) == CellType.Empty)
                    {
                        Board temp = new(Board);
                        temp.board[r] = (temp.board[r] & ~mask) | (CellType)((int)move << (c << 1));
                        newChildren.Add(new TicTacToeGameState(temp, !isMin));
                    }
                }
            }

            children = newChildren.ToArray();
            return children;
        }
    }
}