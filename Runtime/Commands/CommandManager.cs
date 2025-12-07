using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(-50)]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class CommandManager : Singleton<CommandManager>, ILoggable
    {
        [SerializeField] private bool _logEnabled = false;
        [SerializeField] private int _maxCommandsInLogList = 20;

        //
        [SerializeField, ReadOnly, Group("debug")] private bool _playingQueue = false;
        [SerializeField, ReadOnly, Group("debug")] private Command _currentCommand = null;
        [SerializeField, ReadOnly, Group("debug")] private List<CommandInputPair> _commandQueue = new();
        [SerializeField, ReadOnly, Group("debug")] private List<string> _commandList = new();

        //
        public bool IsCommandPlaying => _playingQueue;
        public Command CurrentCommand => _currentCommand;
        public bool LogEnabled => _logEnabled;

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _currentCommand = null;
            _commandQueue = new();
            _commandList = new();
            _playingQueue = false;
        }

        public void AddToQueue(GameObject igniter, Command command, params object[] additionalSettings)
        {
            // Tuple
            CommandInput commandInput = new()
            {
                Igniter = igniter,
                AdditionalSettings = additionalSettings
            };
            CommandInputPair commandTuple = new(command, commandInput);
            _commandQueue.Add(commandTuple);

            //
            this.Log($"Command {commandTuple.Command.name} added to Queue");

            //
            if (!_playingQueue)
            {
                PlayFirstCommandFromQueue();
            }
        }

        public void CommandExecutionComplete()
        {
            if (_commandQueue.Count > 0)
            {
                PlayFirstCommandFromQueue();
            }
            else
            {
                _playingQueue = false;
                _currentCommand = null;
            }
        }

        private void PlayFirstCommandFromQueue()
        {
            _playingQueue = true;

            //
            var command = _commandQueue.Pop(0);
            _currentCommand = command.Command;

            //
            this.Log($"Executing Command {command.Command.name} from Queue");

            //
            _commandList.Add(command.Command.name);
            if (_commandList.Count > _maxCommandsInLogList)
            {
                _commandList.RemoveAt(0);
            }

            //
            command.Command.Execute(command.CommandInput);
        }
    }
}