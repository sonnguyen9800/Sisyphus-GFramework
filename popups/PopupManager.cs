#define LOG_OUTPUT_INFO1

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;


namespace SisyphusFramework.Popup
{
    public partial class PopupManager : BaseSingleton<PopupManager>
    {

        private struct PopupResourcesModel
        {
            public string Name;
            public PackedScene SceneResource;
        }

        private struct PopupModel
        {
            public string Name;
            public Node SceneNode;
        }

        [Export]
        private string _folderPath = "";
        [Export] private string[] _scenePaths = null;
        [Export]
        private Control popupsContainer = null;

        private List<PopupModel> _cachedPopups = new List<PopupModel>();
        private Stack<string> _queuePopupNames = new Stack<string>();


        public bool IsAnyPopupOpened
        {

            get
            {
                return _queuePopupNames.Count > 0;
            }
        }
        private ConcurrentBag<PopupResourcesModel> _popupDatabases = new ConcurrentBag<PopupResourcesModel>(); //new Lis();

        private bool _isLocked = false;
        public override void _Ready()
        {
            LoadPopupsFromFolder(_folderPath);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        #region Setup
        private string[] LoadStringsPath(string folderPath)
        {
            List<string> scenes = new List<string>();
            if (string.IsNullOrEmpty(folderPath))
                return null;

            using var dir = DirAccess.Open(folderPath);
            if (dir == null)
            {
                GD.Print("Cannot open dir");
                return null;
            }


            dir.ListDirBegin();
            var allFiles = dir.GetFiles();
            foreach (var file in allFiles)
            {
                if (!file.EndsWith(".tscn"))
                    continue;
                string str = folderPath + file;
                scenes.Add(str);
            }
            
            return scenes.ToArray();
        }

        private void LoadPopupsFromFolder(string folderPath)
        {
#if TOOLS
            _scenePaths = new string[0];
            _scenePaths = LoadStringsPath(folderPath);
#endif
            Parallel.ForEach(_scenePaths, scenePath =>
            {
                var scene = ResourceLoader.Load<PackedScene>(scenePath);
                var name = RemovePopupSuffix(scenePath);
                _popupDatabases.Add(new PopupResourcesModel
                {
                    Name = name,
                    SceneResource = scene
                });

#if LOG_OUTPUT_INFO
            GD.Print("Loaded popup: " + scenePath);
#endif
            });


        }



#endregion


        #region Public
        public bool OpenPopup(string popupName, object param = null)
        {
            if (_isLocked) return false;
            string lastestPopup = string.Empty;
            if (HasAnyActivePopup)
                lastestPopup = _queuePopupNames.Peek();
            if (popupName == lastestPopup)
                return false;
            if (lastestPopup != null)
                Hide(lastestPopup);
            _queuePopupNames.Push(popupName);
            if (!IsCached(popupName))
                CreatePopupInstance(popupName);

            Show(popupName, param);
            return true;
        }

        public void ClosePopup()
        {
            if (_queuePopupNames.Count <= 0)
                return;
            var lastestPopup = _queuePopupNames.Peek();
            Hide(lastestPopup);
            _queuePopupNames.Pop();

        }
        public void ClosePopup(string popupName){
            if (_queuePopupNames.Count <= 0)
                return;
            if (_queuePopupNames.Peek() != popupName)
                return;
            Hide(popupName);
            _queuePopupNames.Pop();

        }

        public void CloseAllPopups()
        {
            if (_queuePopupNames.Count <= 0)
                return;
            while (_queuePopupNames.Count > 0)
            {
                var lastestPopup = _queuePopupNames.Pop();
                Hide(lastestPopup);
            }
        }
        public void ToggleLocked(bool value)
        {
            _isLocked = value;
        }
        #endregion

        #region Prive
        Node CreatePopupInstance(string popupName)
        {
            var popup_scene_resources = GetPopupFromDB(popupName: popupName);
            if (popup_scene_resources == null)
            {
#if LOG_OUTPUT_ERROR
                        GD.PrintError("Popup not found: " + popupName);
#endif
                return null;
            }
            Node popupInstance = popup_scene_resources.Instantiate();
            _cachedPopups.Add(new PopupModel
            {
                Name = popupName,
                SceneNode = popupInstance
            });
            popupsContainer.AddChild(popupInstance);
            PopupBase popupBase = popupInstance as PopupBase;
            popupBase.Setup(new PopupBase.PopupParam {
                Manager = this,
                PopupName = popupName
            });
            return popupInstance;
        }

        void Show(string popupName, object param = null)
        {
            var cachePopup = GetPopupFromCache(popupName);
            if (cachePopup == null)
                return;

            cachePopup.ProcessMode = ProcessModeEnum.Always;

            var popupBase = cachePopup as PopupBase;

            popupBase.OnShowing(param);
            popupBase.Visible = true;
        }
        void Hide(string popupName)
        {
            if (!HasAnyActivePopup)
                return;
            var node = GetPopupFromCache(popupName);
            if (node == null)
                return;
            node.ProcessMode = ProcessModeEnum.Disabled;

            var popupBase = node as PopupBase;
            popupBase.OnHidden();

        }

        bool IsLastest(string popupName)
        {
            if (!HasAnyActivePopup)
                return false;

            return _queuePopupNames.Peek() == popupName;
        }

        bool IsCached(string popupName)
        {
            if (!HasAnyActivePopup)
                return false;
            return _cachedPopups.Any((e)=> {
                return e.Name == popupName;
            });
        }

        Node GetPopupFromCache(string popupName)
        {
            var cache = _cachedPopups.Find(x => x.Name == popupName);
            return cache.SceneNode;
        }
        PackedScene GetPopupFromDB(string popupName)
        {
            foreach (var i in _popupDatabases)
            {
                if (i.Name == popupName)
                {
                    return i.SceneResource;
                }
            }
            return null;
        }

        bool HasAnyActivePopup { get => _queuePopupNames.Count > 0; }
        #endregion


        #region  Utils

        private string RemovePopupSuffix(string popupScenePath)
        {
            int indexLast = popupScenePath.LastIndexOf("Popup.tscn");
            int indexFirst = popupScenePath.IndexOf(_folderPath) + _folderPath.Length;
            int length = indexLast - indexFirst;
            if (indexLast > 0 && indexFirst > 0)
            {
                string info = popupScenePath.Substring(indexFirst, length);
                return info;
            }
            return popupScenePath;
        }

        #endregion

    }
}
