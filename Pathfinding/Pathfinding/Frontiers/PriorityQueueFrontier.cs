namespace Pathfinding.Frontiers
{
    public class PriorityQueueFrontier<T>() : IFrontier<T>
    {
        private readonly PriorityQueue<AgentData<T>, float> vertices = new();

        public int Count => vertices.Count;

        public void Enqueue(AgentData<T> vertex, float priority) => vertices.Enqueue(vertex, priority);
        public AgentData<T> Dequeue() => vertices.Dequeue();
    }
}