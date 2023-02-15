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

namespace Unity.Game.UI
{
    public class MapEntryManager : MonoBehaviour
    {
        public static event Action SelectMap;
        public static event Action UpdateMap;
        public static event Action LoadMap;

        Dictionary<string, List<Map>> MapLists;
        //Dictionary<string, List<Map>> MapLists = new Dictionary<string, List<Map>>()
        //{
        //    {"a",new List<Map>(){
        //        new Map("a-1"), new Map("a-2"), new Map("a-3")
        //    } },
        //    {"b",new List<Map>()
        //    {
        //        new Map("b-1"), new Map("b-2")
        //    } },
        //    {"c",new List<Map>(){
        //        new Map("c-1")
        //    } }
        //};
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
            mapEntryList = new List<Map>();
        }

        private void OnDisable()
        {
            PanelScreen.OpenLevelPanel -= OnOpenLevelPanel;
        }

        public void UpdateMapData()
        {
            // mockup for sync map data with network
        }

        public void LoadMapFromFile()
        {
            // read map from file
            MapLists = new Dictionary<string, List<Map>>()
        {
            {"a",new List<Map>(){
                new Map("a-1"), new Map("a-2"), new Map("a-3")
            } },
            {"b",new List<Map>()
            {
                new Map("b-1"), new Map("b-2")
            } },
            {"c",new List<Map>(){
                new Map("c-1")
            } }
        };
        }



        public void GenerateMapEntry(string worldSelector)
        {
            mapEntryList.Clear();
            foreach (var map in MapLists[worldSelector])
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

            entryView.onItemsChosen += Debug.Log;
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
            if (MapLists == null)
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
            return MapLists.Keys.ToList();
        }

        void OnClickMapEntry(ClickEvent evt,Map map)
        {
            Debug.Log(map.MapName);
            LoadLevelMap(map);
        }

        void LoadLevelMap(Map map)
        {
            //Debug.Log("loadlevelmap is trigger");
            LevelManager.Instance.SetMap(map);
            SelectMap?.Invoke();
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
