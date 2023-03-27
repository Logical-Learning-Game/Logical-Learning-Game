using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Game.SaveSystem;
using Unity.Game.Level;
using Unity.Game.MapSystem;
using GlobalConfig;

namespace Unity.Game.RuleSystem
{
    public class MedalPanelManager : MonoBehaviour
    {
        public static MedalPanelManager Instance { get; private set; }

        [SerializeField] Image CommandMedal;
        [SerializeField] Image ActionMedal;
        [SerializeField] TMP_Text CurrentCommandMedal;
        [SerializeField] TMP_Text CurrentActionMedal;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdatePanel()
        {
            StateValue currentValue = (StateValue)RuleManager.Instance.CurrentStateValue.Clone();
            Map currentMap = LevelManager.Instance.GetMap();

            if (currentValue.CommandCount <= currentMap.LeastSolvableCommandGold)
            {
                CommandMedal.color = ColorConfig.MEDAL_COLOR[Medal.GOLD];
                CurrentCommandMedal.text = $"{currentValue.CommandCount}/{currentMap.LeastSolvableCommandGold}";
                //commandMedal = Medal.GOLD;
            }
            else if (currentValue.CommandCount <= currentMap.LeastSolvableCommandSilver)
            {
                CommandMedal.color = ColorConfig.MEDAL_COLOR[Medal.SILVER];
                CurrentCommandMedal.text = $"{currentValue.CommandCount}/{currentMap.LeastSolvableCommandSilver}";
                //commandMedal = Medal.SILVER;
            }
            else if (currentValue.CommandCount <= currentMap.LeastSolvableCommandBronze)
            {
                CommandMedal.color = ColorConfig.MEDAL_COLOR[Medal.BRONZE];
                CurrentCommandMedal.text = $"{currentValue.CommandCount}/{currentMap.LeastSolvableCommandBronze}";
                //commandMedal = Medal.BRONZE;
            }
            else
            {
                CommandMedal.color = ColorConfig.MEDAL_COLOR[Medal.NONE];
                CurrentCommandMedal.text = $"{currentValue.CommandCount}/{currentMap.LeastSolvableCommandBronze}";
            }

            if (currentValue.ActionCount <= currentMap.LeastSolvableActionGold)
            {
                ActionMedal.color = ColorConfig.MEDAL_COLOR[Medal.GOLD];
                CurrentActionMedal.text = $"{currentValue.ActionCount}/{currentMap.LeastSolvableActionGold}";
                //actionMedal = Medal.GOLD;
            }
            else if (currentValue.ActionCount <= currentMap.LeastSolvableActionSilver)
            {
                ActionMedal.color = ColorConfig.MEDAL_COLOR[Medal.SILVER];
                CurrentActionMedal.text = $"{currentValue.ActionCount}/{currentMap.LeastSolvableActionSilver}";
                //actionMedal = Medal.SILVER;
            }
            else if (currentValue.ActionCount <= currentMap.LeastSolvableActionBronze)
            {
                ActionMedal.color = ColorConfig.MEDAL_COLOR[Medal.BRONZE];
                CurrentActionMedal.text = $"{currentValue.ActionCount}/{currentMap.LeastSolvableActionBronze}";
                //actionMedal = Medal.BRONZE;
            }
            else
            {
                ActionMedal.color = ColorConfig.MEDAL_COLOR[Medal.NONE];
                CurrentActionMedal.text = $"{currentValue.ActionCount}/{currentMap.LeastSolvableActionBronze}";
            }
        }
    }
}