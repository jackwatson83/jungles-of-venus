using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health : Stat
{
    public bool isActive;

    /// <summary>
    /// Checks if the object has 0 or less health, and is consequently deaded
    /// </summary>
    /// <returns>True if alive. False if dead.</returns>
    public bool CheckIfDead()
    {
        int h = GetCurrent();
        if (h == 0)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
        }

        return isActive;
    }
	
}
