using Pathfinding.Agents;
using Pathfinding.States;

namespace Pathfinding.Frontiers
{
    public class PriorityQueueFrontier<TState>() : IFrontier<TState>
        where TState : IState
    {
        private readonly PriorityQueue<Agent<TState>.Data, float> vertices = new();

        public int Count => vertices.Count;

        public void Enqueue(Agent<TState>.Data vertex, float priority) => vertices.Enqueue(vertex, priority);
        public Agent<TState>.Data Dequeue() => vertices.Dequeue();
    }
}