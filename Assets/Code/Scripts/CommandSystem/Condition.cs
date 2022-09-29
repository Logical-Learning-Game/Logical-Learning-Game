using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    private bool result = false;

    public Condition()
    {
        
    }

    public void SetResult(bool result)
    {
        this.result = result;
    }

    public bool GetResult()
    {
        return result;
    }

}
