using System;

namespace GameUtils
{
    [Serializable]
    public class CommandTuple : ElementTuple<Command, CommandInput>
    {
        public CommandTuple(Command firstData, CommandInput secondData) : base(firstData, secondData)
        {
        }

        //
        public Command Command => Item1;
        public CommandInput CommandInput => Item2;
    }
}