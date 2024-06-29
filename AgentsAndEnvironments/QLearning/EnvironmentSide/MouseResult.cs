using QLearning.AgentSide;

namespace QLearning.EnvironmentSide
{
    public struct MouseResult(MouseAgentResult result, MouseAgentResult actualResult, float probability)
    {
        public MouseAgentResult AgentResult = result;
        public MouseAgentResult ActualResult = actualResult;
        public float Probability = probability;
    }
}