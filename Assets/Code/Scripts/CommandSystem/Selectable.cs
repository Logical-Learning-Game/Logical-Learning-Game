
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour
{
    
    private RectTransform selectingObjectRectTransform;
    [SerializeField]
    private GameObject selectedBorder;
    [SerializeField]
    private bool isSelected = false;

    private void Awake()
    {
        selectingObjectRectTransform = transform as RectTransform;
        selectedBorder = transform.GetChild(0).gameObject;
        selectedBorder.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        //check onclick
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(selectingObjectRectTransform, Input.mousePosition))
            {
                CommandManager.Instance.SetSelectedCommand(selectingObjectRectTransform.gameObject);
            }
        }

        selectedBorder.SetActive(isSelected);

    }

    public void SetisSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

}
