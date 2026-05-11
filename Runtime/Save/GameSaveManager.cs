using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Debug")]
    [DeclareBoxGroup("Save")]
    [DeclareBoxGroup("Events")]
    [DefaultExecutionOrder(-10000)]
    public class GameSaveManager : Singleton<GameSaveManager>, ILoggable
    {
        [SerializeField, Group("Save")] private bool _loadOnEnable = false;
        [SerializeField, Group("Save")] private int _minSaveSlot = 0;
        [SerializeField, Group("Save")] private int _maxSaveSlot = 5;
        [SerializeField, Group("Save")] private bool _autoSaveEnabled = false;
        [SerializeField, Group("Save"), ShowIf(nameof(_autoSaveEnabled), true)] private float _saveInterval = 5f;
        [SerializeField, Group("Save")] private SaveEncryptionMode _saveEncryptionMode = SaveEncryptionMode.None;
        [SerializeField, Group("Save"), ShowIf(nameof(IsAesEncryptionEnabled), true)] private string _encryptionPassword = "change-me";
        [SerializeField, ReadOnly, Group("Debug")] private int _currentSaveSlot;
        [SerializeField, ReadOnly, Group("Debug")] private SerializedDictionary<string, string> _dict;

        //
        private Coroutine _autoSaveCoroutine;
        private bool _isAutoSaveRunning = false;
        private SaveFileStorage _saveStorage;

        //
        protected override void OnPostAwake()
        {
            _saveStorage = new SaveFileStorage(_saveEncryptionMode != SaveEncryptionMode.None, _encryptionPassword, _saveEncryptionMode);

            //
            DebugCurrentFileSave();

            //
            if (_loadOnEnable)
            {
                LoadAll();
            }

            //
            if (_autoSaveEnabled)
            {
                StartAutoSave();
            }
        }

        public void StartAutoSave()
        {
            if (_isAutoSaveRunning)
            {
                this.LogWarning("Auto save is already running.");
                return;
            }

            _autoSaveCoroutine = StartCoroutine(AutoSaveCoroutine());
            _isAutoSaveRunning = true;
            this.Log("Auto save started.");
        }

        public void StopAutoSave()
        {
            if (!_isAutoSaveRunning)
            {
                this.LogWarning("Auto save is not running.");
                return;
            }

            if (_autoSaveCoroutine != null)
            {
                StopCoroutine(_autoSaveCoroutine);
            }
            _isAutoSaveRunning = false;
            this.Log("Auto save stopped.");
        }

        private IEnumerator AutoSaveCoroutine()
        {
            while (_isAutoSaveRunning)
            {
                yield return new WaitForSeconds(_saveInterval);
                SaveAll();
                this.Log($"Auto save executed at {System.DateTime.Now:HH:mm:ss}");
            }
        }

        [Button(ButtonSizes.Medium)]
        public void OpenPersistentDataPath()
        {
            string projectFolderName = new DirectoryInfo(Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath).Name;
            string projectPersistentDataPath = Path.Combine(Application.persistentDataPath, projectFolderName);

            // Ensure the directory exists
            if (!Directory.Exists(projectPersistentDataPath))
            {
                Directory.CreateDirectory(projectPersistentDataPath);
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(projectPersistentDataPath);
#else
            Application.OpenURL($"file://{projectPersistentDataPath}");
#endif
        }

        [Button(ButtonSizes.Medium)]
        public void SetActiveSaveSlot(int slot)
        {
            if (slot < _minSaveSlot || slot > _maxSaveSlot)
            {
                this.LogError($"Invalid save slot: {slot}. Must be between {_minSaveSlot} and {_maxSaveSlot}.");
                return;
            }

            //
            if (_currentSaveSlot == slot)
            {
                this.LogWarning($"Save slot {slot} is already active.");
                return;
            }

            //
            _currentSaveSlot = slot;
        }

        public bool Exists<T>(string context, string key)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);
            return _saveStorage.Exists(_currentSaveSlot, id);
        }

        public bool TryLoad<T>(string context, string key, out T result, T defaultValue = default)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);
            if (_saveStorage.TryRead(_currentSaveSlot, id, out result))
            {
                return true;
            }

            //
            result = defaultValue;
            return false;
        }

        public void Save<T>(string context, string key, T amount)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);

            //
            _saveStorage.Write(_currentSaveSlot, id, amount);

            //
            _dict[id] = amount.ToString();
        }

        public T Load<T>(string context, string key, T defaultValue = default)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);

            //
            if (_saveStorage.TryRead(_currentSaveSlot, id, out T result))
            {
                return result;
            }

            return defaultValue;
        }

        [Button(ButtonSizes.Medium)]
        public void SaveAll()
        {
            CheckFileSave();

            //
            var saveables = FindSceneSaveables(true);
            foreach (var saveable in saveables)
            {
                object state = saveable.CaptureState();
                string json = JsonUtility.ToJson(state);

                //
                var id = GetID<object>(saveable.SaveContext, saveable.GetType().Name);
                _saveStorage.Write(_currentSaveSlot, id, json);
            }

            //
            DebugCurrentFileSave();
        }

        [Button(ButtonSizes.Medium)]
        public void LoadAll()
        {
            DebugCurrentFileSave();

            //
            var saveables = FindSceneSaveables(true);
            foreach (var saveable in saveables)
            {
                var id = GetID<object>(saveable.SaveContext, saveable.GetType().Name);
                if (_saveStorage.TryRead(_currentSaveSlot, id, out string json))
                {
                    var state = JsonUtility.FromJson(json, saveable.GetType());
                    saveable.RestoreState(state);
                }
            }
        }

        public void RemoveKey<T>(string context, string key)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);

            //
            if (_saveStorage.Delete(_currentSaveSlot, id))
            {
                _dict.Remove(id);
            }
        }

        [Button(ButtonSizes.Medium)]
        private void DebugCurrentFileSave()
        {
            CheckFileSave();

            //
            var saveKeys = _saveStorage.GetAllKeys(_currentSaveSlot);

            //
            _dict.Clear();
            foreach (var key in saveKeys)
            {
                if (_dict.ContainsKey(key))
                    continue;

                //
                if (_saveStorage.TryRead(_currentSaveSlot, key, out JObject jObj))
                {
                    _dict.Add(key, CleanJObjectString(jObj.ToString(Formatting.None)));
                }
                else
                {
                    if (_saveStorage.TryRead(_currentSaveSlot, key, out object obj))
                    {
                        _dict.Add(key, obj.ToString());
                    }
                }
            }
        }

        [Button(ButtonSizes.Medium)]
        public void Clear()
        {
            CheckFileSave();

            //
            _saveStorage.Clear(_currentSaveSlot);

            //
            _dict.Clear();
        }

        private void CheckFileSave()
        {
            _saveStorage ??= new SaveFileStorage(_saveEncryptionMode != SaveEncryptionMode.None, _encryptionPassword, _saveEncryptionMode);

            _saveStorage.EnsureRoot(_currentSaveSlot);
        }

        private IEnumerable<ISaveable> FindSceneSaveables(bool includeInactive)
        {
            FindObjectsInactive findInactive = includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            return FindObjectsByType<MonoBehaviour>(findInactive).OfType<ISaveable>().Distinct();
        }

        private string CleanJObjectString(string original)
        {
            var cleaned = original.Replace("{", "");
            cleaned = cleaned.Replace("}", "");
            cleaned = cleaned.Replace(":", ": ");
            cleaned = cleaned.Replace(",", ", ");
            cleaned = cleaned.Replace("\"", "");

            return cleaned;
        }

        //
        private bool IsAesEncryptionEnabled => _saveEncryptionMode == SaveEncryptionMode.Aes;
        protected virtual string GetID<T>(string context, string key) => $"{context}-{key}-{typeof(T).Name}";
        public IReadOnlyList<string> GetKeys() => _dict.Keys.ToList();
        public int GetActiveSaveSlot() => _currentSaveSlot;
    }
}