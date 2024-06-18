using Pathfinding.States;

namespace Pathfinding.Frontiers
{
    public class PriorityQueueFrontier<TState>() : IFrontier<TState>
        where TState : IState
    {
        private readonly PriorityQueue<Agent<TState>.AgentData, float> vertices = new();

        public int Count => vertices.Count;

        public void Enqueue(Agent<TState>.AgentData vertex, float priority) => vertices.Enqueue(vertex, priority);
        public Agent<TState>.AgentData Dequeue() => vertices.Dequeue();
    }
}