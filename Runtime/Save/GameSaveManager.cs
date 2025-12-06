using CI.QuickSave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("save", Title = "Save")]
    [DeclareBoxGroup("events", Title = "Events")]
    [DefaultExecutionOrder(-200)]
    public class GameSaveManager : Singleton<GameSaveManager>, ILoggable
    {
        [SerializeField, Group("save")] private bool _logEnabled = true;
        [SerializeField, Group("save")] private bool _loadOnEnable = false;
        [SerializeField, Group("save")] private int _minSaveSlot = 0;
        [SerializeField, Group("save")] private int _maxSaveSlot = 5;
        [SerializeField, ReadOnly, Group("debug")] private int _currentSaveSlot;
        [SerializeField, ReadOnly, Group("debug")] private SerializedDictionary<string, string> _dict;

        public bool LogEnabled => _logEnabled;

        //
        protected override void OnPostAwake()
        {
            DebugCurrentFileSave();

            //
            if (_loadOnEnable)
            {
                LoadAll();
            }
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
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            var id = GetID<T>(context, key);
            if (saveReader.Exists(id))
            {
                return true;
            }

            //
            return false;
        }

        public bool TryLoad<T>(string context, string key, out T result, T defaultValue = default)
        {
            CheckFileSave();

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            var id = GetID<T>(context, key);
            if (saveReader.Exists(id))
            {
                result = saveReader.Read<T>(id);
                return true;
            }

            //
            result = defaultValue;
            return false;
        }

        public bool TryLoad<T>(ISaveable saveable, string key, out T result, T defaultValue = default)
        {
            return TryLoad(saveable.SaveContext, key, out result, defaultValue);
        }

        public void Save<T>(string context, string key, T amount)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);

            //
            var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);
            saveWriter.Write(id, amount);
            saveWriter.Commit();

            //
            _dict[id] = amount.ToString();
        }

        public T Load<T>(string context, string key, T defaultValue = default)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            if (saveReader.TryRead(id, out T result))
            {
                return result;
            }

            return defaultValue;
        }

        public void RemoveKey<T>(string context, string key)
        {
            CheckFileSave();

            //
            var id = GetID<T>(context, key);
            var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);

            //
            if (saveWriter.Exists(id))
            {
                saveWriter.Delete(id);
                saveWriter.Commit();

                //
                _dict.Remove(id);
            }
        }

        [Button(ButtonSizes.Medium)]
        private void DebugCurrentFileSave()
        {
            CheckFileSave();

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            var saveKeys = saveReader.GetAllKeys().ToList();

            //
            _dict.Clear();
            foreach (var key in saveKeys)
            {
                if (_dict.ContainsKey(key))
                    continue;

                //
                if (saveReader.TryRead(key, out JObject jObj))
                {
                    _dict.Add(key, CleanJObjectString(jObj.ToString(Formatting.None)));
                }
                else
                {
                    if (saveReader.TryRead(key, out object obj))
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
            var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);
            var saveKeys = saveWriter.GetAllKeys().ToList();
            foreach (var key in saveKeys)
            {
                saveWriter.Delete(key);
            }
            saveWriter.Commit();

            //
            _dict.Clear();
        }

        [Button(ButtonSizes.Medium)]
        public void SaveAll(bool includeInactive = true)
        {
            foreach (var saveable in FindSceneSaveables(includeInactive))
            {
                saveable.Save();
            }
        }

        [Button(ButtonSizes.Medium)]
        public void LoadAll(bool includeInactive = true)
        {
            foreach (var saveable in FindSceneSaveables(includeInactive))
            {
                saveable.Load();
            }
        }

        private void CheckFileSave()
        {
            //
            if (!QuickSaveBase.RootExists("Save" + _currentSaveSlot))
            {
                var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);
                saveWriter.Commit();
            }
        }

        private IEnumerable<ISaveable> FindSceneSaveables(bool includeInactive)
        {
            FindObjectsInactive findInactive = includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            return FindObjectsByType<MonoBehaviour>(findInactive, FindObjectsSortMode.InstanceID).OfType<ISaveable>().Distinct();
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
        public void Save<T>(ISaveable saveable, string key, T amount) => Save(saveable.SaveContext, key, amount);
        public T Load<T>(ISaveable saveable, string key, T defaultValue = default) => Load(saveable.SaveContext, key, defaultValue);
        public void RemoveKey<T>(ISaveable saveable, string key) => RemoveKey<T>(saveable.SaveContext, key);
        public bool Exists<T>(ISaveable saveable, string key) => Exists<T>(saveable.SaveContext, key);
        protected virtual string GetID<T>(string context, string key) => $"{context}-{key}-{typeof(T).Name}";
        public List<string> GetKeys() => _dict.Keys.ToList();
        public int GetActiveSaveSlot() => _currentSaveSlot;
    }
}