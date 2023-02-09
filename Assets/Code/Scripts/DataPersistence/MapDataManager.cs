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

namespace Unity.Game.SaveSystem
{
    public class MapDataManager : MonoBehaviour
    {

        //Dictionary<string, List<Map>> MapLists = new Dictionary<string, List<Map>>();
        Dictionary<string, List<Map>> MapLists = new Dictionary<string, List<Map>>()
        {
            {"a",new List<Map>(){
                new Map(), new Map(), new Map()
            } },
            {"b",new List<Map>()
            {
                new Map(), new Map()
            } }
        };
        DropdownField dropdownField;
        ListView entryView;

        private void Awake()
        {
            
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            PanelScreen.OpenLevelPanel += OnOpenLevelPanel;
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
        }

        

        public void GenerateMapEntry(string worldSelector)
        {
            Debug.Log("GenerateMapEntry Trigger");
            entryView = GetComponent<PanelScreen>().LevelPanel.Q<ListView>("LevelListView");
            List<Map> displayList = MapLists[worldSelector];

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/MapEntryTemplate.uxml");
            
            Func<VisualElement> makeItem = () => visualTree.Instantiate();
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                e.Q<Label>("MapName").text = displayList[i].MapName;
            };

            entryView.makeItem = makeItem;
            entryView.bindItem = bindItem;
            entryView.itemsSource = displayList;
            entryView.selectionType = SelectionType.None;
        }

        public void CreateDropDownMenu()
        {
            dropdownField = GetComponent<PanelScreen>().LevelPanel.Q<DropdownField>("WorldSelector");
            dropdownField.RegisterValueChangedCallback(x => GenerateMapEntry(x.newValue));
            dropdownField.choices = GetWorldEntries();
            dropdownField.value = dropdownField.choices[0];
        }

        public void OnOpenLevelPanel()
        {
            if(MapLists == null)
            {
                LoadMapFromFile();
            }
            if(dropdownField == null)
            {
                CreateDropDownMenu();
            }
            GenerateMapEntry(dropdownField.value);
        }

        public List<string> GetWorldEntries()
        {
            return MapLists.Keys.ToList();
        }



    }
}
