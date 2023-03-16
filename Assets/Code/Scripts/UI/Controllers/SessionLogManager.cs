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
    public class SessionLogManager : MonoBehaviour
    {
        [SerializeField] MapDataManager mapDataManager;

        //public static event Action<bool> SelectMap;
        //public static event Action UpdateMap;
        //public static event Action LoadMap;

        //public static Dictionary<string, List<Map>> MapLists;

        public List<WorldData> WorldDatas;

        //public bool isMapLoading;
        ////public Map testdisplaymap;
        //Coroutine RotateCoroutine;
        //Quaternion originalRotation;

        //Button MapUpdateButton;

        GameData gameData;

        //DropdownField dropdownField;
        //public static string LatestChoice;
        ListView entryView;

        List<SessionStatus> SessionLogList;
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
            //VisualElement LevelPanel = GetComponent<PanelScreen>().LevelPanel;
            //MapUpdateButton = LevelPanel.Q<Button>("MapUpdateButton");
        }
        void RegisterButtonCallbacks()
        {
            //MapUpdateButton?.RegisterCallback<ClickEvent>(UpdateMapData);
            //MapUpdateButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
        }

        //void MouseOverButton(MouseOverEvent evt)
        //{
        //    AudioManager.PlayDefaultHoverSound();
        //}
        private void OnEnable()
        {
            PanelScreen.OpenHistoryPanel += OnOpenHistoryPanel;
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            MapDataManager.WorldDataLoaded += OnWorldDataLoaded;

            SessionLogList = new List<SessionStatus>();
        }

        private void OnDisable()
        {
            PanelScreen.OpenLevelPanel -= OnOpenHistoryPanel;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            MapDataManager.WorldDataLoaded -= OnWorldDataLoaded;
        }

        //public async void UpdateMapData(ClickEvent evt)
        //{
        //    var apiClient = new APIClient();

        //    isMapLoading = true;
        //    SetButtonLoading(isMapLoading);

        //    try
        //    {
        //        await mapDataManager.UpdateMap();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogException(ex);
        //    }

        //    isMapLoading = false;
        //    SetButtonLoading(isMapLoading);
        //}

        //void SetButtonLoading(bool isLoading)
        //{
        //    VisualElement icon = MapUpdateButton.contentContainer.Q<VisualElement>("RotateIcon");

        //    if (isLoading)
        //    {
        //        RotateCoroutine = StartCoroutine(RotateIcon(icon));
        //        MapUpdateButton.SetEnabled(false);
        //        originalRotation = icon.transform.rotation;
        //    }
        //    else
        //    {
        //        MapUpdateButton.SetEnabled(true);
        //        StopCoroutine(RotateCoroutine);
        //        icon.transform.rotation = originalRotation;
        //    }
        //}

        //IEnumerator RotateIcon(VisualElement loadingSpinner)
        //{
        //    while (true)
        //    {
        //        loadingSpinner.transform.rotation = Quaternion.Euler(0, 0, loadingSpinner.transform.rotation.eulerAngles.z + 10f);
        //        yield return null;
        //    }
        //}

        public void LoadMapFromFile()
        {
            mapDataManager.OnLoadMap();
        }

        public void OnWorldDataLoaded(List<WorldData> worldDatas)
        {
            WorldDatas = worldDatas;

        }

        public void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;

            GenerateSessionLog();
        }

        public void GenerateSessionLog()
        {
            SessionLogList.Clear();
            //WorldData selectedWorld = WorldDatas.FirstOrDefault(w => w.WorldName == worldSelector);
            foreach (var session in gameData.SessionHistories)
            {
                SessionLogList.Add(session);
            }


            if (entryView != null)
            {
                entryView.Rebuild();
            }
            else
            {
                SetUpListView();
            }

            //if (SessionLogList.Count == 1)
            //{
            //    entryView.style.height = 275;
            //}
            //else
            //{
            //    entryView.style.height = 550;
            //}


        }

        void SetUpListView()
        {
            entryView = GetComponent<PanelScreen>().HistoryPanel.Q<ListView>("HistoryListView");

            FixListViewScrollingBug(entryView);
            entryView.Q<ScrollView>().scrollDecelerationRate = 0.0035f;

            var visualTree = Resources.Load<VisualTreeAsset>("SessionLogTemplate");
            Func<VisualElement> makeItem = () => visualTree.Instantiate();
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                // set map name
                //e.Q<Label>("MapName").text = SessionLogList[i].Session.MapId.ToString();
                string mapName = (WorldDatas.SelectMany(w => w.MapLists).FirstOrDefault(m => m.Id == SessionLogList[i].Session.MapId).MapName).ToString();
                e.Q<Label>("MapName").text = mapName;
                // Get Player Best Submit from each Session
                SubmitHistory mapBestSubmit = SubmitHistory.GetBestSubmit(SessionLogList[i].Session.SubmitHistories);

                if (mapBestSubmit != null)
                {
                    if (mapBestSubmit.IsCompleted)
                    {
                        e.Q($"RuleStar{1}").style.backgroundImage = new StyleBackground(mapBestSubmit.RuleHistories[0].IsPass ? RuleComplete : RuleIncomplete);
                        e.Q($"RuleStar{2}").style.backgroundImage = new StyleBackground(mapBestSubmit.RuleHistories[1].IsPass ? RuleComplete : RuleIncomplete);
                        e.Q($"RuleStar{3}").style.backgroundImage = new StyleBackground(mapBestSubmit.RuleHistories[2].IsPass ? RuleComplete : RuleIncomplete);
                        e.Q("CommandMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[mapBestSubmit.CommandMedal];
                        e.Q("ActionMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[mapBestSubmit.ActionMedal];
                    }
                    else
                    {
                        e.Q($"RuleStar{1}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                        e.Q($"RuleStar{2}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                        e.Q($"RuleStar{3}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                        e.Q("CommandMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                        e.Q("ActionMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                    }

                }
                else
                {
                    Debug.Log("i think this line should not happen");
                    e.Q($"RuleStar{1}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    e.Q($"RuleStar{2}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    e.Q($"RuleStar{3}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    e.Q("CommandMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                    e.Q("ActionMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                }

                e.Q<Label>("SessionPlayDateValue").text = SessionLogList[i].Session.StartDatetime.ToString("g");

                string timeDifference = GetTimeDifference(SessionLogList[i].Session.StartDatetime, SessionLogList[i].Session.EndDatetime);
                e.Q<Label>("SessionDurationValue").text = timeDifference;
            };

            entryView.makeItem = makeItem;
            entryView.itemsSource = SessionLogList;
            entryView.bindItem = bindItem;
        }

        public string GetTimeDifference(DateTime start, DateTime end)
        {
            TimeSpan diff = end - start;
            int minutes = (int)diff.TotalMinutes;
            int seconds = diff.Seconds;
            return $"{minutes} minutes {seconds} seconds";
        }

        public void OnOpenHistoryPanel()
        {
            if (WorldDatas == null || WorldDatas.Count == 0)
            {
                LoadMapFromFile();
            }

            //CreateDropDownMenu();
            GenerateSessionLog();
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
        //public static void FixListViewScrollingBug(ListView listView)
        //{

        //    var scroller = listView.Q<Scroller>();
        //    float scrollSpeed = 2000f; // Adjust this value to control the scrolling speed
        //    float maxScrollDelta = 100f; // Adjust this value to control the maximum amount of scrolling per frame

        //    listView.RegisterCallback<WheelEvent>(@event =>
        //    {
        //        float delta = @event.delta.y * scrollSpeed;
        //        float targetValue = Mathf.Clamp(scroller.value + delta, 0f, scroller.scrollableSize);

        //        float scrollDelta = Mathf.Clamp(targetValue - scroller.value, -maxScrollDelta, maxScrollDelta);

        //        scroller.ScrollTo(scroller.value + scrollDelta);
        //        @event.StopPropagation();
        //    });
        //}


    }
}
