using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Game.Level;
using Unity.Game.SaveSystem;
using Unity.Game.RuleSystem;

namespace Unity.Game.UI
{
    public class UserStatManager : MonoBehaviour
    {
        [SerializeField] MapDataManager mapDataManager;

        public static event Action<List<int>> UpdateUserStat;
        //public static event Action UpdateMap;
        //public static event Action LoadMap;

        //public static Dictionary<string, List<Map>> MapLists;

        public List<WorldData> WorldDatas;

        //public Map testdisplaymap;

        GameData gameData;

        //DropdownField dropdownField;
        //ListView entryView;

        //List<Map> mapEntryList;
        //[SerializeField] private Sprite RuleComplete;
        //[SerializeField] private Sprite RuleIncomplete;

        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            PanelScreen.OpenStatPanel += OnOpenStatPanel;
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            MapDataManager.WorldDataLoaded += OnWorldDataLoaded;

            //mapEntryList = new List<Map>();
        }

        private void OnDisable()
        {
            PanelScreen.OpenStatPanel -= OnOpenStatPanel;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            MapDataManager.WorldDataLoaded -= OnWorldDataLoaded;
        }

        public void LoadMapFromFile()
        {
            WorldDatas = mapDataManager.OnLoadMap();
        }

        public void OnWorldDataLoaded(List<WorldData> worldDatas)
        {
            WorldDatas = worldDatas;
        }

        public void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;

        }

        public void OnOpenStatPanel()
        {
            if (WorldDatas.Count == 0)
            {
                LoadMapFromFile();
            }

            UpdateUserStat?.Invoke(CalculateUserStat(gameData, WorldDatas));

        }

        List<int> CalculateUserStat(GameData gameData, List<WorldData> worldDatas)
        {
            int starNormalMax = 0, starConditionalMax = 0, starLoopMax = 0;
            int playerStarNormalMax = 0, playerStarConditionalMax = 0, playerStarLoopMax = 0;
            int playerGoldMedal = 0, playerSilverMedal = 0, playerBronzeMedal = 0;

            //collect max data
            foreach (WorldData world in worldDatas)
            {
                //Debug.Log($"World Name: {world.WorldName}, World ID: {world.WorldId}");
                foreach (Map map in world.MapLists)
                {
                    //Debug.Log($"Map Name: {map.MapName}, Map ID: {map.Id}");
                    foreach (IRule rule in map.MapRules)
                    {
                        switch (rule.RuleTheme)
                        {
                            case RuleTheme.NORMAL:
                                starNormalMax += 1;
                                break;
                            case RuleTheme.CONDITIONAL:
                                starConditionalMax += 1;
                                break;
                            case RuleTheme.LOOP:
                                starLoopMax += 1;
                                break;
                            default: break;
                        }
                    }
                }
            }

            foreach (KeyValuePair<long, SubmitHistory> submit in gameData.SubmitBest)
            {
                if (submit.Value.IsCompleted)
                {
                    switch (submit.Value.CommandMedal)
                    {
                        case Medal.GOLD:
                            playerGoldMedal += 1;
                            break;
                        case Medal.SILVER:
                            playerSilverMedal += 1;
                            break;
                        case Medal.BRONZE:
                            playerBronzeMedal += 1;
                            break;
                        case Medal.NONE: break;
                        default: break;
                    }
                    switch (submit.Value.ActionMedal)
                    {
                        case Medal.GOLD:
                            playerGoldMedal += 1;
                            break;
                        case Medal.SILVER:
                            playerSilverMedal += 1;
                            break;
                        case Medal.BRONZE:
                            playerBronzeMedal += 1;
                            break;
                        case Medal.NONE: break;
                        default: break;
                    }
                    foreach (RuleHistory history in submit.Value.RuleHistories)
                    {
                        if (history.IsPass)
                        {
                            switch (history.Theme)
                            {
                                case RuleTheme.NORMAL:
                                    playerStarNormalMax += 1;
                                    break;
                                case RuleTheme.CONDITIONAL:
                                    playerStarConditionalMax += 1;
                                    break;
                                case RuleTheme.LOOP:
                                    playerStarLoopMax += 1;
                                    break;
                                default: break;
                            }
                        }
                    }
                }
            }

            List<int> result = new List<int>() { starNormalMax, starConditionalMax, starLoopMax,
                playerStarNormalMax, playerStarConditionalMax, playerStarLoopMax,
                playerGoldMedal, playerSilverMedal, playerBronzeMedal };
            return result;
        }


    }
}
