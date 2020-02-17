using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType { Move, Harvest, Construct, Demolish, Health, Housing, Maintain, Mood, Production, Refine, Repair, Idle, Wait, Fetch, Deposit}

public class Task 
{
    #region Variable Declarations [JWHW - 14/04]
    /// <summary>The type of the Task</summary>
    TaskType type;

    /// <summary>The destination of the Task (movement)</summary>
    Vector3 destination;

    /// <summary>The destination building of the Task (garrison)</summary>
    BuildingObject targetBuilding;

    /// <summary>The destination node of the Task (harvest)</summary>
    ResourceEnvironment targetNode;

    /// <summary>Reference to the Building Manager - used to access the Building List, mostly for storage access</summary>
    BuildingManager buildingManager;

    /// <summary>This is only used for Fetch tasks. This will tell the system how to get the resourceReqs to fetch.</summary>
    TaskType fetchCont;
    /// <summary>
    /// For fetch tasks, the resources required to fetch
    /// </summary>
    ResourceRequirement resourceReqs;
    #endregion

    #region Methods [JWHW - 14/04]

    #region Return Methods
    //These methods might not be required, but they are here in case (likely for debugging).
    //Each one returns the specific variable from the task instance.
    public TaskType ReturnType() { return type; }
    public Vector3 ReturnDestination() { return destination; }
    public BuildingObject ReturnBuilding() { return targetBuilding; }
    public ResourceEnvironment ReturnNode() { return targetNode; }
    #endregion

    #region Constructor
    /// <summary>Constructor method for Task</summary>
    /// <param name="type">The type of the task</param>
    /// <param name="dest">The movement destination for the task (move)</param>
    /// <param name="b">The building for the task (garrison)</param>
    /// <param name="r">The resource node for the task (harvest)</param>
    public Task(TaskType t, Vector3 dest, BuildingObject b, ResourceEnvironment r, BuildingManager bM)
    {
        type = t;
        destination = dest;
        targetBuilding = b;
        targetNode = r;
        buildingManager = bM;
    }

    /// <summary>ONLY USE THIS FOR FETCH TASKS. Fetch tasks require different, more specific setup.</summary>
    /// <param name="fetchContext">The type of task this fetch will be used for</param>
    /// <param name="buildingToFetchFor">The building the materials are being used for</param>
    /// <param name="bM">Building manager reference</param>
    public Task(TaskType fetchContext, BuildingObject buildingToFetchFor, BuildingManager bM)
    {
        type = TaskType.Fetch;
        targetBuilding = buildingToFetchFor;
        buildingManager = bM;
        destination = new Vector3(0f, 0f, 0f);
        targetNode = null;
    }
    #endregion

    #region DoAction method overloads
    /// <summary>The DoAction method for Move tasks</summary>
    /// <param name="colonist">The colonist doing the Task</param>
    /// <param name="destination">The destination to move to</param>
    public void DoAction(Colonist colonist, Vector3 destination)
    {
        Debug.Log(destination);
        //Set the destination of the colonist's NavMeshAgent to be the desired location
        colonist.Agent.SetDestination(destination);
        //Ensure the navmeshagent is active (ie Stopped=False)
        colonist.Agent.isStopped = false;
        bool moving = true;
        //if (moving)
        //{            
        //    //animation for explorer walk is not working. Trigger is correct, but the animation is not playing.
        //    colonist.anim.ResetTrigger("ChopTrigger");
        //    colonist.anim.ResetTrigger("IdleTrigger");
        //    colonist.anim.SetTrigger("WalkTrigger");
        //}

        //Moving(colonist, destination, moving);
        Debug.Log("moving coroutine started");
        colonist.StartCoroutine(colonist.Moving(colonist, destination, moving));
        //if (!moving)
        //{           
        //    colonist.anim.ResetTrigger("WalkTrigger");
        //    colonist.anim.ResetTrigger("ChopTrigger");
        //    colonist.anim.SetTrigger("IdleTrigger");
        //}
        
        
    }    

    //public IEnumerator Moving(Colonist c, Vector3 destination, bool move)
    //{
    //    while (true)
    //    {
    //        while(move)
    //        {
    //            if (c.transform.position == destination)
    //            {
    //                move = false;
    //            }
    //            yield return null;
    //        }

