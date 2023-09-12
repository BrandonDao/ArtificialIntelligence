using System.Diagnostics;

namespace GameTheoryLibrary
{
    public static class MonteCarloTree<T> where T : IGameState<T>
    {
        [DebuggerDisplay("Score: {Score}, SimulationCount: {SimulationCount}")]
        public class Node
        {
            const double C = 1.5;
            public T State { get; private set; }
            public int SimulationCount { get; set; }
            public double Score { get; set; }
            public bool IsExpanded { get; private set; }
            public Node? Parent { get; private set; }
            private Node[]? children;

            public Node(T state) => State = state;

            public Node[] GetChildren()
            {
                if (children != null) return children;

                IsExpanded = true;
                
                T[] stateChildren = State.GetChildren();
                children = new Node[stateChildren.Length];
                for(int i = 0; i < children.Length; i++)
                {
                    children[i] = new Node(stateChildren[i]) { Parent = this };
                }
                return children;
            }

            public double CalculateUCT()
                => SimulationCount == 0 ? double.PositiveInfinity : (Score / SimulationCount) + C * Math.Sqrt(Math.Log(Parent!.SimulationCount, Math.E) / SimulationCount);
        }

        public class GameStateComparer : IComparer<Node>
        {
            public int Compare(Node? x, Node? y) => (x!.Score / x!.SimulationCount).CompareTo(y!.Score / y!.SimulationCount);
        }

        public static T Search(T currentState, int iterations, Random random, GameStateComparer comparer)
        {
            Node root = new(currentState);

            for (int i = 0; i < iterations; i++)
            {
                Node selectedState = Select(root);
                Node expandedState = Expand(selectedState, random);
                double value = Simulate(expandedState,  random);
                Backpropagate(value, expandedState);
            }

            Node[] children = root.GetChildren();
            Array.Sort(children, comparer: comparer);

            return children[^1].State;
        }

        private static Node Select(Node currentState)
        {
            var currentStateCopy = currentState;

            while (currentStateCopy.IsExpanded)
            {
                var selectedChild = currentStateCopy;
                double maxUCT = double.NegativeInfinity;

                foreach (var child in currentStateCopy.GetChildren())
                {
                    double UCT = child.CalculateUCT();
                    if (UCT > maxUCT)
                    {
                        maxUCT = UCT;
                        selectedChild = child;
                    }
                }
                if (selectedChild.Equals(currentStateCopy)) break;

                currentStateCopy = selectedChild;
            }

            return currentStateCopy;
        }

        private static Node Expand(Node currentNode, Random random)
        {
            if (currentNode.State.IsTerminal) return currentNode;

            Node[] children = currentNode.GetChildren();
            return children[0]; //random.Next(0, children.Length)];
        }

        private static double Simulate(Node currentNode, Random random)
        {
            while (!currentNode.State.IsTerminal)
            {
                Node[] children = currentNode.GetChildren();
                currentNode = children[random.Next(0, children.Length)];
            }

            return currentNode.State.Score;
        }

        private static void Backpropagate(double value, Node currentNode)
        {
            Node? currentNodeCopy = currentNode;

            while (currentNodeCopy != null)
            {
                currentNodeCopy.SimulationCount++;
                currentNodeCopy.Score += value;
                currentNodeCopy = currentNodeCopy.Parent;

                value = -value;
            }
        }
    }
}