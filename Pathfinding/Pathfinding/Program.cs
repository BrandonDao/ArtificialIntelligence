using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;
using System.Drawing;

namespace Pathfinding
{

    public partial class Program
    {
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

            Agent<EightPuzzleState, PriorityQueueFrontier<EightPuzzleState>> eightPuzzleAgent = new(
                start,
                new EightPuzzleEnvironment(),
                getPriority: (AgentData<EightPuzzleState> curr, HashSet<EightPuzzleState> visited, Edge<EightPuzzleState> edge)
                => curr.DistanceFromStart + edge.Weight + EightPuzzleEnvironment.DistanceFromSolved(edge.End));

            AgentData<EightPuzzleState>? b;

            while (!eightPuzzleAgent.MakeMove(out b)) ;


            for (var a = b; a != null; a = a.Predecessor)
            {
                Console.WriteLine($"{a.State.Board[0, 0]}|{a.State.Board[0, 1]}|{a.State.Board[0, 2]}\n"
                                + $"{a.State.Board[1, 0]}|{a.State.Board[1, 1]}|{a.State.Board[1, 2]}\n"
                                + $"{a.State.Board[2, 0]}|{a.State.Board[2, 1]}|{a.State.Board[2, 2]}\n");
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