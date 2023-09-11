namespace GameTheoryLibrary
{
    public class MiniMaxTree<T> where T : IGameState<T>
    {
        public class GameStateComparer : IComparer<T>
        {
            public int Compare(T? x, T? y) => x!.Score.CompareTo(y!.Score);
        }
        public T Head { get; private set; }
        public GameStateComparer Comparer { get; private set; }

        public MiniMaxTree(T head)
        {
            Comparer = new();
            Head = head;
        }

        public void GenerateTree()
            => GenerateTree(Head, isMin: true, alpha: int.MinValue, beta: int.MaxValue); // "depth-first backpropogation"

        private void GenerateTree(T currentNode, bool isMin, int alpha, int beta)
        {
            if (currentNode.IsTerminal) return;

            T[] children = currentNode.GetChildren();

            for (int i = 0; i < children.Length; i++)
            {
                if (!children[i].IsTerminal)
                {
                    GenerateTree(children[i], !isMin, alpha, beta);
                }

                if (isMin && children[i].Score < beta)
                {
                    beta = (int)children[i].Score;
                }
                else if (!isMin && children[i].Score > alpha)
                {
                    alpha = (int)children[i].Score;
                }

                if (beta < alpha)
                {
                    if (isMin && children.Length > i + 1)
                    {
                        ;
                    }

                    if (children.Length >= 6 && !isMin)
                    {
                        ;
                    }
                    break;
                }
            }

            currentNode.Score = children[0].Score;

            foreach (var child in children)
            {
                if (isMin ^ (child.Score > currentNode.Score))
                {
                    currentNode.Score = child.Score;
                }
            }

            Array.Sort(children, Comparer);
        }

        public void FindProblem()
        {
            FindProblem(Head, true);
        }

        private void FindProblem(T currentNode, bool isMin)
        {
            T[]? children = currentNode.BackingChildren;

            if (children == null) return;

            if (!isMin)
            {
                if (!children[^1].IsTerminal)
                {
                    FindProblem(children[^1], true);
                }
                return;
            }

            for (int i = 0; i < children.Length; i++)
            {
                T child = children[i];

                if (child.IsTerminal) continue;


                if (child.BackingChildren == null)
                {
                    ;
                }
                else
                {
                    FindProblem(child, !isMin);
                }

            }
        }
    }
}