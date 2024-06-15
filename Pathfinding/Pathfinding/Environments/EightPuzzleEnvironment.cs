using Pathfinding.States;
using System.Drawing;

namespace Pathfinding.Environments
{
    public class EightPuzzle : IEnvironment<EightPuzzleState>
    {
        private static EightPuzzleState goalState = new(board: new int[,]
            {
                { 1,2,3 },
                { 4,5,6 },
                { 7,8,0 }
            },
            emptyTile: new Point(2, 2));
        public EightPuzzleState GoalState => goalState;

        private static readonly Dictionary<int, Point> TileToPosition = new()
        {
            [1] = new Point(0, 0),
            [2] = new Point(0, 1),
            [3] = new Point(0, 2),
            [4] = new Point(1, 0),
            [5] = new Point(1, 1),
            [6] = new Point(1, 2),
            [7] = new Point(2, 0),
            [8] = new Point(2, 1),
            [0] = new Point(2, 2),
        };
        private static readonly List<Point> moves = [new(1, 0), new(-1, 0), new(0, 1), new(0, -1)];

        public List<Edge<EightPuzzleState>> GetSuccessors(IState state)
        {
            List<Edge<EightPuzzleState>> newStates = [];

            var castedState = (EightPuzzleState)state;

            foreach (var move in moves)
            {
                var newEmptySpace = new Point(castedState.EmptyTile.X + move.X, castedState.EmptyTile.Y + move.Y);

                if (newEmptySpace.X is >= 0 and < EightPuzzleState.PuzzleSize
                && newEmptySpace.Y is >= 0 and < EightPuzzleState.PuzzleSize)
                {
                    var newBoard = new int[EightPuzzleState.PuzzleSize, EightPuzzleState.PuzzleSize];
                    for (int r = 0; r < EightPuzzleState.PuzzleSize; r++)
                    {
                        for (int c = 0; c < EightPuzzleState.PuzzleSize; c++)
                        {
                            newBoard[r, c] = castedState.Board[r, c];
                        }
                    }
                    newBoard[castedState.EmptyTile.X, castedState.EmptyTile.Y] = castedState.Board[newEmptySpace.X, newEmptySpace.Y];
                    newBoard[newEmptySpace.X, newEmptySpace.Y] = 0;

                    newStates.Add(
                        new Edge<EightPuzzleState>(
                            start: castedState,
                            end: new EightPuzzleState(newBoard, newEmptySpace),
                            weight: 1));
                }
            }
            return newStates;
        }

        public static float DistanceFromSolved(EightPuzzleState state)
        {
            double error = 0;
            for (int r = 0; r < EightPuzzleState.PuzzleSize; r++)
            {
                for (int c = 0; c < EightPuzzleState.PuzzleSize; c++)
                {
                    error +=
                        Math.Pow(TileToPosition[state.Board[r, c]].X - r, 2)
                      + Math.Pow(TileToPosition[state.Board[r, c]].Y - c, 2);
                }
            }
            return (float)error;
        }
    }
}