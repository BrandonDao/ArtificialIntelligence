namespace GameTheoryLibrary
{
    public interface IGameState<T> where T : IGameState<T>
    {
        public int Score { get; set; }
        public int Alpha { get; set; }
        public int Beta { get; set; }
        public bool IsTerminal { get; }
        public T[] GetChildren();
        public T[] PrivateChildren { get; }
    }
}