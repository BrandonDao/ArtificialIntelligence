using SharedLibrary.Agents;
using SharedLibrary.States;

namespace SharedLibrary.Frontiers
{
    public interface IFrontier<TState>
        where TState : IState
    {
        public int Count { get; }
        public void Enqueue(AgentData<TState> data, float priority);
        public AgentData<TState> Dequeue();
    }
}