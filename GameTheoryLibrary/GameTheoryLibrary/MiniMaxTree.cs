using System.Xml.Serialization;

namespace GameTheoryLibrary
{
    public class MiniMaxTree<T> where T : IGameState<T>
    {
        public class GameStateComparer : IComparer<T>
        {
            public int Compare(T? x, T? y) => x!.Score.CompareTo(y!.Score);
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static T Monke { get; set; }
        public static Stack<T> MonkeFamily = new();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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

            for(int i = 0; i < children.Length; i++)
            {
                if (!children[i].IsTerminal)
                {
                    GenerateTree(children[i], !isMin, alpha, beta);
                }

                if(isMin && children[i].Score < beta)
                {
                    beta = children[i].Score;
                }
                else if(!isMin && children[i].Score > alpha)
                {
                    alpha = children[i].Score;
                }

                if (beta < alpha)
                {
                    if(children.Length >= 6 && !isMin)
                    {
                        ;
                    }
                    break;
                }
            }

            currentNode.Score = children[0].Score;

            foreach(var child in children)
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
            T[] children = currentNode.PrivateChildren;

            if (children == null) return;

            if (!isMin && !currentNode.PrivateChildren[^1].IsTerminal) FindProblem(children[^1], true);

            for(int i = 0; i < children.Length; i++)
            {
                T child = children[i];
                
                if (child.IsTerminal) continue;


                if(child.PrivateChildren == null)
                {
                    if(isMin)
                    {
                        ;
                    }
                }
                else
                {
                    FindProblem(child, !isMin);
                }

            }
        }
    }
}