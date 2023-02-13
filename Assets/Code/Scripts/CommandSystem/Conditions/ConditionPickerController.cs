using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Game.Command;
using Unity.Game.Level;
using Unity.Game.MapSystem;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Conditions
{

    public class ConditionPickerController : MonoBehaviour
    {

        public static ConditionPickerController Instance { get; private set; }
        [SerializeField] private GameObject ConditionChoiceGameObj;
        [SerializeField] private GameObject ConditionPicker;
        [SerializeField] private bool isPickerOpen = false;
        private Coroutine pickerAnimationCoroutine;

        private ConditionCommand selectingCommand;
        HashSet<ConditionSign> uniqueConditions = new HashSet<ConditionSign>();

        private List<ConditionChoice> conditionList;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ConditionPicker.SetActive(false);
                conditionList = new List<ConditionChoice>();
            }
            else
            {
                Destroy(gameObject);
            }

        }

        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            OnPickCondition();
        }

        void OnPickCondition()
        {
            if (Input.GetMouseButtonDown(0) && isPickerOpen)
            {
                foreach (ConditionChoice choice in conditionList)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(choice.transform as RectTransform, Input.mousePosition))
                    {
                        selectingCommand.SetCondition(choice.GetCondition());
                        Close();
                        return;
                    }

                }
                Close();
            }
        }

        public void Open(Transform transform)
        {

            isPickerOpen = true;
            ConditionPicker.SetActive(true);

            if (pickerAnimationCoroutine != null)
            {
                StopCoroutine(pickerAnimationCoroutine);
            }
            pickerAnimationCoroutine = StartCoroutine(AnimatePicker(isPickerOpen));

            ConditionPicker.transform.position = transform.position;
            selectingCommand = transform.GetComponentInParent<ConditionCommand>();

        }

        public void Close()
        {
            isPickerOpen = false;
            if (pickerAnimationCoroutine != null)
            {
                StopCoroutine(pickerAnimationCoroutine);
            }
            pickerAnimationCoroutine = StartCoroutine(AnimatePicker(isPickerOpen));
        }

        private IEnumerator AnimatePicker(bool isPickerOpen)
        {
            RadialLayout layout = ConditionPicker.GetComponent<RadialLayout>();

            float startValue = isPickerOpen ? 0f : 360f;
            float endValue = isPickerOpen ? 360f : 0f;
            float elapsedTime = 0f;
            float duration = .3f;

            while (elapsedTime < duration)
            {
                float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                layout.MinAngle = currentValue;
                layout.CalculateLayoutInputHorizontal();
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            layout.MinAngle = endValue;
            layout.CalculateLayoutInputHorizontal();
            ConditionPicker.SetActive(isPickerOpen);
        }

        public void InitConditionPicker()
        {
            uniqueConditions = LevelManager.Instance.GetUniqueConditions();
            foreach (ConditionSign condition in uniqueConditions)
            {
                ConditionChoice choice = Instantiate(ConditionChoiceGameObj, ConditionPicker.transform).GetComponent<ConditionChoice>();
                choice.SetCondition(condition);
                conditionList.Add(choice);
            }
        }

        //public HashSet<ConditionSign> GetUniqueConditions()
        //{
        //    TileType[,] TileArray = LevelManager.Instance.GetMap().TileArray;
        //    HashSet<ConditionSign> uniqueConditions = new HashSet<ConditionSign>();
        //    foreach (TileType tile in TileArray)
        //    {
        //        if (tile == TileType.CONDITION_A)
        //        {
        //            uniqueConditions.Add(ConditionSign.A);
        //        }
        //        else if (tile == TileType.CONDITION_B)
        //        {
        //            uniqueConditions.Add(ConditionSign.B);
        //        }
        //        else if (tile == TileType.CONDITION_C)
        //        {
        //            uniqueConditions.Add(ConditionSign.C);
        //        }
        //        else if (tile == TileType.CONDITION_D)
        //        {
        //            uniqueConditions.Add(ConditionSign.D);
        //        }
        //        else if (tile == TileType.CONDITION_E)
        //        {
        //            uniqueConditions.Add(ConditionSign.E);
        //        }
        //    }
        //    return uniqueConditions;
        //}
    }

}