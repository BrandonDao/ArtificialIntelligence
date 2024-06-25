using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;

namespace Pathfinding.Agents
{
    public class PlanningAgent<TState> : IAgent<TState>
        where TState : IState
    {
        private readonly IFrontier<TState> frontier;
        private readonly IEnvironment<TState> environment;
        private readonly Func<AgentData<TState>, HashSet<TState>, Movement<TState>.Result, float> getPriority;

        private readonly HashSet<TState> visited;

        private AgentData<TState> currentData;

        public PlanningAgent(
            TState startingState,
            IFrontier<TState> frontier,
            IEnvironment<TState> environment,
            Func<AgentData<TState>, HashSet<TState>, Movement<TState>.Result, float> getPriority)
        {
            visited = [];
            this.frontier = frontier;
            this.environment = environment;
            this.getPriority = getPriority;

            currentData = new AgentData<TState>(startingState, new StateToken<IState>(startingState), founder: null, priority: 0, cumulativeCost: 0);

            environment.RegisterAgent(currentData.StateToken, currentData.State);

            frontier.Enqueue(currentData, currentData.Priority);
        }

        public bool MakeMove(Predicate<TState> hasReachedGoal)
        {
            currentData = frontier.Dequeue();
            visited.Add(currentData.State);

            List<Movement<TState>> movements = environment.GetMovements(currentData.StateToken);

            foreach (var move in movements)
            {
                foreach(Movement<TState>.Result result in move.Results)
                {
                    float newPriority = getPriority.Invoke(currentData, visited, result);

                    AgentData<TState> nextData = new(
                        state: result.SuccessorState,
                        stateToken: result.SuccessorStateToken,
                        founder: currentData,
                        priority: newPriority,
                        cumulativeCost: currentData.CumulativeCost + result.Cost);

                    if (hasReachedGoal.Invoke(nextData.State))
                    {
                        currentData = nextData;
                        return true;
                    }

                    frontier.Enqueue(nextData, newPriority);
                }
            }

            return false;
        }

        public AgentData<TState> GetFinishedState() => currentData;
    }
}