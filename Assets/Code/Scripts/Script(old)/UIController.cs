using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject playStatus;
    private GameObject reverseText;
    private GameObject actionSpeed;
    void Start()
    {
        playStatus = GameObject.Find("Pause");
        reverseText = GameObject.Find("Reverse");
        actionSpeed = GameObject.Find("SpeedParameter");
    }

    // Update is called once per frame
    void Update()
    {

        playStatus.SetActive(PlayerController.isPause);
        reverseText.SetActive(PlayerController.isReverse);
        actionSpeed.GetComponent<TMP_Text>().text = (PlayerController.speed/1.5f)+"x";
    }
}
