using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class CommandManager : MonoBehaviour
    {
        [Tab("Debug")]
        [SerializeField, ReadOnly] private List<Command> _commandQueue = new();
        [SerializeField, ReadOnly] private bool _playingQueue = false;

        //
        public bool IsCommandPlaying => _playingQueue;

        public void AddToQueue(Command command)
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
            _commandQueue.Pop(0).Execute();
        }
    }
}