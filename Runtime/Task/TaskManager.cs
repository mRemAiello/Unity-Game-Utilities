using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class TaskManager : Singleton<TaskManager>
    {
        [SerializeField] private List<ITask> _tasks = new();
        [SerializeField] private bool _enableLog = false;

        [ShowIf(nameof(_enableLog), true)]
        [SerializeField] private string _logCategory;
        [EndIf]

        // Get
        public List<ITask> Tasks => _tasks;

        public void Execute(int index, object context, object data)
        {
            var stopwatch = new Stopwatch();
            if (index < Tasks.Count)
            {
                stopwatch.Start();
                Tasks[index].Execute(ref context, ref data);
                stopwatch.Stop();

                Tasks[index].TimeElapsed = stopwatch.ElapsedMilliseconds;
            }
        }

        public void ShowElapsedTimeForTasks()
        {
            if (Tasks == null || Tasks.Count == 0)
                return;

            //
            foreach (ITask task in Tasks)
            {
                if (_enableLog)
                {
                    var message = string.Format("Task {0} completed in {1}", task.ShortName, task.TimeElapsed);
                    DebugManager.Instance.GetCategory(_logCategory).Log(message);
                }
            }
        }
    }
}