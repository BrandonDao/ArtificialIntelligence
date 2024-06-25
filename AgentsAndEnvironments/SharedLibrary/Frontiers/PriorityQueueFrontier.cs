using Pathfinding.Agents;
using Pathfinding.States;

namespace Pathfinding.Frontiers
{
    public class PriorityQueueFrontier<TState>() : IFrontier<TState>
        where TState : IState
    {
        private readonly PriorityQueue<AgentData<TState>, float> priorityQ = new();

        public int Count => priorityQ.Count;

        public void Enqueue(AgentData<TState> vertex, float priority) => priorityQ.Enqueue(vertex, priority);
        public AgentData<TState> Dequeue() => priorityQ.Dequeue();
    }
}