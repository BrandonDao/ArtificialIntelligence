using Pathfinding.States;

namespace Pathfinding.Frontiers
{
    public interface IFrontier<TState>
        where TState : IState
    {
        public int Count { get; }
        public void Enqueue(Agent<TState>.AgentData data, float priority);
        public Agent<TState>.AgentData Dequeue();
    }
}