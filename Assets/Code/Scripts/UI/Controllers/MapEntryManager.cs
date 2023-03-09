﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Game.Level;
using Unity.Game.SaveSystem;
using System.Threading.Tasks;

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

        public bool isMapLoading;
        //public Map testdisplaymap;
        Coroutine RotateCoroutine;
        Quaternion originalRotation;

        Button MapUpdateButton;

        GameData gameData;

        DropdownField dropdownField;
        public static string LatestChoice;
        ListView entryView;

        List<Map> mapEntryList;
        [SerializeField] private Texture2D RuleComplete;
        [SerializeField] private Texture2D RuleIncomplete;
        [SerializeField] private Texture2D MapImagePlaceHolder;

        private void Awake()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        private void Start()
        {

        }

        void SetVisualElements()
        {
            VisualElement LevelPanel = GetComponent<PanelScreen>().LevelPanel;
            MapUpdateButton = LevelPanel.Q<Button>("MapUpdateButton");
        }
        void RegisterButtonCallbacks()
        {
            MapUpdateButton?.RegisterCallback<ClickEvent>(UpdateMapData);
            MapUpdateButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
        }

        void MouseOverButton(MouseOverEvent evt)
        {
            AudioManager.PlayDefaultHoverSound();
        }
        private void OnEnable()
        {
            PanelScreen.OpenLevelPanel += OnOpenLevelPanel;
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            MapDataManager.WorldDataLoaded += OnWorldDataLoaded;

            mapEntryList = new List<Map>();
        }

        private void OnDisable()
        {
            PanelScreen.OpenLevelPanel -= OnOpenLevelPanel;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            MapDataManager.WorldDataLoaded -= OnWorldDataLoaded;
        }

        public async void UpdateMapData(ClickEvent evt)
        {
            var apiClient = new APIClient();

            isMapLoading = true;
            SetButtonLoading(isMapLoading);

            try
            {
                await mapDataManager.UpdateMap();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            isMapLoading = false;
            SetButtonLoading(isMapLoading);
        }

        void SetButtonLoading(bool isLoading)
        {
            VisualElement icon = MapUpdateButton.contentContainer.Q<VisualElement>("RotateIcon");

            if (isLoading)
            {
                RotateCoroutine = StartCoroutine(RotateIcon(icon));
                MapUpdateButton.SetEnabled(false);
                originalRotation = icon.transform.rotation;
            }
            else
            {
                MapUpdateButton.SetEnabled(true);
                StopCoroutine(RotateCoroutine);
                icon.transform.rotation = originalRotation;
            }
        }

        IEnumerator RotateIcon(VisualElement loadingSpinner)
        {
            while (true)
            {
                loadingSpinner.transform.rotation = Quaternion.Euler(0, 0, loadingSpinner.transform.rotation.eulerAngles.z + 10f);
                yield return null;
            }
        }

        public void LoadMapFromFile()
        {
            WorldDatas = mapDataManager.OnLoadMap();
        }

        public void OnWorldDataLoaded(List<WorldData> worldDatas)
        {
            WorldDatas = worldDatas;

            if (entryView != null)
            {
                entryView.Rebuild();
            }
            else
            {
                SetUpListView();
            }
        }

        public void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;

            if (entryView != null)
            {
                entryView.Rebuild();
            }
            else
            {
                SetUpListView();
            }
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
            entryView.Q<ScrollView>().scrollDecelerationRate = 5f;
            entryView.Q<ScrollView>().elasticity = 3f;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/MapEntryTemplate.uxml");
            Func<VisualElement> makeItem = () => visualTree.Instantiate();
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                // set map name
                e.Q<Label>("MapName").text = mapEntryList[i].MapName;

                // reset map image to placeholder first
                e.Q<VisualElement>("MapPreviewImage").style.backgroundImage = MapImagePlaceHolder;
                // set map image
                string mapImageFilename = mapEntryList[i].MapImagePath;
                StartCoroutine(MapImageManager.GetMapImage(mapEntryList[i].Id.ToString(), mapImageFilename, texture =>
                {
                    e.Q<VisualElement>("MapPreviewImage").style.backgroundImage = texture;
                }));

                //setting isEnterable based on currentstar
                bool isEnterable = gameData.GetCurrentStar() >= mapEntryList[i].StarRequirement ? true : false;

                //starRequirement
                e.Q<Label>("RequirementValue").text = mapEntryList[i].StarRequirement.ToString();

                // Accessing Player Submit Data
                SubmitHistory mapBestSubmit;

                if (gameData.SubmitBest.TryGetValue(mapEntryList[i].Id, out mapBestSubmit))
                {
                    if (mapBestSubmit.IsCompleted)
                    {
                        e.Q($"RuleStar{1}").style.backgroundImage = mapBestSubmit.RuleHistories[0].IsPass ? RuleComplete : RuleIncomplete;
                        e.Q($"RuleStar{2}").style.backgroundImage = mapBestSubmit.RuleHistories[1].IsPass ? RuleComplete : RuleIncomplete;
                        e.Q($"RuleStar{3}").style.backgroundImage = mapBestSubmit.RuleHistories[2].IsPass ? RuleComplete : RuleIncomplete;
                        e.Q("CommandMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[mapBestSubmit.CommandMedal];
                        e.Q("ActionMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[mapBestSubmit.ActionMedal];
                    }

                    e.Q<Label>("BestPlayDate").text = mapBestSubmit.SubmitDatetime.ToString("g");
                }
                else
                {
                    e.Q($"RuleStar{1}").style.backgroundImage = RuleIncomplete;
                    e.Q($"RuleStar{2}").style.backgroundImage = RuleIncomplete;
                    e.Q($"RuleStar{3}").style.backgroundImage = RuleIncomplete;
                    e.Q("CommandMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                    e.Q("ActionMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                }
                var mapEntryButton = e.Q<UnityEngine.UIElements.Button>("MapEntryButton");
                mapEntryButton.clickable = new Clickable(e => OnClickMapEntry(e, mapEntryList[i], isEnterable));
                mapEntryButton.UnregisterCallback<MouseOverEvent>(e => AudioManager.PlayDefaultHoverSound());
                mapEntryButton.RegisterCallback<MouseOverEvent>(e => AudioManager.PlayDefaultHoverSound());

            };

            entryView.makeItem = makeItem;
            entryView.itemsSource = mapEntryList;
            entryView.bindItem = bindItem;
        }

        void CreateDropDownMenu()
        {

            dropdownField = GetComponent<PanelScreen>().LevelPanel.Q<DropdownField>("WorldSelector");
            dropdownField.RegisterValueChangedCallback(x =>
            {
                GenerateMapEntry(x.newValue);
                LatestChoice = x.newValue;
            });
            dropdownField.choices = GetWorldEntries();
            dropdownField.value = LatestChoice ?? dropdownField.choices[0];

        }

        public void OnOpenLevelPanel()
        {
            if (WorldDatas.Count == 0)
            {
                LoadMapFromFile();
            }

            CreateDropDownMenu();
            GenerateMapEntry(dropdownField.value);
        }

        public List<string> GetWorldEntries()
        {
            if (WorldDatas == null || WorldDatas.Count == 0) return new List<string>() { "__NONE__" };
            return WorldDatas.Select(w => w.WorldName).ToList();
        }

        void OnClickMapEntry(EventBase evt, Map map, bool isEnterable)
        {
            Debug.Log($"{map.MapName}: can enter? : {isEnterable}");
            if (isEnterable)
            {
                LoadLevelMap(map);
            }
            else
            {
                AudioManager.PlayDefaultWarningSound();
            }

        }

        void LoadLevelMap(Map map)
        {
            bool isSameMap = false;
            //Debug.Log("loadlevelmap is trigger");
            if (LevelManager.Instance == null)
            {
                gameObject.AddComponent<LevelManager>();
            }
            if (LevelManager.Instance.GetMap() != null)
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
                scroller.value += @event.delta.y * 1000;
                @event.StopPropagation();
            });
#else
                    var scroller = listView.Q<Scroller>();
                    listView.RegisterCallback<WheelEvent>(@event => {
                        scroller.value -=  @event.delta.y * 1000;
                        @event.StopPropagation();
                    });
#endif
        }


    }
}
