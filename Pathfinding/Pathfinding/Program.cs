using System.Drawing;
using Pathfinding.Graph;
using Pathfinding.Wrappers;

namespace deletemePathfinding
{

    public class Program
    {
        public static SearchState<T> Pathfind<T>(Vertex<T> start, Vertex<T> end, Func<SearchState<T>, HashSet<Vertex<T>>, Edge<T>, int> getPriority)
        {
            Frontier<T> frontier = new();
            frontier.Enqueue(new(start, founder: null, priority: 0, distance: 0), priority: 0);

            HashSet<Vertex<T>> visited = [];

            while (frontier.Count > 0)
            {
                SearchState<T> currentState = frontier.Dequeue();
                visited.Add(currentState.Vertex);

                foreach (Edge<T> edge in currentState.Vertex.Edges)
                {
                    int newPriority = getPriority.Invoke(currentState, visited, edge);

                    SearchState<T> nextState = new(
                        vertex: edge.End,
                        founder: currentState,
                        priority: newPriority,
                        distance: currentState.DistanceFromStart + edge.Weight);

                    if (nextState.Vertex == end) return nextState;

                    frontier.Enqueue(nextState, newPriority);
                }
            }

            throw new ArgumentException("Invalid search parameters!");
        }

        private static void Main()
        {
            float sqrt2 = (float)Math.Sqrt(2);

            Vertex<Point> start = new(new Point(0));
            FillDown(start, limit: 2, sqrt2);

            Vertex<Point> end = start.Edges[0].End.Edges[1].End.Edges[1].End.Edges[1].End;

            float octileHeuristic(Edge<Point> e) =>
                  Math.Abs(e.Start.Value.X - e.End.Value.X)
                + (sqrt2 - 2)
                * int.Min(
                    Math.Abs(e.Start.Value.X - e.End.Value.X),
                    Math.Abs(e.Start.Value.Y - e.End.Value.Y));



            var dfs = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
                => visited.Contains(edge.End) ? int.MaxValue : curr.Priority - 1);

            var bfs = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
                => visited.Contains(edge.End) ? int.MaxValue : curr.Priority + 1);

            var ucs = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
                => (int)(curr.DistanceFromStart + edge.Weight));

            var aStar = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
                => (int)(curr.DistanceFromStart + edge.Weight + octileHeuristic(edge)));

            ;

            static void FillDown(Vertex<Point> current, int limit, float diagonalDist)
            {
                if (current.Value.X < limit)
                {
                    Vertex<Point> right = new(new Point(current.Value.X + 1, current.Value.Y));
                    current.Edges.Add(new Edge<Point>(current, right, 1));
                    right.Edges.Add(new Edge<Point>(right, current, 1));
                    FillDown(right, limit, diagonalDist);
                }
                if (current.Value.Y < limit)
                {
                    Vertex<Point> down = new(new Point(current.Value.X, current.Value.Y + 1));
                    current.Edges.Add(new Edge<Point>(current, down, 1));
                    down.Edges.Add(new Edge<Point>(down, current, 1));
                    FillDown(down, limit, diagonalDist);
                }
                if (current.Value.X < limit && current.Value.Y < limit)
                {
                    Vertex<Point> downright = new(new Point(current.Value.X + 1, current.Value.Y + 1));
                    current.Edges.Add(new Edge<Point>(current, downright, diagonalDist));
                    downright.Edges.Add(new Edge<Point>(downright, current, diagonalDist));
                    FillDown(downright, limit, diagonalDist);
                }
            }
        }
    }
}