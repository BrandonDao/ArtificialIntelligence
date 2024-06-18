using Pathfinding.Agents;
using Pathfinding.States;

namespace Pathfinding.Frontiers
{
    public class PriorityQueueFrontier<TState>() : IFrontier<TState>
        where TState : IState
    {
        private readonly PriorityQueue<AgentData<TState>, float> vertices = new();

        public int Count => vertices.Count;

        public void Enqueue(AgentData<TState> vertex, float priority) => vertices.Enqueue(vertex, priority);
        public AgentData<TState> Dequeue() => vertices.Dequeue();
    }
}