
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandSelectable : MonoBehaviour
{
    
    private RectTransform selectingObjectRectTransform;
    [SerializeField]
    private GameObject selectedBorder;
    [SerializeField]
    private bool isSelected = false;

    private void Awake()
    {
        selectingObjectRectTransform = transform.parent.transform as RectTransform;
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