    //        yield return null;
    //    }
    //}

    /// <summary>The DoAction method for Harvest tasks</summary>
    /// <param name="colonist">The colonist doing the Task</param>
    /// <param name="node">The node to be harvested</param>
    public void DoAction(Colonist colonist, ResourceEnvironment node, StorageBuilding store)
    {
        //This runs when the colonist is at the node's harvest hotspot
        //run the ProductionFunction of the node
        //node.ProductionFunction.ColonistReq.AddColonist(colonist);
        //bool harvesting = true;
        //colonist.StartCoroutine(colonist.Harvesting(colonist, harvesting, node, this, store));
        //harvesting = false;
        //Deposit(colonist, store);
        //This runs until the node is depleted (as far as I know)
        //after this is over, find the closest storage unit and determine the next node to harvest (closest node to the one just harvested? within a certain distance?)
        //go to the storage unit, and dump colonist's inventory into it - THIS IS HANDLED IN DEPOSIT METHOD
        //go to the next node, and repeat this process [ie enqueue a new MoveTask(newnode.harvestposition), HarvestTask(newnode)]
        //**TODO** sort out how to repeat task - LEAVE THIS, IT'S JUST GRAVY [as good as it'll be to get units to endlessly harvest, I don't have time for this]

        //TEMP HARVEST CODE
        Debug.Log("I'm harvesting, I promise!");
    }

    /// <summary>The DoAction method for Garrison tasks</summary>
    /// <param name="colonist">The colonist doing the Task</param>
    /// <param name="building">The building the task is performed at/in</param>
    /// <param name="type">The type of garrison task, used to select which object function to use</param>
    public void DoAction(Colonist colonist, BuildingObject building, TaskType type)
    {
        //This runs when the colonist is at the building's entrance
        //switch statement using TaskType
        //Depending on the tasktype, add the colonist to that specific function
        //For some tasks, the building must be checked and parsed into a child type - ie if TaskType is Health, targetBuilding must be checked to see if it's a HealthBuilding, and then we can put the colonist in that function.
        switch (type)
        {
            case TaskType.Construct:
                building.ConstructionFunction.ColonistReq.AddColonist(colonist);
                break;
            case TaskType.Demolish:
                //Debug.Log(colonist.gameObject.name);
                //Debug.Log(building.gameObject.name);
                building.DemolishFunction.ColonistReq.AddColonist(colonist);
                break;
            case TaskType.Health:
                if(building.GetType().IsSubclassOf(typeof(HealthBuilding)))
                {
                    HealthBuilding hB = (HealthBuilding)building;
                    hB.HealthFunction.ColonistReq.AddColonist(colonist);
                    break;
                }
                else
                {
                    Debug.Log("TASK: PARSING ERROR: Health Task requires a HealthBuilding");
                    break;
                }
            case TaskType.Housing:
                if (building.GetType().IsSubclassOf(typeof(HousingBuilding)))
                {
                    HousingBuilding hB = (HousingBuilding)building;
                    hB.HousingFunction.ColonistReq.AddColonist(colonist);
                    break;
                }
                else
                {
                    Debug.Log("TASK: PARSING ERROR: House Task requires a HousingBuilding");
                    break;
                }
            case TaskType.Maintain:
                building.MaintenanceFunction.ColonistReq.AddColonist(colonist);
                break;
            case TaskType.Mood:
                if (building.GetType().IsSubclassOf(typeof(MoodBuilding)))
                {
                    MoodBuilding mB = (MoodBuilding)building;
                    mB.MoodFunction.ColonistReq.AddColonist(colonist);
                    break;
                }
                else
                {
                    Debug.Log("TASK: PARSING ERROR: Mood Task requires a MoodBuilding");
                    break;
                }
            case TaskType.Production:
                if (building.GetType().IsSubclassOf(typeof(ProductionBuilding)))
                {
                    ProductionBuilding pB = (ProductionBuilding)building;
                    pB.ProductionFunction.ColonistReq.AddColonist(colonist);
                    break;
                }
                else
                {
                    Debug.Log("TASK: PARSING ERROR: Production Task requires a ProductionBuilding");
                    break;
                }
            case TaskType.Refine:
                if (building.GetType().IsSubclassOf(typeof(RefiningBuilding)))
                {
                    RefiningBuilding rB = (RefiningBuilding)building;
                    rB.RefineFunctions.ColonistReq.AddColonist(colonist); //I can imagine errors cropping up here - If there are multiple refine functions I might need classification here for that - but that isn't my job to sort out at this moment in time.
                    break;
                }
                else
                {
                    Debug.Log("TASK: PARSING ERROR: House Task requires a HousingBuilding");
                    break;
                }                
            case TaskType.Repair:
                building.RepairFunction.ColonistReq.AddColonist(colonist);
                break;
            case TaskType.Move:
            case TaskType.Harvest:
            case TaskType.Deposit:
            case TaskType.Fetch:
            case TaskType.Idle:
            case TaskType.Wait:
                Debug.Log("TASK: MISMATCH: Attempting to do a Garrison Task using incorrect TaskType");
                break;
        }        
    }
    #endregion

