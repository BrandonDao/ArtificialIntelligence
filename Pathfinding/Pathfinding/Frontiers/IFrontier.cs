namespace Pathfinding.Frontiers
{
    public interface IFrontier<T>
    {
        public int Count { get; }
        public void Enqueue(Agent.Data<T> vertex, float priority);
        public Agent.Data<T> Dequeue();
    }
}