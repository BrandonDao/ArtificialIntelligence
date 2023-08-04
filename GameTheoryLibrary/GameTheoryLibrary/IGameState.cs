namespace GameTheoryLibrary
{
    public interface IGameState<T> where T : IGameState<T>
    {
        public int Score { get; set; }
        public bool IsTerminal { get; }
        public T[] GetChildren();
    }
}