    #region Deposit + Fetch methods
    /// <summary>The method for depositing items in a storage unit</summary>
    /// <param name="colonist">The colonist going to storage</param>
    public void Deposit(Colonist colonist, StorageBuilding store)
    {
        Debug.Log("deposit called");
        List<StorageBuilding> ignore = new List<StorageBuilding>();
        //get the closest storage unit        
        //Debug.Log("TASK.CS - building manager name: " + buildMAN.gameObject.name);
        StorageBuilding closestStorage = store;
        //send the colonist there
        colonist.Agent.SetDestination(closestStorage.doorHotspot.transform.position);
        #region Inventory Dump - DK
        //When colonist arrives here, deposit inventory
        colonist.Inventory().TransferAll(closestStorage.Inventory());
        #endregion
    }

    /// <summary>The method for fetching items from storage buildings</summary>
    /// <param name="colonist">The colonist doing the fetching</param>
    /// <param name="ignoreList">The list of storage buildings that have been visited - when called from outside of the Fetch method, this should be an empty list</param>
    /// <param name="building">The building this fetch task is relevant to</param>
    public void Fetch(Colonist colonist, List<StorageBuilding> ignoreList, BuildingObject building)
    {
        //Get a list of the resources required to fetch  
        
        List<ResourceAmount> resourceAmounts = new List<ResourceAmount>();
        bool firstInstance = true;
        if (firstInstance)
        {
            resourceReqs = null;
            switch (fetchCont)
            {
                case TaskType.Construct:
                    resourceReqs = targetBuilding.ConstructionFunction.ResourceReq;
                    resourceAmounts = resourceReqs.AllInputs();
                    firstInstance = false;
                    break;
                case TaskType.Production:
                    if (targetBuilding.GetType().IsSubclassOf(typeof(ProductionBuilding)))
                    {
                        ProductionBuilding pB = (ProductionBuilding)targetBuilding;
                        resourceReqs = pB.ProductionFunction.ResourceReq;
                        resourceAmounts = resourceReqs.AllInputs();
                        firstInstance = false;
                        break;
                    }
                    else
                    {
                        Debug.Log("TASK: MISMATCH: Production task given to a non-production building. Required for resourceReq for production function");
                        break;
                    }
                case TaskType.Refine:
                    if (targetBuilding.GetType().IsSubclassOf(typeof(RefiningBuilding)))
                    {
                        RefiningBuilding rB = (RefiningBuilding)targetBuilding;
                        resourceReqs = rB.RefineFunctions.ResourceReq;
                        resourceAmounts = resourceReqs.AllInputs();
                        firstInstance = false;
                        break;
                    }
                    else
                    {
                        Debug.Log("TASK: MISMATCH: Refine task given to a non-refining building. Required for resourceReq for refine function");
                        break;
                    }
                case TaskType.Repair:
                    resourceReqs = targetBuilding.RepairFunction.ResourceReq;
                    resourceAmounts = resourceReqs.AllInputs();
                    firstInstance = false;
                    break;
                case TaskType.Maintain:
                case TaskType.Deposit:
                case TaskType.Fetch:
                case TaskType.Harvest:
                case TaskType.Idle:
                case TaskType.Wait:
                case TaskType.Move:
                case TaskType.Demolish:
                case TaskType.Health:
                case TaskType.Housing:
                case TaskType.Mood:
                    Debug.Log("TASK: MISMATCH: Incorrect TaskType for FetchContext");
                    break;
            }
        }
        
        //Get the closest storage unit
        StorageBuilding storage = buildingManager.GetClosestStorageBuilding(colonist.transform.position, ignoreList);
        Debug.Log(storage.gameObject.name);
        #region Logic for Storage trip - DK
        //go there
        colonist.Agent.SetDestination(storage.doorHotspot.transform.position);
        //withdraw the items that are required
        for (int i = 0; i < resourceAmounts.Count; i++)
        {
            if (storage.Inventory().IsResourceInInventory(resourceAmounts[i].Resource))
            {
                if (storage.Inventory().HasAmountOfResource(resourceAmounts[i].Resource, resourceAmounts[i].Amount))
                {
                    colonist.Inventory().ReceiveResources(resourceAmounts[i]);
                    storage.Inventory().ReduceResource(resourceAmounts[i]);
                }
                else
                {
                    ResourceAmount actualAmountInStorage = storage.Inventory().GetInventorySlot(resourceAmounts[i].Resource);
                    colonist.Inventory().ReceiveResources(actualAmountInStorage);
                    storage.Inventory().ReduceResource(actualAmountInStorage);
                }
            }
        }
        //check if requirement is fulfilled
        bool fulfilled = true;
        for (int j = 0; j < resourceReqs.AllInputs().Count; j++)
        {
            if (!colonist.Inventory().HasAmountOfResource(resourceReqs.AllInputs()[j].Resource, resourceReqs.AllInputs()[j].Amount))
            {
                fulfilled = false;                
            }
;        }

        //if yes - task complete
        if (!fulfilled)
        {
            //update resources required
            for (int k = 0; k < resourceAmounts.Count; k++)
            {
                resourceAmounts[k].ChangeAmount(resourceAmounts[k].Amount - colonist.Inventory().GetInventorySlot(resourceAmounts[k].Resource).Amount);
            }
            //if no - add current storage unit to ignore list, and rerun Fetch with the list to ignore
            ignoreList.Add(storage);
            if (ignoreList.Count == buildingManager.storageUnits.Count)
            {
                Debug.Log("You've run out of resources in storage units you melt. Go get more resources.");                
            }
            else
            {
                Fetch(colonist, ignoreList, targetBuilding);
            }            
        }
        
       
    }
    #endregion
    #endregion
    #endregion

