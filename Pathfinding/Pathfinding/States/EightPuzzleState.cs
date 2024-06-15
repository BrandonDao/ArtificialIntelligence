using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Pathfinding.States
{
    public struct EightPuzzleState(int[,] board, Point emptyTile) : IState
    {
        public const int PuzzleSize = 3;

        public int[,] Board = board;
        public Point EmptyTile = emptyTile;

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not EightPuzzleState other) return false;

            for (int r = 0; r < PuzzleSize; r++)
            {
                for (int c = 0; c < PuzzleSize; c++)
                {
                    if (board[r, c] != other.Board[r, c]) return false;
                }
            }
            return true;
        }

        public static bool operator ==(EightPuzzleState left, EightPuzzleState right) => left.Equals(right);
        public static bool operator !=(EightPuzzleState left, EightPuzzleState right) => !(left == right);

        public override readonly int GetHashCode() => base.GetHashCode();
    }
}