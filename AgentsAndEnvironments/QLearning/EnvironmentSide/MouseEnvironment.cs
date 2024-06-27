using QLearning.AgentSide;
using SharedLibrary;
using SharedLibrary.Agents;
using SharedLibrary.Environments;
using SharedLibrary.States;
using Point = Microsoft.Xna.Framework.Point;

namespace QLearning.EnvironmentSide
{
    public class MouseEnvironment() : IEnvironment<MouseState, MouseAgentMovement, MouseAgentResult>
    {
        public const int Width = 4;
        public const int Height = 3;
        public readonly Point WallPosition = new(1, 1);
        public readonly Point FirePosition = new(3, 1);
        public readonly Point CheesePosition = new(3, 0);
        
        private readonly Point[][] possibleMoves = [
            [new(0, -1)/*, new(-1, 0), new(1, 0)*/],
            [new(0, 1)/*, new(-1, 0), new(1, 0)*/],
            [new(-1, 0)/*, new(0, -1), new(0, 1)*/],
            [new(1, 0)/*, new(0, -1), new(0, 1)*/],
        ];
        private readonly Dictionary<StateToken<IState>, MouseState> stateMap = [];

        private readonly Dictionary<MouseState, List<MouseMovement>> stateToMovement = [];
        private readonly Dictionary<MouseState, List<MouseAgentMovement>> stateToAgentMovement = [];

        public readonly Dictionary<IAgent<MouseState>, StateToken<IState>> AgentToStateToken = [];
        public readonly Dictionary<IAgent<MouseState>, MouseState> AgentToStartingState = [];

        private readonly Dictionary<MouseAgentMovement, MouseMovement> agentMovementToMovement = [];

        public void RegisterAgent(IAgent<MouseState> agent, MouseState startingState, StateToken<IState> startingStateToken)
        {
            AgentToStateToken.Add(agent, startingStateToken);
            AgentToStartingState.Add(agent, startingState);
            stateMap.Add(startingStateToken, startingState);
        }

        public List<MouseAgentMovement> GetMovements(IAgent<MouseState> agent, StateToken<IState> stateToken)
            => throw new InvalidDataException("Mouse agents are now permissed to plan!");
        public List<MouseAgentMovement> GetMovements(IAgent<MouseState> agent)
        {
            MouseState state = stateMap[AgentToStateToken[agent]];

            if (stateToAgentMovement.TryGetValue(state, out List<MouseAgentMovement>? agentMovements)) return agentMovements;

            agentMovements = [];
            List<MouseMovement> movements = [];

            foreach (var possibleMoveSet in possibleMoves)
            {
                MouseMovement movement = new(new MouseResult[1]);
                MouseAgentMovement agentMovement = new(new MouseAgentResult[1]);

                movement.Results[0] = GetResult(state, possibleMoveSet[0], probability: 1f);
                agentMovement.Results[0] = movement.Results[0].AgentResult;

                //movement.Results[1] = GetResult(state, possibleMoveSet[1], probability: .1f);
                //agentMovement.Results[1] = movement.Results[1].AgentResult;

                //movement.Results[2] = GetResult(state, possibleMoveSet[2], probability: .1f);
                //agentMovement.Results[2] = movement.Results[2].AgentResult;

                movements.Add(movement);
                agentMovements.Add(agentMovement);

                agentMovementToMovement.Add(agentMovement, movement);
            }

            stateToMovement.Add(state, movements);
            stateToAgentMovement.Add(state, agentMovements);

            return agentMovements;

            MouseResult GetResult(MouseState state, Point moveOffset, float probability)
            {
                var newPosition = new Point(state.Position.X + moveOffset.X, state.Position.Y + moveOffset.Y);

                MouseState newState = new(
                    position: newPosition,
                    type: newPosition == FirePosition ? MouseState.Types.Fire
                        : newPosition == CheesePosition ? MouseState.Types.Cheese
                            : MouseState.Types.Empty);

                MouseState newAgentState = new(newPosition, MouseState.Types.Unknown);

                StateToken<IState> newStateToken = new(newAgentState);

                MouseAgentResult agentResult = new(newState, newStateToken);
                MouseAgentResult actualResult;

                if (newState.Position.X is < 0 or >= Width || newState.Position.Y is < 0 or >= Height || newState.Position == WallPosition)
                {
                    actualResult = new(state, new StateToken<IState>(state));
                }
                else
                {
                    actualResult = agentResult;
                }
                stateMap.Add(actualResult.SuccessorStateToken, actualResult.SuccessorState);


                return new MouseResult(agentResult, actualResult, probability);
            }
        }

        public MouseAgentResult MakeMove(IAgent<MouseState> agent, MouseAgentMovement agentMovement)
        {
            var stateToken = AgentToStateToken[agent];
            var state = stateMap[stateToken];

            //stateMap.Remove(stateToken);

            if (state.Type is MouseState.Types.Fire or MouseState.Types.Cheese)
            {
                var newStateToken = new StateToken<IState>(AgentToStartingState[agent]);

                AgentToStateToken[agent] = newStateToken;
                stateMap.Add(newStateToken, AgentToStartingState[agent]);

                return new MouseAgentResult(AgentToStartingState[agent], newStateToken);
            }

            var movement = agentMovementToMovement[agentMovement];

            float accumulator = 0;
            float random = Random.Shared.NextSingle();
            foreach (MouseResult result in movement.Results)
            {
                accumulator += result.Probability;

                if (accumulator < random) continue;
                
                AgentToStateToken[agent] = result.ActualResult.SuccessorStateToken;

                return result.ActualResult;
            }
            throw new InvalidOperationException("Random assignment of result failed");
        }
    }
}