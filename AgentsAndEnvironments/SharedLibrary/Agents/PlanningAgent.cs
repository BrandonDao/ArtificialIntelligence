using SharedLibrary.Environments;
using SharedLibrary.Frontiers;
using SharedLibrary.Movement;
using SharedLibrary.Movement.Results;
using SharedLibrary.States;

namespace SharedLibrary.Agents
{
    public class PlanningAgent<TState> : IAgent<TState>
        where TState : IState
    {
        private readonly IFrontier<TState> frontier;
        private readonly IEnvironment<TState, PlanningMovement<TState>, PlanningResult<TState>> environment;
        private readonly Func<AgentData<TState>, HashSet<TState>, PlanningResult<TState>, float> getScore;

        private readonly HashSet<TState> visited;

        public AgentData<TState> CurrentData { get; private set; }

        public PlanningAgent(
            TState startingState,
            IFrontier<TState> frontier,
            IEnvironment<TState, PlanningMovement<TState>, PlanningResult<TState>> environment,
            Func<AgentData<TState>, HashSet<TState>, PlanningResult<TState>, float> getScore)
        {
            visited = [];
            this.frontier = frontier;
            this.environment = environment;
            this.getScore = getScore;

            CurrentData = new AgentData<TState>(startingState, new StateToken<IState>(startingState), founder: null, priority: 0, cumulativeCost: 0);

            environment.RegisterAgent(CurrentData.StateToken, CurrentData.State);

            frontier.Enqueue(CurrentData, CurrentData.Priority);
        }

        public bool MakeMove(Predicate<TState> hasReachedGoal)
        {
            CurrentData = frontier.Dequeue();
            visited.Add(CurrentData.State);

            List<PlanningMovement<TState>> movements = environment.GetMovements(CurrentData.StateToken);

            foreach (var move in movements)
            {
                foreach(PlanningResult<TState> result in move.Results)
                {
                    float newScore = getScore.Invoke(CurrentData, visited, result);

                    AgentData<TState> nextData = new(
                        state: result.SuccessorState,
                        stateToken: result.SuccessorStateToken,
                        founder: CurrentData,
                        priority: newScore,
                        cumulativeCost: CurrentData.CumulativeCost + result.Cost);

                    if (hasReachedGoal.Invoke(nextData.State))
                    {
                        CurrentData = nextData;
                        return true;
                    }

                    frontier.Enqueue(nextData, newScore);
                }
            }

            return false;
        }

        public AgentData<TState> GetFinishedState() => CurrentData;
    }
}