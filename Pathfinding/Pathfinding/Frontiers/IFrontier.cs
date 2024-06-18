using Pathfinding.Agents;
using Pathfinding.States;

namespace Pathfinding.Frontiers
{
    public interface IFrontier<TState>
        where TState : IState
    {
        public int Count { get; }
        public void Enqueue(Agent<TState>.Data data, float priority);
        public Agent<TState>.Data Dequeue();
    }
}