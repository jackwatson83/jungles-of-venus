using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat
{
    /// <summary>
    /// the maximum value for this stat
    /// </summary>
    private int max;

    /// <summary>
    /// the current value for this stat
    /// </summary>
    private int current;

    /// <summary>
    /// method to change the current value of the stat
    /// </summary>
    /// <param name="direction">True to increase, False to decrease.</param>
    /// <param name="amount">The amount to change by</param>
    public void ChangeCurrent(bool direction, int amount)
    {
        if(direction)
        {
            //Check if increasing the stat will take it over the max
            if ((current += amount) > max)
            {
                //if so, set stat to max
                current = max;
            }
            else
            {
                current += amount;
            }
        }
        else if (!direction)
        {
            //Check if decreasing the stat will take it below zero
            if((current -= amount) < 0)
            {
                //if so, set current to be zero
                current = 0;
            }
            else
            {
                current -= amount;
            }
        }
        else
        {
            Debug.Log("Invalid change in current stat.");
        }
    }

    /// <summary>
    /// method to change max value of the stat
    /// </summary>
    /// <param name="direction">True to increase, False to decrease</param>
    /// <param name="amount">the amount to change by</param>
    private void ChangeMax(bool direction, int amount)
    {
        if (direction)
        {
            max += amount;
        }
        else if (!direction)
        {
            max -= amount;
        }
        else
        {
            Debug.Log("Invalid change in stat's maximum.");
        }
    }

    /// <summary>
    /// Allows the current value to be set to a specific value
    /// </summary>
    /// <param name="target">The value to set the stat to</param>
    public void SetCurrent(int target)
    {
        //make sure the target is valid
        if (target > max || target < 0)
        {
            Debug.Log("The value cannot be set to " + target);
        }
        else
        {
            current = target;
        }
    }

    /// <summary>
    /// Allows the max value to be set to a specific value
    /// </summary>
    /// <param name="target">the value to set the stat's maximum to</param>
    public void SetMax(int target)
    {
        if (target > 0)
        {
            max = target;
        }
        else
        {
            Debug.Log("Max cannot be zero");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns current value of stat</returns>
    public int GetCurrent()
    {
        return current;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>returns max value of stat</returns>
    public int GetMax()
    {
        return max;
    }
}
