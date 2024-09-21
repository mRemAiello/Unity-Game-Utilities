using System;

namespace GameUtils
{
    [Serializable]
    public class CommandTuple : ElementTuple<Command, ICommandInput>
    {
        public CommandTuple(Command firstData, ICommandInput secondData) : base(firstData, secondData)
        {
        }
    }
}