    #region LEGACY CODE [JWHW - Task System restructure on 14/04]
    ////**TODO** Encapsulate these variables ;)
    ///// <summary> the type of task </summary>
    //public TaskType type;
    ///// <summary> destination to move to, either an empty space or the hotspot tied to the location </summary>    
    //public Vector3 destination;
    ///// <summary> target building to interact with </summary>
    //public BuildingObject targetBuilding;
    ///// <summary> target resource node to harvest </summary>
    //public ResourceEnvironment targetResourceNode;

    //public Task(TaskType t, Vector3 d, BuildingObject bO, ResourceEnvironment rO)
    //{
    //    type = t;
    //    destination = d;
    //    targetBuilding = bO;
    //    targetResourceNode = rO;
    //}

    ///// <summary>The DoAction method for moving</summary>
    ///// <param name="colonist">The colonist doing the action</param>
    ///// <param name="destination">The destination to move to</param>
    //public void DoAction(Colonist colonist, Vector3 destination)
    //{
    //    //Set the agents destination to the desired one
    //    colonist.Agent.SetDestination(destination);
    //    //Start the nav mesh agent
    //    colonist.Agent.isStopped = false;
    //}

    ///// <summary>
    ///// The DoAction method for harvesting (or otherwise interacting with a resource node, but for now harvesting is the only option)
    ///// </summary>
    ///// <param name="colonist">The colonist doing the action</param>
    ///// <param name="targetNode">the node to harvest</param>
    //public void DoAction(Colonist colonist, ResourceEnvironment targetNode)
    //{
    //    //run the targetNode's harvest function, adding the colonist
    //    targetNode.ProductionFunction.ColonistReq.AddColonist(colonist);

