using System;

namespace TicTacToe
{
    public struct Board
    {
        [Flags]
        public enum CellType : byte
        {
            Empty = 0,
            X = 1,
            O = 2,
            leftMask  = 0b11 << 4,
            midMask   = 0b11 << 2,
            rightMask = 0b11,
            XWinAcross = 0b0001_0101,
            OWinAcross = 0b0010_1010,
            mid_O = 0b0000_0100,
            mid_X = 0b0000_1000,
            left_O = 0b0001_0000,
            left_X = 0b0010_0000,
        }

        public CellType[] board;

        public Board()
        {
            board = new CellType[3];
        }

        public Board(Board other)
        {
            board = new CellType[3];

            for (int r = 0; r < 3; r++)
            {
                board[r] = other.board[r];
            }
        }


        public CellType TopLeft  { readonly get => board[0] & CellType.leftMask;  set => board[0] = (board[0] & ~CellType.leftMask)  | (CellType)((int)value << 4); }
        public CellType TopMid   { readonly get => board[0] & CellType.midMask;   set => board[0] = (board[0] & ~CellType.midMask)   | (CellType)((int)value << 2); }
        public CellType TopRight { readonly get => board[0] & CellType.rightMask; set => board[0] = (board[0] & ~CellType.rightMask) | value;                       }
        public CellType MidLeft  { readonly get => board[1] & CellType.leftMask;  set => board[1] = (board[1] & ~CellType.leftMask)  | (CellType)((int)value << 4); }
        public CellType Mid      { readonly get => board[1] & CellType.midMask;   set => board[1] = (board[1] & ~CellType.midMask)   | (CellType)((int)value << 2); }
        public CellType MidRight { readonly get => board[1] & CellType.rightMask; set => board[1] = (board[1] & ~CellType.rightMask) | value;                       }
        public CellType LowLeft  { readonly get => board[2] & CellType.leftMask;  set => board[2] = (board[2] & ~CellType.leftMask)  | (CellType)((int)value << 4); }
        public CellType LowMid   { readonly get => board[2] & CellType.midMask;   set => board[2] = (board[2] & ~CellType.midMask)   | (CellType)((int)value << 2); }
        public CellType LowRight { readonly get => board[2] & CellType.rightMask; set => board[2] = (board[2] & ~CellType.rightMask) | value;                       }
    }
}