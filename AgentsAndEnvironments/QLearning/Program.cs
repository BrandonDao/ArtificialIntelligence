using SharedLibrary;
using SharedLibrary.Agents;
using SharedLibrary.Environments;
using SharedLibrary.Movement;
using SharedLibrary.Movement.Results;
using SharedLibrary.States;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace QLearning
{
    public struct MouseState(Point position, MouseState.Types type) : IState
    {
        public enum Types
        {
            Empty,
            Fire,
            Cheese
        }
        public Point Position = position;
        public Types Type = type;
    }

    public class MouseAgent : IAgent<MouseState>
    {
        int score;

        public AgentData<MouseState> GetFinishedState() => throw new NotImplementedException();

        public bool MakeMove(Predicate<MouseState> hasReachedGoal)
        {
            throw new NotImplementedException();
        }
    }

    public class MouseAgentResult(MouseState successorState) : IResult<MouseState>
    {
        public MouseState SuccessorState => successorState;
        public StateToken<IState> SuccessorStateToken => new(successorState);
    }
    public class MouseAgentMovement(MouseAgentResult[] results) : IMovement<MouseState, MouseAgentResult>
    {
        public MouseAgentResult[] Results { get; } = results;
    }


    public class MouseMovement(MouseResult[] results)
    {
        public readonly MouseResult[] Results = results;
    }
    public class MouseResult(MouseAgentResult result, MouseAgentResult actualResult, float probability)
    {
        public MouseAgentResult AgentResult = result;
        public MouseAgentResult ActualResult = actualResult;
        public float Probability = probability;
    }

    public class MouseEnvironment() : IEnvironment<MouseState, MouseAgentMovement, MouseAgentResult>
    {
        readonly Dictionary<StateToken<IState>, MouseState> stateMap = [];
        readonly Size[][] possibleMoves = [
            [new(0, -1), new(-1, 0), new(1, 0)],
            [new(0, 1), new(-1, 0), new(1, 0)],
            [new(-1, 0), new(0, -1), new(0, 1)],
            [new(1, 0), new(0, -1), new(0, 1)],
        ];

        const int Width = 4;
        const int Height = 3;
        readonly Point WallPosition = new(1, 1);
        readonly Point FirePosition = new(3, 1);
        readonly Point CheesePosition = new(3, 0);

        readonly Dictionary<MouseState, List<MouseMovement>> stateToMovement = [];
        readonly Dictionary<MouseState, List<MouseAgentMovement>> stateToAgentMovement = [];

        public void RegisterAgent(StateToken<IState> currentStateToken, MouseState state)
        {
            stateMap.Add(currentStateToken, state);
        }
        public List<MouseAgentMovement> GetMovements(StateToken<IState> stateToken)
        {
            MouseState state = stateMap[stateToken];

            if (stateToAgentMovement.TryGetValue(state, out List<MouseAgentMovement>? agentMovements)) return agentMovements;

            agentMovements = [];
            List<MouseMovement> movements = [];

            foreach(var possibleMoveSet in possibleMoves)
            {
                MouseMovement movement = new(new MouseResult[3]);
                MouseAgentMovement agentMovement = new(new MouseAgentResult[3]);

                movement.Results[0] = GetResult(state, possibleMoveSet[0], probability: .8f);
                agentMovement.Results[0] = movement.Results[0].AgentResult;
                
                movement.Results[1] = GetResult(state, possibleMoveSet[1], probability: .1f);
                agentMovement.Results[1] = movement.Results[1].AgentResult;
                
                movement.Results[2] = GetResult(state, possibleMoveSet[2], probability: .1f);
                agentMovement.Results[2] = movement.Results[2].AgentResult;

                movements.Add(movement);
                agentMovements.Add(agentMovement);
            }

            stateToMovement.Add(state, movements);
            stateToAgentMovement.Add(state, agentMovements);

            return agentMovements;

            MouseResult GetResult(MouseState state, Size moveOffset, float probability)
            {
                var newPosition = Point.Add(state.Position, new(0, -1));

                MouseState newState = new(
                    position: newPosition,
                    type: newPosition == FirePosition ? MouseState.Types.Fire
                        : (newPosition == CheesePosition ? MouseState.Types.Cheese
                            : MouseState.Types.Empty));

                MouseAgentResult agentResult = new(newState);
                MouseAgentResult actualResult;

                if (newState.Position.X is < 0 or > Width || newState.Position.Y is < 0 or > Height || newState.Position == WallPosition)
                {
                    actualResult = new(state);
                }
                else
                {
                    actualResult = agentResult;
                }
                return new MouseResult(agentResult, actualResult, probability);
            }
        }

        public MouseAgentResult MakeMove(MouseAgentMovement movement)
        {
            throw new NotImplementedException();
        }
    }





    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}