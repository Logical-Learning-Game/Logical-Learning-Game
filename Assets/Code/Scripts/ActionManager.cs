using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ActionManager Instance { get; private set; }
    private TMP_Text actionSequence;

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("actionManager Initialized");
            Instance = this;
            actionSequence = GameObject.Find("ActionSequence").GetComponent<TMP_Text>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddSequenceText(string text)
    {
        actionSequence.text += text;
    }

    public void ClearSequenceText()
    {
        actionSequence.text = "Action:\n";
    }
}
