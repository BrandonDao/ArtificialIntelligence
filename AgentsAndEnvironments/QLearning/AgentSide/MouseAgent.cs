using QLearning.EnvironmentSide;
using SharedLibrary.Agents;

namespace QLearning.AgentSide
{
    public class MouseAgent(MouseEnvironment environment, MouseState startState) : IAgent<MouseState>
    {
        private const float defaultQ = 0;

        private readonly MouseEnvironment environment = environment;

        public MouseState CurrentState = startState;
        public Dictionary<MouseAgentMovement, float> QMap { get; private set; } = [];

        private readonly Dictionary<MouseAgentMovement, MouseAgentResult> movementToResult = [];
        public readonly Dictionary<MouseState, List<MouseAgentMovement>> stateToBestMovements = [];
        public readonly Dictionary<MouseState, HashSet<MouseAgentMovement>> stateToAllMovements = [];

        public float learningRate = 0;
        public float decayFactor = 0;
        public float costOfLiving = 0;
        public float epsilon = 0;

        public bool MakeMove(Predicate<MouseState> hasReachedGoal)
        {
            List<MouseAgentMovement> movements = environment.GetMovements(agent: this);
            List<MouseAgentMovement> bestExpectedMoves = new(capacity: movements.Count) { movements[0] };
            var q = new float[movements.Count];

            MouseAgentMovement movement;

            float choice = Random.Shared.NextSingle();
            if (choice > epsilon)
            {
                for (int i = 0; i < movements.Count; i++)
                {
                    if (!stateToAllMovements.TryAdd(CurrentState, [movements[i]]))
                    {
                        stateToAllMovements[CurrentState].Add(movements[i]);
                    }

                    if (QMap.TryGetValue(movements[i], out float qValue))
                    {
                        q[i] = qValue;
                    }
                    else
                    {
                        QMap.Add(movements[i], defaultQ);
                        q[i] = defaultQ;
                    }

                    if (q[i] > QMap[bestExpectedMoves[0]])
                    {
                        bestExpectedMoves.Clear();
                        bestExpectedMoves.Add(movements[i]);
                    }
                    else if (q[i] == QMap[bestExpectedMoves[0]])
                    {
                        bestExpectedMoves.Add(movements[i]);
                    }
                }

                if (!stateToBestMovements.TryAdd(CurrentState, bestExpectedMoves))
                {
                    stateToBestMovements[CurrentState] = bestExpectedMoves;
                }

                movement = bestExpectedMoves[Random.Shared.Next(0, bestExpectedMoves.Count)];
            }
            else
            {
                movement = movements[Random.Shared.Next(0, movements.Count)];

                if (!stateToAllMovements.TryAdd(CurrentState, [movement]))
                {
                    stateToAllMovements[CurrentState].Add(movement);
                }

                QMap.TryAdd(movement, defaultQ);
            }

            MouseAgentResult result = environment.MakeMove(agent: this, movement);


            if (!movementToResult.TryAdd(movement, result))
            {
                movementToResult[movement] = result;
            }

            float score = -costOfLiving;
            if (result.State.Position == CurrentState.Position)
            {
                score -= 100;
            }
            else
            {
                if (result.State.Type is MouseState.Types.Cheese)
                {
                    QMap[movement] = 100 - costOfLiving;
                    CurrentState = result.State;
                    return false;
                }
                else if (result.State.Type is MouseState.Types.Fire)
                {
                    QMap[movement] = -1000 - costOfLiving;
                    CurrentState = result.State;
                    return false;
                }
            }

            float nextQ = defaultQ;
            if (stateToBestMovements.TryGetValue(result.State, out List<MouseAgentMovement>? nextMoves))
            {
                var nextMove = nextMoves[Random.Shared.Next(0, nextMoves.Count)];

                if (!QMap.TryGetValue(nextMove, out nextQ))
                {
                    nextQ = defaultQ;
                }
            }

            QMap[movement] = (1 - learningRate) * QMap[movement] + learningRate * (score + decayFactor * nextQ);

            CurrentState = result.State;
            return false;
        }
    }
}