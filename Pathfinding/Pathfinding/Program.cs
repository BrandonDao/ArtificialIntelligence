using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;
using System.Drawing;

namespace Pathfinding
{
    public partial class Program
    {
        public static AgentData<T> FindPath<T, TFrontier, TEnvironment>(
                T start,
                Func<AgentData<T>, HashSet<T>, Edge<T>, float> getPriority)
            where T : IState
            where TFrontier : IFrontier<T>, new()
            where TEnvironment : IEnvironment<T>, new()
        {
            IEnvironment<T> environment = new TEnvironment();
            IFrontier<T> frontier = new TFrontier();
            frontier.Enqueue(new(start, founder: null, priority: 0, distance: 0), priority: 0);

            HashSet<T> visited = [];

            while (frontier.Count > 0)
            {
                AgentData<T> currState = frontier.Dequeue();
                visited.Add(currState.Value);

                List<Edge<T>> children = environment.GetSuccessors(currState.Value);

                foreach (Edge<T> edge in children)
                {
                    float newPriority = getPriority.Invoke(currState, visited, edge);

                    AgentData<T> nextState = new(
                        value: edge.End,
                        founder: currState,
                        priority: newPriority,
                        distance: currState.DistanceFromStart + edge.Weight);

                    if (nextState.Value.Equals(environment.GoalState)) return nextState;

                    frontier.Enqueue(nextState, newPriority);
                }
            }

            throw new ArgumentException("Invalid search parameters!");
        }

        private static void Main()
        {
            EightPuzzleState start = new(new int[,]
            {
                { 2,5,7 },
                { 3,1,4 },
                { 8,6,0 }
            },
            new Point(2, 2));

            EightPuzzleState end = new(new int[,]
            {
                { 1,2,3 },
                { 4,5,6 },
                { 7,8,0 }
            },
            new Point(2, 2));

            var solvedState = FindPath<EightPuzzleState, PriorityQueueFrontier<EightPuzzleState>, EightPuzzle>(
                start: start,
                getPriority: (AgentData<EightPuzzleState> curr, HashSet<EightPuzzleState> visited, Edge<EightPuzzleState> edge)
                    => curr.DistanceFromStart + edge.Weight + EightPuzzle.DistanceFromSolved(edge.End));

            for (var a = solvedState; a != null; a = a.Predecessor)
            {
                Console.WriteLine($"{a.Value.Board[0, 0]}|{a.Value.Board[0, 1]}|{a.Value.Board[0, 2]}\n"
                                + $"{a.Value.Board[1, 0]}|{a.Value.Board[1, 1]}|{a.Value.Board[1, 2]}\n"
                                + $"{a.Value.Board[2, 0]}|{a.Value.Board[2, 1]}|{a.Value.Board[2, 2]}\n");
            }


            //var dfs = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
            //    => visited.Contains(edge.End) ? float.PositiveInfinity : curr.Priority - 1);

            //var bfs = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
            //    => visited.Contains(edge.End) ? float.PositiveInfinity: curr.Priority + 1);

            //var ucs = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
            //    => curr.DistanceFromStart + edge.Weight);

            //var aStar = Pathfind(start, end, (SearchState<Point> curr, HashSet<Vertex<Point>> visited, Edge<Point> edge)
            //    => curr.DistanceFromStart + edge.Weight + octileHeuristic(edge));
        }
    }
}