    //    // WHEN THE INVENTORY IS FULL, DO THIS:
    //    #region Trip to Storage
    //    //move to a storage unit and store some stuff ??
    //    //1. Locate storage unit to go to
    //    //have a manager somewhere that stores a list of storage buildings which is updated whenever a new one is placed, and here just access that list **TODO**
    //    StorageBuilding[] storageBuildings = Object.FindObjectsOfType<StorageBuilding>(); 
    //    //2. Move to the unit
    //    //manager can also have the methods for creating the ordered list, and the closest ones (the methods from the bottom of this class) **TODO**
    //    StorageBuilding storage = GetClosestStorage(storageBuildings, colonist); 
    //    // **TODO** CHANGE THIS TO GET THE ENTRANCE OF THE BUILDING :)
    //    Vector3 storageLocation = storage.transform.position; 
    //    DoAction(colonist, storageLocation);
    //    //3. Put the shit in the box **TODO** [needs dump inventory method]
    //    #endregion
    //    //repeat the harvesting - enqueue the same task to the colonist's task queue **TODO** [find closest resource to targetNode.position [within x distance?], since ProductionFunction runs until node is depleted] make sure it isn't null, add visibility check to make sure you can see/access the next node
    //}

    ///// <summary>
    ///// The DoAction method for interacting with buildings
    ///// </summary>
    ///// <param name="colonist">The colonist doing the action</param>
    ///// <param name="building">The building to interact with</param>
    ///// <param name="type">The type of task, used to select which object function to use</param>
    //public void DoAction(Colonist colonist, BuildingObject building, TaskType type)
    //{
    //    //go through the building's worldobjectfunctions
    //    //find one that matches type
    //    //run that WOFunction's AddColonist method, adding colonist        
    //    switch(type)
    //    {
    //        case TaskType.Construct:
    //            //send the fetcher to get the materials for construction
    //            if (colonist.isTeamFetcher)
    //                {
    //                #region Trip to Storage
    //                //1. Locate storage unit to go to
    //                //have a manager somewhere that stores a list of storage buildings which is updated whenever a new one is placed, and here just access that list **TODO**
    //                StorageBuilding[] storageBuildings = Object.FindObjectsOfType<StorageBuilding>();                    
    //                //2. Get the ingredients for the building being constructed
    //                //Get the ResourceReqs from the building
    //                ResourceRequirement resourceReqs = building.ConstructionFunction.ResourceReq;
    //                List<ResourceAmount> resourceAmounts = resourceReqs.ResourceAmounts;
    //                SendFetcherToCollectResources(colonist, storageBuildings, resourceAmounts);                    
    //                #endregion
    //            }
    //            building.ConstructionFunction.ColonistReq.AddColonist(colonist);
    //            break;
    //        case TaskType.Demolish:
    //            building.DemolishFunction.ColonistReq.AddColonist(colonist);
    //            //re-add the colonist model to the object pool? **TODO** [for each tast type]
    //            break;
    //        case TaskType.Health:
    //            if (building.GetType().IsSubclassOf(typeof(HealthBuilding)))
    //            {
    //                HealthBuilding hB = (HealthBuilding)building;
    //                hB.HealthFunction.ColonistReq.AddColonist(colonist);
    //                break;
    //            }
    //            else { break; }
    //        case TaskType.Housing:
    //            if (building.GetType().IsSubclassOf(typeof(HousingBuilding)))
    //            {
    //                HousingBuilding hB = (HousingBuilding)building;
    //                hB.HousingFunction.ColonistReq.AddColonist(colonist);
    //                break;
    //            }
    //            else { break; }
    //        case TaskType.Maintain:
    //            building.MaintenanceFunction.ColonistReq.AddColonist(colonist);
    //            break;
    //        case TaskType.Mood:
    //            if (building.GetType().IsSubclassOf(typeof(MoodBuilding)))
    //            {
    //                MoodBuilding mB = (MoodBuilding)building;
    //                mB.MoodFunction.ColonistReq.AddColonist(colonist);
    //                break;
    //            }
    //            else { break; }
    //        case TaskType.Repair:
    //            building.RepairFunction.ColonistReq.AddColonist(colonist);
    //            break;
    //    }
    //}

