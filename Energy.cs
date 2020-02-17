using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Energy : Stat
{
    /// <summary>
    /// Reset the energy to maximum
    /// </summary>
    public void Rest()
    {
        int e = GetMax();
        SetCurrent(e);
    }
}
