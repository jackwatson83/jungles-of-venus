//v3 - JWHW

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabWizard : ScriptableWizard {

    [Tooltip("GameObject to add the functionality to."), SerializeField] private GameObject prefabObject;    
    private enum WorldObject { Building, ResourceNode}
    [Tooltip("Select if you want the object to be a Building or Environment Object."), SerializeField] private WorldObject worldObjectType;
    [Tooltip("Put the ScriptableObject here. (Building)"), SerializeField] private Building buildingType;
    [Tooltip("Put the ScriptableObject here. (Environment)"), SerializeField] private ResourceNode nodeType;

    [MenuItem("Tools/Prefab Builder")]
	static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<PrefabWizard>("Prefab Builder", "Update Selected", "Create New");
    }

    //This is what runs when the wizard is opened
    //The method names are part of the ScriptableWizard class itself, nothing to do with the names of the buttons, apologies for any confusion
    void OnWizardUpdate()
    {
        //The text displayed at the top of the wizard
        helpString = string.Format("This wizard will add the required scripts and set up the object ready to be turned into a prefab.");
    }

    //This is the default button; "Update Selected"
    private void OnWizardCreate()
    {
        //Check to make sure the user has selected an object
        if(prefabObject != null)
        {
            //Check if the desired object is Building or Environmental
            if (worldObjectType == WorldObject.Building)
            {
                //Check to make see if there is already a BuildingTemplate component
                BuildingTemplate bTemplate = prefabObject.GetComponent<BuildingTemplate>();
                //If there already is a Template, set the building type to be the chosen type
                if (bTemplate != null)
                {
                    bTemplate.building = buildingType;
                }
                //If there isn't a template already, add one before setting the building type
                else
                {
                    BuildingTemplate bT = prefabObject.AddComponent<BuildingTemplate>();
                    bT.building = buildingType;
                }
            }
            else if (worldObjectType == WorldObject.ResourceNode)
            {
                //Check to make see if there is already a EnvironmentalTemplate component
                ResourceNodeTemplate nodeTemplate = prefabObject.GetComponent<ResourceNodeTemplate>();
                //If there already is a Template, set the environment type to be the chosen type
                if (nodeTemplate != null)
                {
                    nodeTemplate.node = nodeType;
                }
                //If there isn't a template already, add one before setting the type
                else
                {
                    ResourceNodeTemplate rT = prefabObject.AddComponent<ResourceNodeTemplate>();
                    rT.node = nodeType;
                }
            }
        }
        else
        {
            if (EditorUtility.DisplayDialog("Error: No Selected GameObject", "Would you like to create a new object, or go back and reselect?", "Create New", "Go Back"))
            {
                //If the user chooses to "Create New", run the method as if they had chosen to create a new object
                //Selecting "Go Back" will close the error window
                OnWizardOtherButton();
            }
        }
    }

    //This is the "Create New" button
    private void OnWizardOtherButton()
    {
        //Create a new empty object
        GameObject newObject = new GameObject();
        //Check which type of prefab is desired
        if (worldObjectType == WorldObject.Building)
        {
            //Add the correct template, and fill it
            BuildingTemplate newBuildingComponent = newObject.AddComponent<BuildingTemplate>();
            newBuildingComponent.building = buildingType;
        }
        else if (worldObjectType == WorldObject.ResourceNode)
        {
            //Add the correct template, and fill it
            ResourceNodeTemplate newResourceComponent = newObject.AddComponent<ResourceNodeTemplate>();
            newResourceComponent.node = nodeType;
        }
    }
}