    ////**TODO** Move this to TaskManager
    //StorageBuilding GetClosestStorage(StorageBuilding[] storageBuildings, Colonist c) 
    //{
    //    StorageBuilding closest = null;
    //    float closestDistance = Mathf.Infinity;
    //    Vector3 position = c.transform.position;

    //    foreach (StorageBuilding sB in storageBuildings)
    //    {
    //    //    if (targetDrop.GetComponent<StorageBuilding>() != null)
    //    //    {
    //            StorageBuilding targetNode = sB;
    //            Vector3 direction = sB.transform.position - position;
    //            float distance = direction.sqrMagnitude;
    //            if (distance < closestDistance)
    //            {
    //               closestDistance = distance;
    //               closest = sB;
    //            }
    //    //    }

    //    }
    //    return closest;
    //}

    ////**TODO** Move this to TaskManager
    //StorageBuilding GetClosestStorageToOtherStorage(List<StorageBuilding> storageBuildings, StorageBuilding x) 
    //{
    //    StorageBuilding closest = null;
    //    float closestDistance = Mathf.Infinity;
    //    Vector3 position = x.transform.position;

    //    foreach (StorageBuilding sB in storageBuildings)
    //    {
    //        //    if (targetDrop.GetComponent<StorageBuilding>() != null)
    //        //    {
    //        StorageBuilding targetNode = sB;
    //        Vector3 direction = sB.transform.position - position;
    //        float distance = direction.sqrMagnitude;
    //        if (distance < closestDistance)
    //        {
    //            closestDistance = distance;
    //            closest = sB;
    //        }
    //        //    }

    //    }
    //    return closest;
    //}

    ////**TODO** Move this to TaskManager
    //List<StorageBuilding> GetStorageBuildingOrderedDistanceList(StorageBuilding[] storageBuildings, Colonist c) 
    //{
    //    List<StorageBuilding> storageList = new List<StorageBuilding>(storageBuildings);
    //    List<StorageBuilding> orderedList = new List<StorageBuilding>();
    //    orderedList.Add(GetClosestStorage(storageBuildings, c));       
    //    for (int i = 0; i < orderedList.Count; i++)
    //    {
    //        orderedList.Add(GetClosestStorageToOtherStorage(storageList, orderedList[i]));            
    //    }
    //    return orderedList;
    //}

    ////**TODO** Rework this into giving extra tasks to a specific assigned colonist? In which case move to TaskManager and create a "fetch" task : when assigning a relevent task, give Colonist[0] the fetch task, and everyone else the basic one
    //void SendFetcherToCollectResources(Colonist colonist, StorageBuilding[] storageBuildings, List<ResourceAmount> resourceAmounts) 
    //{
    //    //CREATE THE ORDERED STORAGE LIST
    //    List<StorageBuilding> orderedListOfStorageUnitsByDistance = GetStorageBuildingOrderedDistanceList(storageBuildings, colonist);
    //    //Get Closest storage unit
    //    StorageBuilding tempStorageBuilding = GetClosestStorage(storageBuildings, colonist);

    //    DoAction(colonist, orderedListOfStorageUnitsByDistance[0].transform.position); //CHANGE THIS TO GET THE ENTRY HOTSPOT TO THE BUILDING, ONCE ADDED **TODO**     

    //    //for each resource
    //    //check it's inventory
    //    //if it has enough of x resource, take it all
    //    //if it only has partially enough, take what it has

    //    //Check if resourceAmounts has been fulfilled
    //    for (int i = 0; i < resourceAmounts.Count; i++)
    //    {
    //        Resource r = resourceAmounts[i].Resource;
    //        int amount = resourceAmounts[i].Amount;

    //        //Compare these to the colonist's inventory
    //        if (!colonist.inventory.HasAmountOfResource(r, amount))
    //        {
    //            //if not, remove the closest storage unit from storageBuildings, repeat this method
    //            //REMOVE THE CLOSEST UNIT FROM THE STORAGE LIST, THEN RERUN
    //            orderedListOfStorageUnitsByDistance.RemoveAt(0);
    //            SendFetcherToCollectResources(colonist, storageBuildings, resourceAmounts); //create a sub method to recur, instead of this one - this currently will always go to the same storage building **TODO**
    //        }
    //    }     
    //}
    #endregion
}
