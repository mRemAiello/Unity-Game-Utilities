using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class CommandInputPair
    {
        [SerializeField] private Command _command;
        [SerializeField] private CommandInput _commandInput;

        public CommandInputPair(Command command, CommandInput commandInput)
        {
            _command = command;
            _commandInput = commandInput;
        }

        //
        public Command Command => _command;
        public CommandInput CommandInput => _commandInput;
    }
}