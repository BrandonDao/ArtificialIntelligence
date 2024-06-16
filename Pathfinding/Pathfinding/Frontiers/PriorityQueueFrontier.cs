namespace Pathfinding.Frontiers
{
    public class PriorityQueueFrontier<T>() : IFrontier<T>
    {
        private readonly PriorityQueue<Agent.Data<T>, float> vertices = new();

        public int Count => vertices.Count;

        public void Enqueue(Agent.Data<T> vertex, float priority) => vertices.Enqueue(vertex, priority);
        public Agent.Data<T> Dequeue() => vertices.Dequeue();
    }
}