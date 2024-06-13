namespace Pathfinding.Wrappers
{
    public class Frontier<T>()
    {
        private readonly PriorityQueue<SearchState<T>, int> vertices = new();

        public int Count => vertices.Count;

        public void Enqueue(SearchState<T> vertex, int priority) => vertices.Enqueue(vertex, priority);
        public SearchState<T> Dequeue() => vertices.Dequeue();
    }
}