using Microsoft.VisualStudio.Utilities;
using NeuralNetworkLibrary;
using NeuralNetworkLibrary.NetworkStructure;
using QLearning.EnvironmentSide;
using SharedLibrary.Agents;

namespace QLearning.AgentSide
{
    public class MouseAgent : IAgent<MouseState>
    {
        private class Experience(MouseState state, MouseAgentMovement.Directions direction, MouseState resultState, double reward)
        {
            public MouseState State = state;
            public MouseAgentMovement.Directions MovementDirection = direction;
            public MouseState ResultState = resultState;
            public double Reward = reward;
        }

        public MouseState CurrentState { get; private set; }

        public Dictionary<MouseAgentMovement, MouseAgentResult> MovementToResult { get; }
        public Dictionary<MouseState, List<MouseAgentMovement>> StateToBestMovements { get; }
        public Dictionary<MouseState, HashSet<MouseAgentMovement>> StateToAllMovements { get; }

        private const int experiencesPerTrain = 1000;
        private const int iterationsPerTrain = 5000;

        private readonly double[][] inputs;
        private readonly double[][] expectedOutputs;
        private double[] predictionNetOutputs;
        private double[] stableNetOutputs;

        private readonly CircularBuffer<Experience> experienceBuffer;

        private NeuralNetwork predictionNet;
        private NeuralNetwork stableNet;

        private readonly MouseEnvironment environment;

        private int currentIteration;

        public float learningRate;
        public float decayFactor;
        public float costOfLiving;
        public float epsilon;

        public MouseAgent(MouseEnvironment environment, MouseState startState)
        {
            this.environment = environment;
            CurrentState = startState;
            //QMap = [];
            MovementToResult = [];
            StateToBestMovements = [];
            StateToAllMovements = [];
            experienceBuffer = new(capacity: 10000);
            predictionNet = new(ActivationFunction.ReLU, ErrorFunction.MeanSquaredError, 2, 5, 5, 4);
            predictionNet.Randomize(Random.Shared, 0, 1);
            stableNet = predictionNet.Clone();

            predictionNetOutputs = new double[4];
            stableNetOutputs = new double[4];
            inputs = new double[experiencesPerTrain][];
            expectedOutputs = new double[experiencesPerTrain][];
            for (int i = 0; i < experiencesPerTrain; i++)
            {
                inputs[i] = new double[2];
                expectedOutputs[i] = new double[4];
            }

            learningRate = 0;
            decayFactor = 0;
            costOfLiving = 0;
            epsilon = 0;
        }

        public bool MakeMove(Predicate<MouseState> hasReachedGoal)
        {
            if (experienceBuffer.IsFull)
            {
                List<Experience> experienceSample = new(capacity: experiencesPerTrain);

                while (experienceSample.Count < experiencesPerTrain)
                {
                    experienceSample.Add(experienceBuffer[Random.Shared.Next(0, experienceBuffer.Count)]);
                }

                int offset = Random.Shared.Next(0, experienceBuffer.Count - experiencesPerTrain);

                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i][0] = experienceSample[i].State.Position.X / (double)MouseEnvironment.Width;
                    inputs[i][1] = experienceSample[i].State.Position.Y / (double)MouseEnvironment.Height;

                    predictionNetOutputs = predictionNet.Compute(inputs[i]);
                    stableNetOutputs = stableNet.Compute(inputs[i]);

                    for(int j = 0; j < expectedOutputs[i].Length; j++)
                    {
                        if (!StateToBestMovements.TryGetValue(experienceSample[i].ResultState, out List<MouseAgentMovement>? nextMoves)) continue;

                        var nextMove = nextMoves[Random.Shared.Next(0, nextMoves.Count)];

                        double[] nextMoveOutputs = stableNet.Compute(inputs: [
                            experienceSample[i].ResultState.Position.X / (double)MouseEnvironment.Width,
                            experienceSample[i].ResultState.Position.Y / (double)MouseEnvironment.Height]);

                        int movementIdx = 0;
                        for (int x = 1; x < nextMoveOutputs.Length; x++)
                        {
                            if (nextMoveOutputs[x] > nextMoveOutputs[movementIdx])
                            {
                                movementIdx = x;
                            }
                        }

                        if (j == (int)experienceSample[i].MovementDirection)
                        {
                            expectedOutputs[i][j] =
                                ((((1 - learningRate) * stableNetOutputs[j])
                              + (learningRate * (experienceSample[i].Reward + decayFactor * nextMoveOutputs[movementIdx])))
                              + 1000) / 1100;
                        }
                        else
                        {
                            expectedOutputs[i][j] = stableNetOutputs[j];
                        }
                    }
                }

                predictionNet.TrainWithGradientDescent(inputs, expectedOutputs, learningRate);

                if (currentIteration > iterationsPerTrain)
                {
                    currentIteration = 0;

                    stableNet = predictionNet.Clone();
                }
            }

            List<MouseAgentMovement> movements = environment.GetMovements(agent: this);
            var q = new float[movements.Count];

            MouseAgentMovement movement = SelectMovement(CurrentState, movements);

            MouseAgentResult result = environment.MakeMove(agent: this, movement);


            if (!MovementToResult.TryAdd(movement, result))
            {
                MovementToResult[movement] = result;
            }

            float score = -costOfLiving;
            if (result.State.Position == CurrentState.Position)
            {
                score -= 100;
            }
            else
            {
                if (result.State.Type is MouseState.Types.Cheese)
                {
                    //QMap[movement] = 100 - costOfLiving;
                    CurrentState = result.State;
                    return false;
                }
                else if (result.State.Type is MouseState.Types.Fire)
                {
                    //QMap[movement] = -1000 - costOfLiving;
                    CurrentState = result.State;
                    return false;
                }
            }

            experienceBuffer.Add(new(CurrentState, movement.Direction, result.State, score));

            //float nextQ = defaultQ;
            //if (stateToBestMovements.TryGetValue(result.State, out List<MouseAgentMovement>? nextMoves))
            //{
            //    var nextMove = nextMoves[Random.Shared.Next(0, nextMoves.Count)];

            //    //if (!QMap.TryGetValue(nextMove, out nextQ))
            //    //{
            //    //    nextQ = defaultQ;
            //    //}
            //}

            //QMap[movement] = (1 - learningRate) * QMap[movement] + learningRate * (score + decayFactor * nextQ);

            CurrentState = result.State;
            currentIteration++;
            return false;
        }

        private MouseAgentMovement SelectMovement(MouseState state, List<MouseAgentMovement> movements)
        {
            MouseAgentMovement movement;

            float choice = Random.Shared.NextSingle();
            if (choice > epsilon)
            {
                double[] outputs = stableNet.Compute(inputs: [
                    state.Position.X / (double)MouseEnvironment.Width,
                    state.Position.Y / (double)MouseEnvironment.Height]);

                int movementIdx = 0;
                for (int i = 1; i < outputs.Length; i++)
                {
                    if (outputs[i] > outputs[movementIdx])
                    {
                        movementIdx = i;
                    }
                }

                movement = movements[movementIdx];
                if (!StateToBestMovements.TryAdd(CurrentState, [movement]))
                {
                    StateToBestMovements[CurrentState] = [movement];
                }
            }
            else
            {
                movement = movements[Random.Shared.Next(0, movements.Count)];
            }

            if (!StateToAllMovements.TryAdd(CurrentState, [movement]))
            {
                StateToAllMovements[CurrentState].Add(movement);
            }

            return movement;
        }
    }
}