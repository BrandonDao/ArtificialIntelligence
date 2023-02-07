using System;

namespace HillClimber
{
    internal class Program
    {
        class StringHillClimber
        {
            private static readonly Random RNG = new(32);

            private static void Mutate(char[] chars)
            {
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
            }

            private static double GetError(char[] current, char[] target)
            {
                double correct = 0;

                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i] == target[i]) correct++;
                }

                return Math.Abs(target.Length - correct) / target.Length;
            }

            public static void VisualizeThing(string targetString)
            {
                char[] target = targetString.ToCharArray();
                char[] current = new char[target.Length];

                for (int i = 0; i < target.Length; i++)
                {
                    current[i] = (char)(RNG.Next(32, 127));
                }



                double error = GetError(current, target);
                double previousError = error;
                char[] previous = new char[current.Length];

                while (error != 0)
                {
                    //Thread.Sleep(1);
                    Console.Clear();

                    for (int i = 0; i < current.Length; i++)
                    {
                        previous[i] = current[i];
                    }

                    Mutate(current);
                    error = GetError(current, target);

                    if(error > previousError)
                    {
                        for (int i = 0; i < current.Length; i++)
                        {
                            current[i] = previous[i];
                        }
                    }
                    else
                    {
                        previousError = error;
                    }

                    for (int i = 0; i < current.Length; i++)
                    {
                        Console.Write(current[i]);
                    }
                    Console.Write($" ({100 - (int)(GetError(current, target) * 100)}%)");
                }
            }
        }

        static void Main(string[] args)
        {
            StringHillClimber.VisualizeThing("Hello World!");
        }
    }
}