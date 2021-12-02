using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Util
{
    public class StateMachine<T1, T2>
    {
        
        public T1 State { get; private set; }

        private Func<T1, T2, T1> StepFunction { get; }
        
        public StateMachine(T1 initialValue, Func<T1, T2, T1> stepFunction)
        {
            State = initialValue;
            StepFunction = stepFunction;
        }

        public void Apply(T2 command)
        {
            State = StepFunction(State, command);
        }

        public void Apply(IEnumerable<T2> commands)
        {
            foreach (var cmd in commands)
            {
                Apply(cmd);
            }   
        }
        
    }
}