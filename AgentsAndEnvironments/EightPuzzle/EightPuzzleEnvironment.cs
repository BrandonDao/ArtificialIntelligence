using SharedLibrary.Agents;
using SharedLibrary.Environments;
using SharedLibrary.States;
using System.Drawing;

namespace SharedLibrary
{

    public class EightPuzzleEnvironment() : IEnvironment<EightPuzzleState>
    {
        private static EightPuzzleState goalState = new(board: new int[,]
            {
                { 1,2,3 },
                { 4,5,6 },
                { 7,8,0 }
            }, emptyTile: new Point(2, 2));
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

        private readonly Dictionary<StateToken<IState>, EightPuzzleState> stateMap = [];

        public void RegisterAgent(StateToken<IState> currentStateToken, EightPuzzleState state)
            => stateMap.Add(currentStateToken, state);

        public List<Movement<EightPuzzleState>> GetMovements(StateToken<IState> stateToken)
        {
            List<Movement<EightPuzzleState>> movements = [];

            var castedState = stateMap[stateToken];
            stateMap.Remove(stateToken);

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

                    var newState = new EightPuzzleState(newBoard, newEmptySpace);

                    movements.Add(new Movement<EightPuzzleState>([
                        new Movement<EightPuzzleState>.Result(
                            successor: newState,
                            successorToken: new StateToken<IState>(newState),
                            cost: 1,
                            probability: 1)
                        ]));

                    stateMap.Add(movements[^1].Results[^1].SuccessorStateToken, movements[^1].Results[^1].SuccessorState);
                }
            }
            return movements;
        }

        public AgentData<EightPuzzleState> MakeMove(AgentData<EightPuzzleState> newStateData) => newStateData;

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