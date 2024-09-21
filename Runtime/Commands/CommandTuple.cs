using System;

namespace GameUtils
{
    [Serializable]
    public class CommandTuple : ElementTuple<Command, CommandInput>
    {
        public CommandTuple(Command firstData, CommandInput secondData) : base(firstData, secondData)
        {
        }
    }
}