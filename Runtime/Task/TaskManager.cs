using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GameUtils
{
    public class TaskManager : Singleton<TaskManager>, ILoggable
    {
        [SerializeField] private List<ITask> _tasks = new();
        [SerializeField] private bool _logEnabled = false;

        // Get
        public List<ITask> Tasks => _tasks;
        public bool LogEnabled => _logEnabled;

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
                this.Log($"Task {task.ShortName} completed in {task.TimeElapsed}");
            }
        }
    }
}