using System;

namespace TicTacToe
{
    public struct Board
    {
        [Flags]
        public enum CellType : byte
        {
            Empty = 0,
            X = 0b01,
            O = 0b10,
            leftMask  = 0b11 << 4,
            midMask   = 0b11 << 2,
            rightMask = 0b11,

            XWinAcross = X << 4 | X << 2 | X,
            OWinAcross = O << 4 | O << 2 | O,

            mid_O  = O << 2,
            mid_X  = X << 2,
            left_O = O << 4,
            left_X = X << 4,
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

        public static bool operator==(Board lhs, Board rhs)
        {
            for(int i = 0; i < 3; i++)
            {
                if (lhs.board[i] != rhs.board[i]) return false;
            }
            return true;
        }
        public static bool operator!=(Board lhs, Board rhs)
        {
            for (int i = 0; i < 3; i++)
            {
                if (lhs.board[i] != rhs.board[i]) return true;
            }
            return false;
        }

        public readonly CellType TopLeft  { get => board[0] & CellType.leftMask;  set => board[0] = (board[0] & ~CellType.leftMask)  | (CellType)((int)value << 4); }
        public readonly CellType TopMid   { get => board[0] & CellType.midMask;   set => board[0] = (board[0] & ~CellType.midMask)   | (CellType)((int)value << 2); }
        public readonly CellType TopRight { get => board[0] & CellType.rightMask; set => board[0] = (board[0] & ~CellType.rightMask) | value;                       }
        public readonly CellType MidLeft  { get => board[1] & CellType.leftMask;  set => board[1] = (board[1] & ~CellType.leftMask)  | (CellType)((int)value << 4); }
        public readonly CellType Mid      { get => board[1] & CellType.midMask;   set => board[1] = (board[1] & ~CellType.midMask)   | (CellType)((int)value << 2); }
        public readonly CellType MidRight { get => board[1] & CellType.rightMask; set => board[1] = (board[1] & ~CellType.rightMask) | value;                       }
        public readonly CellType LowLeft  { get => board[2] & CellType.leftMask;  set => board[2] = (board[2] & ~CellType.leftMask)  | (CellType)((int)value << 4); }
        public readonly CellType LowMid   { get => board[2] & CellType.midMask;   set => board[2] = (board[2] & ~CellType.midMask)   | (CellType)((int)value << 2); }
        public readonly CellType LowRight { get => board[2] & CellType.rightMask; set => board[2] = (board[2] & ~CellType.rightMask) | value;                       }
    }
}