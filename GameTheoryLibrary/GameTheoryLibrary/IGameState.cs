namespace GameTheoryLibrary
{
    public interface IGameState<T> where T : IGameState<T>
    {
        /// <summary>
        /// Determines how focus is devided between expanding good moves more & exploring other moves
        /// </summary>
        public const float C = 1.5f;

        public double Score { get; set; }
        public int Alpha { get; set; }
        public int Beta { get; set; }
        public int SimulationCount { get; set; }
        public bool IsMin { get; }
        public bool IsTerminal { get; }
        public bool IsExpanded { get; protected set; }
        public T Parent { get; set; }
        public T[]? BackingChildren { get; }
        public T[] GetChildren();
        public double CalculateUCT(T parent);
    }
}