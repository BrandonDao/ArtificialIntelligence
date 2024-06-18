using Pathfinding.States;

namespace Pathfinding.Agents
{

    public class AgentData<TState>(TState state, StateToken<IState> stateToken, AgentData<TState>? founder, float priority, float distance)
    {
        public TState State { get; } = state;
        public StateToken<IState> StateToken { get; } = stateToken;
        public AgentData<TState>? Predecessor { get; } = founder;
        public float Priority { get; } = priority;
        public float DistanceFromStart { get; } = distance;
    }
}