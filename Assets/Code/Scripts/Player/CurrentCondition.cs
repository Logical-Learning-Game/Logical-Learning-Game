using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Conditions;
using Unity.Game.Level;

public class CurrentCondition : MonoBehaviour
{
    [SerializeField] TMP_Text TextLabel;
    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        if(TextLabel == null)
        {
            TextLabel = GetComponentInChildren<TMP_Text>();
        }
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        LevelManager.OnSignChanged += SetDisplayText;
    }

    private void OnDisable()
    {
        LevelManager.OnSignChanged -= SetDisplayText;
    }

    void SetDisplayText(ConditionSign sign)
    {
        switch (sign)
        {
            case ConditionSign.EMPTY:
                transform.localScale = Vector3.zero;
                break;
            case ConditionSign.A:
                TextLabel.text = "A";
                transform.localScale = originalScale;
                break;
            case ConditionSign.B:
                TextLabel.text = "B";
                transform.localScale = originalScale;
                break;
            case ConditionSign.C:
                TextLabel.text = "C";
                transform.localScale = originalScale;
                break;
            case ConditionSign.D:
                TextLabel.text = "D";
                transform.localScale = originalScale;
                break;
            case ConditionSign.E:
                TextLabel.text = "E";
                transform.localScale = originalScale;
                break;
            default: break;
        }
    }
}
