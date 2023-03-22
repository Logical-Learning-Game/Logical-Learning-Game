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

        }
        void RegisterButtonCallbacks()
        {

        }

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
        public void LoadMapFromFile()
        {
            Debug.Log("SessionLog");
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
            if (gameData == null || WorldDatas == null) return;
            SessionLogList.Clear();
            //WorldData selectedWorld = WorldDatas.FirstOrDefault(w => w.WorldName == worldSelector);
            foreach (var session in gameData.SessionHistories)
            {
                SessionLogList.Add(session);
            }
            SessionLogList.Reverse();

            if (entryView != null)
            {
                entryView.Rebuild();
            }
            else
            {
                SetUpListView();
            }


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

                string mapName = "__MAPNAME__";
                if (gameData != null && (WorldDatas != null && WorldDatas.Count > 0))
                {
                    Map searchedMap = (WorldDatas.SelectMany(w => w.MapLists).FirstOrDefault(m => m.Id == SessionLogList[i]?.Session.MapId));
                    mapName = searchedMap.MapName.ToString();
                }
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
                    e.Q($"RuleStar{1}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    e.Q($"RuleStar{2}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    e.Q($"RuleStar{3}").style.backgroundImage = new StyleBackground(RuleIncomplete);
                    e.Q("CommandMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                    e.Q("ActionMedal").style.unityBackgroundImageTintColor = ColorConfig.MEDAL_COLOR[Medal.NONE];
                }

                e.Q<Label>("SessionPlayDateValue").text = SessionLogList[i].Session.StartDatetime.ToString("g");

                string timeDifference = GetTimeDifference(SessionLogList[i].Session.StartDatetime, SessionLogList[i].Session.EndDatetime);
                e.Q<Label>("SessionDurationValue").text = timeDifference;

                e.Q<Label>("TotalSubmitValue").text = SessionLogList[i].Session.SubmitHistories.Count.ToString();
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
                        scroller.value +=  @event.delta.y * 1000;
                        @event.StopPropagation();
                    });
#endif
        }



    }
}
