using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class CommandManager : Singleton<CommandManager>, ILoggable
    {
        [Tab("Variables")]
        [SerializeField] private bool _logEnabled = false;
        [SerializeField] private int _maxCommandsInLogList = 20;

        [Tab("Debug")]
        [ReadOnly] private Command _currentCommand = null;
        [ReadOnly] private List<CommandTuple> _commandQueue = new();
        [ReadOnly] private List<string> _commandList = new();
        [ReadOnly] private bool _playingQueue = false;

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
            CommandTuple commandTuple = new(command, commandInput);
            _commandQueue.Add(commandTuple);

            //
            this.Log($"Command {commandTuple.Item1.name} added to Queue");

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
            _currentCommand = command.Item1;

            //
            this.Log($"Executing Command {command.Item1.name} from Queue");

            //
            _commandList.Add(command.Item1.name);
            if (_commandList.Count > _maxCommandsInLogList)
            {
                _commandList.RemoveAt(0);
            }

            //
            command.Item1.Execute(command.Item2);
        }
    }
}