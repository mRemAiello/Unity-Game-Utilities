using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class CommandManager : Singleton<CommandManager>
    {
        [Tab("Debug")]
        [SerializeField, ReadOnly] private List<CommandTuple> _commandQueue = new();
        [SerializeField, ReadOnly] private bool _playingQueue = false;

        //
        public bool IsCommandPlaying => _playingQueue;

        public void AddToQueue(CommandTuple command)
        {
            _commandQueue.Add(command);
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
            }
        }

        private void PlayFirstCommandFromQueue()
        {
            _playingQueue = true;

            //
            var command = _commandQueue.Pop(0);
            command.Item1.Execute(command.Item2);
        }
    }
}