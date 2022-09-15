using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightForwardCommandInitiator : CommandInitiator
{

    public override AbstractCommand Initiate(GameObject commandObject)
    {
        return new RightForwardCommand(commandObject);
    }

}
