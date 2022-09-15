using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartCommandInitiator : CommandInitiator
{

    public override AbstractCommand Initiate(GameObject commandObject)
    {
        return new StartCommand(commandObject);
    }

}
