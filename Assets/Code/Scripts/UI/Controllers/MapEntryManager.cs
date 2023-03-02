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

namespace Unity.Game.UI
{
    public class MapEntryManager : MonoBehaviour
    {
        [SerializeField] MapDataManager mapDataManager;

        public static event Action<bool> SelectMap;
        public static event Action UpdateMap;
        public static event Action LoadMap;

        //public static Dictionary<string, List<Map>> MapLists;

        public List<WorldData> WorldDatas; 

        public Map testdisplaymap;

        GameData gameData;

        DropdownField dropdownField;
        ListView entryView;

        List<Map> mapEntryList;
        [SerializeField] private Sprite RuleComplete;
        [SerializeField] private Sprite RuleIncomplete;

        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            PanelScreen.OpenLevelPanel += OnOpenLevelPanel;
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            //MapDataManager.WorldDataLoaded += OnWorldDataLoaded;

            mapEntryList = new List<Map>();
        }

        private void OnDisable()
        {
            PanelScreen.OpenLevelPanel -= OnOpenLevelPanel;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            //MapDataManager.WorldDataLoaded -= OnWorldDataLoaded;
        }

        public void UpdateMapData()
        {
            // mockup for sync map data with network
        }

        public void LoadMapFromFile()
        {
            WorldDatas = mapDataManager.OnLoadMap();
        }

        //public void OnWorldDataLoaded(List<WorldData> worldDatas)
        //{
        //    WorldDatas = worldDatas;
        //}

        public void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;
            
        }

        public void GenerateMapEntry(string worldSelector)
        {
            if (worldSelector == "__NONE__") return;
            mapEntryList.Clear();
            WorldData selectedWorld = WorldDatas.FirstOrDefault(w => w.WorldName == worldSelector);
            foreach (var map in selectedWorld.MapLists)
            {
                mapEntryList.Add(map);
            }


            if (entryView != null)
            {
                entryView.Rebuild();
            }
            else
            {
                SetUpListView();
            }

            if (mapEntryList.Count == 1)
            {
                entryView.style.height = 275;
            }
            else
            {
                entryView.style.height = 550;
            }


        }
        void SetUpListView()
        {
            entryView = GetComponent<PanelScreen>().LevelPanel.Q<ListView>("LevelListView");
            FixListViewScrollingBug(entryView);
            entryView.Q<ScrollView>().scrollDecelerationRate = 0.0035f;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/MapEntryTemplate.uxml");
            Func<VisualElement> makeItem = () => visualTree.Instantiate();
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                e.Q<Label>("MapName").text = mapEntryList[i].MapName;
                e.Q<Label>("RequirementValue").text = mapEntryList[i].StarRequirement.ToString();
                e.Q<UnityEngine.UIElements.Button>("MapEntryButton").RegisterCallback<ClickEvent>(e => OnClickMapEntry(e, mapEntryList[i]));
            };

            entryView.makeItem = makeItem;
            entryView.itemsSource = mapEntryList;
            entryView.bindItem = bindItem;

            //entryView.onItemsChosen += Debug.Log;
        }

        void CreateDropDownMenu()
        {
           
            dropdownField = GetComponent<PanelScreen>().LevelPanel.Q<DropdownField>("WorldSelector");
            dropdownField.RegisterValueChangedCallback(x => GenerateMapEntry(x.newValue));
            dropdownField.choices = GetWorldEntries();
            dropdownField.value = dropdownField.choices[0];

        }

        public void OnOpenLevelPanel()
        {
            if (WorldDatas.Count == 0)
            {
                LoadMapFromFile();
            }
            if (dropdownField == null)
            {
                CreateDropDownMenu();
            }
            GenerateMapEntry(dropdownField.value);
        }

        public List<string> GetWorldEntries()
        {
            if (WorldDatas == null || WorldDatas.Count == 0) return new List<string>() {"__NONE__" };
            return WorldDatas.Select(w => w.WorldName).ToList();
        }

        void OnClickMapEntry(ClickEvent evt,Map map)
        {
            //Debug.Log(map.MapName);
            LoadLevelMap(map);
        }

        void LoadLevelMap(Map map)
        {
            bool isSameMap = false;
            //Debug.Log("loadlevelmap is trigger");
            if (LevelManager.Instance == null) {
                gameObject.AddComponent<LevelManager>();
            }
            if(LevelManager.Instance.GetMap() != null)
            {
                isSameMap = (map.Id == LevelManager.Instance.GetMap().Id);
            }
            LevelManager.Instance.SetMap(map);
            SelectMap?.Invoke(isSameMap);
        }

        public static void FixListViewScrollingBug(ListView listView)
        {
#if UNITY_EDITOR
            var scroller = listView.Q<Scroller>();
            listView.RegisterCallback<WheelEvent>(@event =>
            {
                scroller.value += @event.delta.y * 300;
                @event.StopPropagation();
            });
#else
            var scroller = listView.Q<Scroller>();
            listView.RegisterCallback<WheelEvent>(@event => {
                scroller.value -=  @event.delta.y * 30000;
                @event.StopPropagation();
            });
#endif
        }


    }
}
