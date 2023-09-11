namespace GameTheoryLibrary
{
    public static class MonteCarloTree<T> where T : IGameState<T>
    {
        public class GameStateComparer : IComparer<T>
        {
            public int Compare(T? x, T? y) => (x!.Score / x!.SimulationCount).CompareTo(y!.Score / y!.SimulationCount);
        }

        public static T Search(T currentState, int iterations, Random random, GameStateComparer comparer)
        {
            for (int i = 0; i < iterations; i++)
            {
                T selectedState = MonteCarloTree<T>.Select(currentState);
                T expandedState = Expand(selectedState, random);
                double value = Simulate(expandedState, random);
                Backpropagate(value, expandedState);
            }

            T[] children = currentState.GetChildren();
            Array.Sort (children, comparer);

            return children[^1];
        }

        private static T Select(T currentState)
        {
            T currentStateCopy = currentState;

            while (currentStateCopy.IsExpanded)
            {
                T selectedChild = currentStateCopy;
                double maxUCT = double.NegativeInfinity;

                foreach (var child in currentStateCopy.GetChildren())
                {
                    double UCT = child.CalculateUCT(currentStateCopy);
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

        private static T Expand(T currentState, Random random)
        {
            if (currentState.IsTerminal) return currentState;

            T[] children = currentState.GetChildren();
            T child = children[random.Next(0, children.Length)];

            return child;
        }

        private static double Simulate(T currentState, Random random)
        {
            while (!currentState.IsTerminal)
            {
                T[] children = currentState.GetChildren();
                currentState = children[random.Next(0, children.Length)];
            }

            return currentState.Score;
        }

        private static void Backpropagate(double value, T currentState)
        {
            T currentStateCopy = currentState;

            while (currentStateCopy != null)
            {
                currentStateCopy.SimulationCount++;
                currentStateCopy.Score += value;
                currentStateCopy = currentStateCopy.Parent;

                value = -value;
            }
        }
    }
}