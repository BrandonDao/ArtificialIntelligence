namespace Pathfinding.Frontiers
{
    public interface IFrontier<T>
    {
        public int Count { get; }
        public void Enqueue(AgentData<T> vertex, float priority);
        public AgentData<T> Dequeue();
    }
}