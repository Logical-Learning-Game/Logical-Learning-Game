using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;

public class Spinnable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set the initial rotation
        //transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, ItemConfig.ITEM_SPIN_SPEED, 0);
    }
}
