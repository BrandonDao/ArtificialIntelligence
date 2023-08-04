namespace GameTheoryLibrary
{
    public class MiniMaxTree<T> where T : IGameState<T>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static T Monke { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public T Head { get; private set; }

        public MiniMaxTree(T head)
        {
            Head = head;
        }

        public void GenerateTree()
            => GenerateTree(Head, isMin: true); // "depth-first backpropogation"

        private void GenerateTree(T currentNode, bool isMin)
        {
            if (currentNode.IsTerminal) return;

            T[] children = currentNode.GetChildren();

            if (children.Length == 0)
            {
                ;
            }

            currentNode.Score = children[0].Score;

            foreach (var child in children)
            {
                if (!child.IsTerminal)
                {
                    GenerateTree(child, !isMin);
                }

                if (isMin ^ (child.Score > currentNode.Score))
                {
                    currentNode.Score = child.Score;
                }
            }
        }
    }
}