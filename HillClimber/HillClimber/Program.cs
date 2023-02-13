using System;
using System.Runtime.InteropServices;

namespace HillClimber
{
    internal class Program
    {
        public class HillClimber<T>
        {
            public static void Climb(T current, T target, Action<T> mutator, Func<T, T, double> getError, Func<T, T> CopyT, Action<T, double, int> visualizer)
            {
                T previous = CopyT.Invoke(current);
                double error = getError.Invoke(current, target);
                double previousError = error;

                int iterations = 0;

                while (error != 0)
                {
                    mutator.Invoke(current);
                    error = getError(current, target);

                    if (error > previousError)
                    {
                        current = CopyT.Invoke(previous);
                    }
                    else
                    {
                        previousError = error;
                    }

                    previous = CopyT.Invoke(current);
                    iterations++;
                    visualizer.Invoke(current, error, iterations);
                }
            }
        }

        static void Main(string[] args)
        {
            char[] current = "~~~~~~      ".ToCharArray();
            char[] target = "Hello World!".ToCharArray();

            var mutate = (char[] chars) =>
            {
                Random RNG = new Random();
                int mutationIndex = RNG.Next(0, chars.Length);
                char character = chars[mutationIndex];
                char newCharacter = (char)(character + (RNG.Next(0, 2) == 1 ? 1 : -1));

                if (newCharacter > '~')
                {
                    newCharacter = (char)(character - 1);
                }
                else if (newCharacter < ' ')
                {
                    newCharacter = (char)(character + 1);
                }

                chars[mutationIndex] = newCharacter;
            };
            var smartGetError = (char[] current, char[] target) =>
            {
                double accumulativeError = 0;

                for (int i = 0; i < current.Length; i++)
                {
                    accumulativeError += Math.Abs(current[i] - target[i]) / (double)target[i];
                }
                return accumulativeError;
            };
            var badGetError = (char[] current, char[] target) =>
            {
                double correct = 0;

                for (int i = 0; i < current.Length; i++)
                {
                    if(current[i] == target[i]) correct++;
                }
                
                return Math.Abs(correct - target.Length) / (double)target.Length;
            };
            var copyFunc = (char[] chars) =>
            {
                char[] charsCopy = new char[chars.Length];
                Array.Copy(chars, charsCopy, chars.Length);
                return charsCopy;
            };
            var print = (char[] current, double error, int iterations) =>
            {
                Thread.Sleep(1);
                Console.Clear();
                Console.Write(current);
                Console.Write($" ({Math.Round(error, 2)}) {iterations}");
            };

            //HillClimber<char[]>.Climb(current, target, mutate, badGetError, copyFunc, print); //  74450 iterations
            HillClimber<char[]>.Climb(current, target, mutate, smartGetError, copyFunc, print); //  2289
        }
    }
}