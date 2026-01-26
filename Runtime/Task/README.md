# Task Utilities

## Overview
The Task utilities provide a lightweight way to define and run time-measured tasks in Unity. Tasks are represented by the `ITask` interface and executed by the `TaskManager`, which tracks execution time and can log results.

## Core Types
- **`ITask`**: Defines the task contract (`Execute`, `ShortName`, `TimeElapsed`).
- **`TaskContext`**: Base class for shared execution context objects.
- **`TaskData`**: Base class for task-specific input data.
- **`TaskManager`**: Singleton that stores a list of tasks, executes them by index, and can log their elapsed time.

## How to Use
1. Create a class that implements `ITask`.
2. Optionally define a `TaskContext` and `TaskData` subclass to carry state and inputs.
3. Register task instances in `TaskManager` (via Inspector or code).
4. Call `TaskManager.Execute` with the task index, context, and data.
5. Use `ShowElapsedTimeForTasks` to log execution times.

## Example
```csharp
using UnityEngine;
using GameUtils;

// Example context that can store shared state.
public sealed class ExampleTaskContext : TaskContext
{
    // No class-level comments by project convention.
}

// Example data used as input to a task.
public sealed class ExampleTaskData : TaskData
{
    // No class-level comments by project convention.
}

// Example task implementation.
public sealed class ExampleTask : ITask
{
    // No class-level comments by project convention.

    public double TimeElapsed { get; set; }

    public string ShortName => "Example";

    public void Execute(ref object context, ref object data)
    {
        // Comment: cast the incoming context/data to your concrete types.
        var typedContext = context as ExampleTaskContext;
        var typedData = data as ExampleTaskData;

        // Comment: perform your task logic here.
        Debug.Log("Executing ExampleTask");

        // Comment: optionally update the passed-in context/data objects.
        context = typedContext;
        data = typedData;
    }
}

public sealed class ExampleTaskRunner : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;

    private void Start()
    {
        // Comment: create context and data instances for the task.
        var context = new ExampleTaskContext();
        var data = new ExampleTaskData();

        // Comment: execute the first task in the TaskManager list.
        _taskManager.Execute(0, context, data);

        // Comment: log the elapsed time for all tasks.
        _taskManager.ShowElapsedTimeForTasks();
    }
}
```

## Notes
- `Execute` uses `ref object` to keep the interface generic. Cast to your concrete `TaskContext`/`TaskData` types inside the task.
- `TaskManager` tracks elapsed time in milliseconds and stores it per task.
- Ensure your task list order in the `TaskManager` matches the index used in `Execute`.
