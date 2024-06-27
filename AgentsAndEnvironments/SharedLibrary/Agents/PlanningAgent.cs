using SharedLibrary.Environments;
using SharedLibrary.Frontiers;
using SharedLibrary.Movement;
using SharedLibrary.Movement.Results;
using SharedLibrary.States;

namespace SharedLibrary.Agents
{
    public class PlanningAgent<TState>(
        IFrontier<TState> frontier,
        IEnvironment<TState, PlanningMovement<TState>, PlanningResult<TState>> environment,
        Func<AgentData<TState>, HashSet<TState>, PlanningResult<TState>, float> getScore) : IAgent<TState>
        where TState : IState
    {
        private readonly IFrontier<TState> frontier = frontier;
        private readonly IEnvironment<TState, PlanningMovement<TState>, PlanningResult<TState>> environment = environment;
        private readonly Func<AgentData<TState>, HashSet<TState>, PlanningResult<TState>, float> getScore = getScore;

        private readonly HashSet<TState> visited = [];

        public AgentData<TState>? CurrentData { get; private set; }

        public void SpecifyStartState(TState state, StateToken<IState> stateToken)
        {
            CurrentData = new AgentData<TState>(state, stateToken, founder: null, priority: 0, cumulativeCost: 0);
            frontier.Enqueue(CurrentData, CurrentData.Priority);
        }

        public bool MakeMove(Predicate<TState> hasReachedGoal)
        {
            CurrentData = frontier.Dequeue();
            visited.Add(CurrentData.State);

            List<PlanningMovement<TState>> movements = environment.GetMovements(agent: this, CurrentData.StateToken);

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
    }
}