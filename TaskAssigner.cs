using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAssigner
{
    #region Methods [JWHW - 14/04]

    #region Constructor [JWHW - 14/04]
    //Singleton pattern constructor - ensures that only one instance of TaskAssigner can exist
    private static TaskAssigner taInstance;
    private static readonly object locker = new object();
    private TaskAssigner() { }
    public static TaskAssigner TAInstance
    {
        get
        {
            if (taInstance == null)
            {
                lock (locker)
                {
                    if (taInstance == null)
                    {
                        taInstance = new TaskAssigner();
                    }
                }
            }
            return taInstance;
        }
    }
    #endregion

    /// <summary>Assigns a list of tasks to every colonist in a list</summary>
    /// <param name="colonists">The list of colonists</param>
    /// <param name="taskList">The list of tasks</param>
    public void AssignToAll(List<Colonist> colonists, Task taskList)
    {
        //Assigns the same list of tasks to each colonist
        foreach (Colonist c in colonists)
        {
            //for (int i = 0; i < taskList.Count; i++)
            //{
            //    Debug.Log("assigner: " + taskList[i].ReturnType().ToString());
            //    c.taskQueue.Enqueue(taskList[i]);                
            //}
            c.taskQueue.Enqueue(taskList);
            c.ColonistAct();
        }
    }

    /// <summary>Assigns a list of unique tasks to all the colonists in a list. An example of this is move tasks, where every colonist will be given a unique offset position</summary>
    /// <param name="colonists">The list of colonists</param>
    /// <param name="uniqueTaskList">The list of unique tasks</param>
    public void AssignUnique(List<Colonist> colonists, List<Task> uniqueTaskList)
    {
        //Enqueue a different task to each colonist. This might be part of AssignToAll, but keeping it seperate now while the system is being reworked
        if(colonists.Count != uniqueTaskList.Count)
        {
            //Make sure the lists are the same length
            Debug.Log("TASKASSIGNER: MISMATCH: colonist count, unique task list count");
        }
        else
        {
            for (int i = 0; i < colonists.Count; i++)
            {
                //go through each list together, and enqueue them in order - this only works if the lists are the same length
                colonists[i].taskQueue.Enqueue(uniqueTaskList[i]);
                
            }
            foreach (Colonist c in colonists)
            {
                c.ColonistAct();
            }
        }
    }

    /// <summary>Assigns the fetch task to the designated fetcher</summary>
    /// <param name="fetcher">The designated fetcher</param>
    /// <param name="fetchTask">The fetch task</param>
    public void AssignFetcher(Colonist fetcher, Task fetchTask)
    {
        //assign the fetch task to the designated fetcher
        fetcher.taskQueue.Enqueue(fetchTask);
        fetcher.ColonistAct();
    }

    #endregion

    #region LEGACY CODE [JWHW - Task System restructure on 14/04]
    ///// <summary>
    ///// Assigns a list of tasks to the task queue of any number of colonists
    ///// </summary>
    ///// <param name="colonists">A List of colonists to assign a task to.</param>
    ///// <param name="tasks">A List of tasks to assign to any number of colonists.</param>
    //public void Assign(List<Colonist> colonists, List<Task> tasks)
    //{
    //    //When a task is assigned, set first colonist to be the fetcher, which can be used to get items for construction
    //    colonists[0].isTeamFetcher = true;
    //    foreach (Colonist c in colonists)
    //    {
    //        for (int i = 0; i < tasks.Count; i++)
    //        {
    //            c.taskQueue.Enqueue(tasks[i]);
    //        }
    //    }
    //}

    ///// <summary>Assigns one colonist the role of "fetcher", giving them a unique Fetch Task, and then the list of tasks to all of the colonists</summary>
    ///// <param name="colonists">The list of colonists</param>
    ///// <param name="fetcherTasks">The specific task to assign only to the fetcher</param>
    ///// <param name="taskList">The list of tasks to assign to all colonists</param>
    //public void AssignFetcher(List<Colonist> colonists, List<Task> fetcherTasks, List<Task> taskList)
    //{
    //    //Assign the fetcherTasks to Colonist[0]
    //    for (int i = 0; i < fetcherTasks.Count; i++)
    //    {
    //        colonists[0].taskQueue.Enqueue(fetcherTasks[i]);
    //    }
    //    //Then call AssignToAll which will enqueue the taskList to every colonist
    //    AssignToAll(colonists, taskList);
    //}
    #endregion
}
