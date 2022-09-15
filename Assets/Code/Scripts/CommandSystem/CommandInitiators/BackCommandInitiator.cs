using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackCommandInitiator : CommandInitiator
{

    public override AbstractCommand Initiate(GameObject commandObject)
    {
        return new BackCommand(commandObject);
    }

}
