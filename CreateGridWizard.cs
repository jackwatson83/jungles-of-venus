//v5 - JWHW

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateGridWizard : ScriptableWizard {

    [Tooltip("The Game Object to make the grid out of."), SerializeField] private GameObject tile;
    [Tooltip("The number of rows for the grid."), SerializeField] private int rows;
    [Tooltip("The number of columns for the grid."), SerializeField] private int columns;
    [Tooltip("This lets you set a gap between each tile. Not required."), SerializeField] private float tileOffset;
    private GameObject row;

    [MenuItem("Tools/Grid Wizard")]

    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CreateGridWizard>("Grid Creator", "Create Grid");        
    }

    private void OnWizardCreate()
    {   
        //Ensure viable information has been entered
        if (tile != null && rows != 0 && columns !=0)
        {
            GameObject grid = new GameObject("Grid");
            grid.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            CreateGrid(columns, rows, grid);
        }
        //If it hasn't, make a fuss
        else
        {
            Debug.Log(string.Format("<color=red>Error creating grid:</color> Select a Tile Prefab, and ensure Rows/Columns are higher than 0."));
        }
        
    }

    void OnWizardUpdate()
    {
        helpString = string.Format("This lets you create a grid of any size, using a Tile GameObject.\nThe grid will be at world position (0,0,0).");
    }

    private void CreateGrid(int c, int r, GameObject g)
    {
        //Get the mesh of the tile in order to get correct scale of each tile object
        Mesh mesh = tile.GetComponent<MeshFilter>().sharedMesh;
        //First Create a row
        CreateRow(r, g);
        //Then clone the row and move the clones along, taking into account any offset
        for (int i = 0; i < (c-1); i++)
        {            
            GameObject newRow = Instantiate(row, new Vector3((i+1) * (mesh.bounds.size.x + tileOffset), 0.0f, 0.0f), Quaternion.identity, g.transform);
            newRow.transform.position += new Vector3(tileOffset, 0.0f, 0.0f);
        }
    }

    private void CreateRow(int r, GameObject grid)
    {
        //Get the mesh of the tile in order to get correct scale of each tile object
        Mesh mesh = tile.GetComponent<MeshFilter>().sharedMesh;
        //Make a row parent object to hold the tiles in the row
        row = new GameObject("Row");
        row.transform.parent = grid.transform;
        //Make the correct number of tiles, and each time increment it's z position by 1 - also take into account the offset, if desired.
        for (int i = 0; i < r; i++)
        {            
            GameObject t = Instantiate(tile, new Vector3(0.0f, 0.0f, i * (mesh.bounds.size.x + tileOffset)), Quaternion.identity, row.transform);
            t.transform.position += new Vector3(0.0f, 0.0f, tileOffset);
        }
    }
}
