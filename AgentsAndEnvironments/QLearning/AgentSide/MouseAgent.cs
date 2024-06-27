using QLearning.EnvironmentSide;
using SharedLibrary.Agents;

namespace QLearning.AgentSide
{
    public class MouseAgent : IAgent<MouseState>
    {
        private const float defaultQ = 0;

        private MouseEnvironment environment;
        private Dictionary<MouseAgentMovement, float> QMap;

        public MouseAgent(MouseEnvironment environment)
        {
            this.environment = environment;
            QMap = [];
        }


        public bool MakeMove(Predicate<MouseState> hasReachedGoal)
        {
            List<MouseAgentMovement> movements = environment.GetMovements(agent: this);

            var movement = movements[Random.Shared.Next(0, movements.Count)];
            
            environment.MakeMove(agent: this, movement);

            ;
            ;

            return false;
        }
    }
}