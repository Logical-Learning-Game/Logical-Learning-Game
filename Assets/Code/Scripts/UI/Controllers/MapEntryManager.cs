using System;
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
        ListView entryView;

        List<Map> mapEntryList;
        [SerializeField] private Sprite RuleComplete;
        [SerializeField] private Sprite RuleIncomplete;

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
        }

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

                //starRequirement
                e.Q<Label>("RequirementValue").text = mapEntryList[i].StarRequirement.ToString();

                // Accessing PlayerData
                SubmitHistory mapBestSubmit;
                if (gameData.SubmitBest.TryGetValue(mapEntryList[i].Id, out mapBestSubmit))
                {
                    if (mapBestSubmit.IsCompleted)
                    {
                        e.Q($"RuleStar{1}").style.backgroundImage = new StyleBackground(mapBestSubmit.RuleHistories[0].IsPass ? RuleComplete : RuleIncomplete);
                        e.Q($"RuleStar{2}").style.backgroundImage = new StyleBackground(mapBestSubmit.RuleHistories[1].IsPass ? RuleComplete : RuleIncomplete);
                        e.Q($"RuleStar{3}").style.backgroundImage = new StyleBackground(mapBestSubmit.RuleHistories[2].IsPass ? RuleComplete : RuleIncomplete);
                    }
                    else
                    {
                        e.Q($"RuleStar{1}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                        e.Q($"RuleStar{2}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                        e.Q($"RuleStar{3}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    }
                    e.Q<Label>("BestPlayDate").text = mapBestSubmit.SubmitDatetime.ToString("g");
                }
                e.Q<UnityEngine.UIElements.Button>("MapEntryButton").RegisterCallback<ClickEvent>(e => OnClickMapEntry(e, mapEntryList[i]));
                e.Q<UnityEngine.UIElements.Button>("MapEntryButton").RegisterCallback<MouseOverEvent>(e => AudioManager.PlayDefaultHoverSound());
            };

            entryView.makeItem = makeItem;
            entryView.itemsSource = mapEntryList;
            entryView.bindItem = bindItem;
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
            if (WorldDatas == null || WorldDatas.Count == 0) return new List<string>() { "__NONE__" };
            return WorldDatas.Select(w => w.WorldName).ToList();
        }

        void OnClickMapEntry(ClickEvent evt, Map map)
        {
            //Debug.Log(map.MapName);
            LoadLevelMap(map);
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
