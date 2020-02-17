using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBuilder
{
    #region Variable Declarations [JWHW - 14/04]
    /// <summary>Use this vector as a dummy since "null" cannot be used for a vector argument. This won't be used when the task is processed</summary>
    private Vector3 noVector = new Vector3(0f, 0f, 0f);
    #endregion

    #region Methods [JWHW - 14/04]

    #region Constructor [JWHW - 14/04]
    //Singleton pattern constructor - ensures that only one instance of TaskBuilder can exist
    private static TaskBuilder tbInstance;
    private static readonly object locker = new object();
    private TaskBuilder() { }
    public static TaskBuilder TBInstance
    {
        get
        {
            if (tbInstance == null)
            {
                lock (locker)
                {
                    if (tbInstance == null)
                    {
                        tbInstance = new TaskBuilder();
                    }
                }
            }
            return tbInstance;
        }
    }
    #endregion

    /// <summary>Creates the move task to assign to colonists</summary>
    /// <param name="destination">The location to move to</param>
    /// <returns>A list, length 1, that includes the move task</returns>
    public List<Task> CreateMoveTask(Vector3 destination)
    {       
        //Move tasks only require a destination
        List<Task> mList = new List<Task>();
        Task mTask = new Task(TaskType.Move, destination, null, null, null);
        mList.Add(mTask);      
        return mList;
    }

    /// <summary>Creates the harvest task to assign to colonists</summary>
    /// <param name="node">The resource node to be harvested</param>
    /// <param name="bM">Reference to the Building Manager - required for creating a storage task</param>
    /// <returns>A List of tasks for Harvesting (movement, harvesting, depositing)</returns>
    public Task CreateHarvestTask(ResourceEnvironment node, BuildingManager bM)
    {
        ////Will include creating a move task, using the node.harvestHotspot.transform.position
        //List<Task> hList = new List<Task>();
        ////First create the move task to get to the node
        ////Task move = new Task(TaskType.Move, node.harvestHotspot.transform.position, null, node, null); (hard code for doing this)
        //List<Task> m = CreateMoveTask(node.harvestHotspot.transform.position); //can just call the CreateMoveTask method instead
        //Debug.Log("Harvest Location to move to: " + node.harvestHotspot.transform.position);
        //hList.AddRange(m);
        ////Second create the harvest task
        //Task harvest = new Task(TaskType.Harvest, noVector, null, node, null);
        //hList.Add(harvest);
        ////Lastly create the storage task [DEPOSIT]
        //List<Task> s = CreateStorageTask(bM, TaskType.Deposit);
        //hList.AddRange(s);
        //return hList; 

        //TEMP HARVEST TASK IGNORING MOVEMENT COMPONENT
        Task h;
        h = new Task(TaskType.Harvest, node.harvestHotspot.transform.position, null, node, null);
        return h;
        
    }

    /// <summary>Creates the garrison task, which varies depending on the TaskType</summary>
    /// <param name="building">The building to garrison the colonist in to</param>
    /// <param name="type">The type of task - decides which WOFunction to assign the colonist to</param>
    /// <param name="bM">Reference to the building manager - used for fetching tasks</param>
    /// <returns></returns>
    public List<Task> CreateGarrisonTask(BuildingObject building, TaskType type, BuildingManager bM)
    {
        List<Task> bList = new List<Task>();
        //move to the building's entrance
        List<Task> m = CreateMoveTask(building.doorHotspot.transform.position);
        bList.AddRange(m);
        //Some garrison tasks require extra steps, most commonly the fetching of materials **Handled in TaskManager**        
        switch (type)
        {
            case TaskType.Deposit:
                Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask using an invalid TaskType");
                break;
            case TaskType.Fetch:
                Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask using an invalid TaskType");
                break;
            case TaskType.Construct:
                //all buildings
                Task construct = new Task(TaskType.Construct, noVector, building, null, bM);
                bList.Add(construct);
                break;
            case TaskType.Demolish:
                //all buildings
                Task demolish = new Task(TaskType.Demolish, noVector, building, null, bM);
                bList.Add(demolish);
                break;
            case TaskType.Harvest:
                Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask using an invalid TaskType");
                break;
            case TaskType.Health:
                //parse as health building
                if (building.GetType().IsSubclassOf(typeof(HealthBuilding)))
                {
                    Task health = new Task(TaskType.Health, noVector, building, null, bM);
                    bList.Add(health);
                    break;
                }
                else
                {
                    Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask: Not a health building");
                    break;
                }
            case TaskType.Housing:
                //parse as housing building
                if (building.GetType().IsSubclassOf(typeof(HousingBuilding)))
                {
                    Task housing = new Task(TaskType.Housing, noVector, building, null, bM);
                    bList.Add(housing);
                    break;
                }
                else
                {
                    Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask: Not a house building");
                    break;
                }
            case TaskType.Idle:
                Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask using an invalid TaskType");
                break;
            case TaskType.Maintain:
                //all buildings
                Task maintain = new Task(TaskType.Maintain, noVector, building, null, bM);
                bList.Add(maintain);
                break;
            case TaskType.Mood:
                //parse as mood building
                if (building.GetType().IsSubclassOf(typeof(MoodBuilding)))
                {
                    Task mood = new Task(TaskType.Mood, noVector, building, null, bM);
                    bList.Add(mood);
                    break;
                }
                else
                {
                    Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask: Not a mood building");
                    break;
                }
            case TaskType.Move:
                Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask using an invalid TaskType");
                break;
            case TaskType.Production:
                //parse as production building
                if (building.GetType().IsSubclassOf(typeof(ProductionBuilding)))
                {
                    Task production = new Task(TaskType.Production, noVector, building, null, bM);
                    bList.Add(production);
                    break;
                }
                else
                {
                    Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask: Not a Production building");
                    break;
                }
            case TaskType.Refine:
                //parse as refine building
                if (building.GetType().IsSubclassOf(typeof(RefiningBuilding)))
                {
                    Task refine = new Task(TaskType.Refine, noVector, building, null, bM);
                    bList.Add(refine);
                    break;
                }
                else
                {
                    Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask: Not a refining building");
                    break;
                }
            case TaskType.Repair:
                //all buildings
                Task repair = new Task(TaskType.Repair, noVector, building, null, bM);
                bList.Add(repair);
                break;
            case TaskType.Wait:
                Debug.Log("TASKBUILDER: MISMATCH: CreateGarrisonTask using an invalid TaskType");
                break;
        }
        return bList;
    }

    /// <summary>Creates a task to interact with Storage Units - either fetch items from storage units, or dump the colonists inventory into the storage unit</summary>
    /// <param name="bM">Reference to the BuildingManager - used to access the list of storage buildings at the time the task is performed (not generated)</param>
    /// <param name="type">The type of task - does nothing unless Fetch or Deposit</param>
    /// <returns></returns>
    public List<Task> CreateStorageTask(BuildingManager bM, TaskType type)
    {
        List<Task> sList = new List<Task>();        
        switch (type)
        {
            case TaskType.Deposit:
                Task deposit = new Task(TaskType.Deposit, noVector, null, null, bM);
                sList.Add(deposit);
                break;
            case TaskType.Fetch:
                Task fetch = new Task(TaskType.Fetch, noVector, null, null, bM); //**TODO** Cleanup here, this might be inaccurate, but also I don't know if this is needed.
                sList.Add(fetch);
                break;
            case TaskType.Construct:
            case TaskType.Demolish:
            case TaskType.Harvest:
            case TaskType.Health:
            case TaskType.Housing:
            case TaskType.Idle:
            case TaskType.Maintain:
            case TaskType.Mood:
            case TaskType.Move:
            case TaskType.Production:
            case TaskType.Refine:
            case TaskType.Repair:
            case TaskType.Wait:
                //these types shouldn't be here
                Debug.Log("TASKBUILDER: MISMATCH: CreateStorageTask using an invalid TaskType");
                break;
        }
        return sList;
    }

    #endregion

    #region LEGACY CODE [JWHW - Task System restructure on 14/04]
    ///// <summary> Use this vector as a dummy since "null" cannot be used for a vector argument. This won't be used when the task is processed.</summary>
    //private Vector3 noVector = new Vector3(0, 0, 0);

    ///// <summary>Generates a List of tasks that will move the colonist to a destination. This needs to be a list to parse into the Assign method.</summary>
    ///// <param name="destination">The destination to move to.</param>
    ///// <returns>Returns a list of individual tasks that in order complete a Move task.</returns>
    //public List<Task> CreateMoveTask(Vector3 destination)
    //{
    //    //Generate offsets for when multiple units are given this command at the same time **work out something about assigning to multiple colonists**

    //    List<Task> mList = new List<Task>();
    //    //Create a move task.
    //    Task move = new Task(TaskType.Move, destination, null, null);
    //    mList.Add(move);

    //    return mList;
    //}

    ///// <summary>Generates a list of tasks that will enable the colonist to harvest a resource node.</summary>
    ///// <param name="destination">The destination to move to - in this case, the harvest spots around the node.</param>
    ///// <param name="node">The node to be harvested.</param>
    ///// <returns>Returns a list of individual tasks that in order complete a Harvest task.</returns>
    //public List<Task> CreateHarvestTask(Vector3 destination, ResourceEnvironment node)
    //{
    //    List<Task> hList = new List<Task>();
    //    //Move to the node's hotspot
    //    Task moveToNode = new Task(TaskType.Move, destination, null, null);
    //    hList.Add(moveToNode);

    //    //Interact with the node
    //    Task harvestNode = new Task(TaskType.Harvest, noVector, null, node);
    //    hList.Add(harvestNode);

    //    //Find the closest storage unit, and go there **THIS IS NOW HANDLED IN THE TASK ITSELF, SINCE IT NEEDS A COLONIST REFERENCE** (also **TODO** make this work properly)

    //    return hList;
    //}

    ///// <summary>Generates a list of tasks that will enable the colonist to interact with a building.</summary>
    ///// <param name="destination">The destination to be moved to - in this case, the doorway to the building</param>
    ///// <param name="building">The building to enter</param>
    ///// <param name="typeOfBuildingInteract">The type of interaction the colonist(s) will be doing in the building.</param>
    ///// <returns>Returns a list of individual tasks that in order will garrison a unit in a building</returns>
    //public List<Task> CreateGarrisonTask(Vector3 destination, BuildingObject building, TaskType typeOfBuildingInteract)
    //{
    //    List<Task> bList = new List<Task>();
    //    //Move to the building's entrance
    //    Task moveToBuilding = new Task(TaskType.Move, destination, null, null);
    //    bList.Add(moveToBuilding);

    //    //Interact with the building
    //    Task enterBuilding = new Task(typeOfBuildingInteract, noVector, building, null);
    //    bList.Add(enterBuilding);

    //    return bList;
    //}
    #endregion
}
