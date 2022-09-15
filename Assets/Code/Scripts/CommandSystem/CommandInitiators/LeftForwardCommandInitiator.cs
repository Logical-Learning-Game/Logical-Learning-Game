using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftForwardCommandInitiator : CommandInitiator
{

    public override AbstractCommand Initiate(GameObject commandObject)
    {
        return new LeftForwardCommand(commandObject);
    }

}
