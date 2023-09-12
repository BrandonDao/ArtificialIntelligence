namespace GameTheoryLibrary
{
    public interface IGameState<T> where T : IGameState<T>
    {
        public double Score { get; set; }
        public int Alpha { get; set; }
        public int Beta { get; set; }
        public bool IsMin { get; }
        public bool IsTerminal { get; }
        public T[]? BackingChildren { get; }
        public T[] GetChildren();
    }
}