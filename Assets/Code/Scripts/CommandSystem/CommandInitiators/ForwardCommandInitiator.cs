using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForwardCommandInitiator : CommandInitiator
{

    public override AbstractCommand Initiate(GameObject commandObject)
    {
        return new ForwardCommand(commandObject);
    }

}
