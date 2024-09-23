using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class CommandManager : Singleton<CommandManager>
    {
        [Tab("Variables")]
        [SerializeField] private bool _enableLog = false;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private Command _currentCommand;
        [SerializeField, ReadOnly] private List<CommandTuple> _commandQueue = new();
        [SerializeField, ReadOnly] private List<string> _commandList = new();
        [SerializeField, ReadOnly] private bool _playingQueue = false;

        //
        private DebugCategory _commandLogger;

        //
        public bool IsCommandPlaying => _playingQueue;

        protected override void OnPostAwake()
        {
            _commandLogger = DebugManager.Instance.GetCategory("Command");
        }

        public void AddToQueue(CommandTuple command)
        {
            //
            _commandQueue.Add(command);
            
            //
            if (_enableLog)
            {
                _commandLogger.Log("Command " + command.Item1.name +  " added to Queue");
            }

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
            if (_enableLog)
            {
                _commandLogger.Log("Executing Command " + command.Item1.name);
            }

            //
            _commandList.Add(command.Item1.name);
            command.Item1.Execute(command.Item2);
        }
    }
}