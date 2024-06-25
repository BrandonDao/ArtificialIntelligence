using Pathfinding.Agents;
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

            var environment = new EightPuzzleEnvironment();

            PlanningAgent<EightPuzzleState> eightPuzzleAgent = new(
                startingState: start,
                frontier: new PriorityQueueFrontier<EightPuzzleState>(),
                environment: environment,
                getPriority: (AgentData<EightPuzzleState> curr, HashSet<EightPuzzleState> visited, Movement<EightPuzzleState>.Result result)
                => curr.CumulativeCost + result.Cost + EightPuzzleEnvironment.DistanceFromSolved(result.SuccessorState));

            while (!eightPuzzleAgent.MakeMove((state) => state == environment.GoalState)) ;
                

            for (var a = eightPuzzleAgent.GetFinishedState(); a != null; a = a.Predecessor)
            {
                Console.WriteLine($"{a.State.Board[0, 0]}|{a.State.Board[0, 1]}|{a.State.Board[0, 2]}\n"
                                + $"{a.State.Board[1, 0]}|{a.State.Board[1, 1]}|{a.State.Board[1, 2]}\n"
                                + $"{a.State.Board[2, 0]}|{a.State.Board[2, 1]}|{a.State.Board[2, 2]}\n");
            }
        }
    }
}