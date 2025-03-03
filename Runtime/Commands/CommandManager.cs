using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class CommandManager : Singleton<CommandManager>
    {
        [Tab("Variables")]
        [SerializeField] private bool _enableLog = false;
        [SerializeField, ShowIf("_enableLog", true)] private string _logCategory = "Command";
        [SerializeField] private int _maxCommandsInLogList = 20;

        [Tab("Debug")]
        [ReadOnly] private Command _currentCommand = null;
        [ReadOnly] private List<CommandTuple> _commandQueue = new();
        [ReadOnly] private List<string> _commandList = new();
        [ReadOnly] private bool _playingQueue = false;

        //
        public bool IsCommandPlaying => _playingQueue;

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
            if (_enableLog)
            {
                var debugger = DebugManager.Instance.GetCategory(_logCategory);
                debugger.Log("Command " + commandTuple.Item1.name + " added to Queue");
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
                var debugger = DebugManager.Instance.GetCategory(_logCategory);
                debugger.Log("Executing Command " + command.Item1.name);
            }

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