using System;
using System.Collections.Generic;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day2
{
    public class Day2Solver : ISolvable
    {

        private static string FileName => "Input/Day2_A.input";

        private record Command(char Cmd, int Dist);

        private static List<Command> ReadCommands()
        {
            return FileRead.ReadLinesAndConvert(FileName,
                tokens => new Command(tokens[0][0], int.Parse(tokens[1])));
        }

        private record State(long Aim, long Depth, long Forwards);

        private static State StepFunctionStar1(State prev, Command curr)
        {
            return curr.Cmd switch
            {
                'f' => prev with { Forwards = prev.Forwards + curr.Dist },
                'd' => prev with { Depth = prev.Depth + curr.Dist },
                'u' => prev with { Depth = prev.Depth - curr.Dist }
            };
        }

        private static State StepFunctionStar2(State prev, Command curr)
        {
            return curr.Cmd switch
            {
                'f' => prev with
                {
                    Forwards = prev.Forwards + curr.Dist,
                    Depth = prev.Depth + prev.Aim * curr.Dist
                },
                'd' => prev with { Aim = prev.Aim + curr.Dist },
                'u' => prev with { Aim = prev.Aim - curr.Dist }
            };
        }

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private long SolveForStepFunction(Func<State, Command, State> func)
        {
            var initialState = new State(0, 0, 0);
            var machine = new StateMachine<State, Command>(initialState, func);

            var input = ReadCommands();
            machine.Apply(input);
            var depth = machine.State.Depth;
            var forwards = machine.State.Forwards;

            return depth * forwards;
        }

        private void SolveFirstStar()
        {
            var result = SolveForStepFunction(StepFunctionStar1);
            Console.WriteLine("Solution (1): " + result);
        }

        private void SolveSecondStar()
        {

            var result = SolveForStepFunction(StepFunctionStar2);
            Console.WriteLine("Solution (2): " + result);
        }

    }
}
