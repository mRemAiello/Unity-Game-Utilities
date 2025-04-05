using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Rappresenta una quest nel gioco
    /// </summary>
    [Serializable]
    public class Quest
    {
        /// <summary>
        /// ID univoco della quest
        /// </summary>
        [SerializeField] private string _id;

        /// <summary>
        /// Titolo della quest
        /// </summary>
        [SerializeField] private string _title;

        /// <summary>
        /// Descrizione della quest
        /// </summary>
        [SerializeField] private string _description;

        /// <summary>
        /// Stato attuale della quest
        /// </summary>
        [SerializeField] private QuestStatus _status;

        /// <summary>
        /// Obiettivi della quest
        /// </summary>
        [SerializeField] private List<string> _objectives;

        /// <summary>
        /// Stato attuale di ogni obiettivo (true = completato)
        /// </summary>
        [SerializeField] private List<bool> _objectivesCompleted;

        /// <summary>
        /// Ricompense per il completamento della quest
        /// </summary>
        [SerializeField] private Dictionary<string, int> _rewards;

        /// <summary>
        /// Data di inizio della quest
        /// </summary>
        [SerializeField] private DateTime _startTime;

        /// <summary>
        /// Data di completamento della quest
        /// </summary>
        [SerializeField] private DateTime _completionTime;

        /// <summary>
        /// Costruttore
        /// </summary>
        public Quest(string id, string title, string description, List<string> objectives = null, Dictionary<string, int> rewards = null)
        {
            _id = id;
            _title = title;
            _description = description;
            _status = QuestStatus.Available;
            _objectives = objectives ?? new List<string>();
            _objectivesCompleted = new List<bool>();
            _rewards = rewards ?? new Dictionary<string, int>();

            // Inizializza gli stati degli obiettivi a falso
            for (int i = 0; i < _objectives.Count; i++)
            {
                _objectivesCompleted.Add(false);
            }
        }

        // Proprietà pubbliche
        public string ID => _id;
        public string Title => _title;
        public string Description => _description;
        public QuestStatus Status 
        { 
            get => _status;
            set => _status = value;
        }
        public List<string> Objectives => _objectives;
        public List<bool> ObjectivesCompleted => _objectivesCompleted;
        public Dictionary<string, int> Rewards => _rewards;
        public DateTime StartTime => _startTime;
        public DateTime CompletionTime => _completionTime;

        /// <summary>
        /// Inizia la quest
        /// </summary>
        public void Start()
        {
            if (_status == QuestStatus.Available)
            {
                _status = QuestStatus.Active;
                _startTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Completa la quest
        /// </summary>
        public void Complete()
        {
            if (_status == QuestStatus.Active)
            {
                _status = QuestStatus.Completed;
                _completionTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Fallisci la quest
        /// </summary>
        public void Fail()
        {
            if (_status == QuestStatus.Active)
            {
                _status = QuestStatus.Failed;
                _completionTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Abbandona la quest
        /// </summary>
        public void Abandon()
        {
            if (_status == QuestStatus.Active)
            {
                _status = QuestStatus.Abandoned;
                _completionTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Segna un obiettivo come completato
        /// </summary>
        /// <param name="index">Indice dell'obiettivo</param>
        /// <returns>True se tutti gli obiettivi sono stati completati</returns>
        public bool CompleteObjective(int index)
        {
            if (index < 0 || index >= _objectivesCompleted.Count)
                return false;

            _objectivesCompleted[index] = true;

            // Controlla se tutti gli obiettivi sono stati completati
            foreach (bool completed in _objectivesCompleted)
            {
                if (!completed)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Verifica se tutti gli obiettivi sono stati completati
        /// </summary>
        /// <returns>True se tutti gli obiettivi sono stati completati</returns>
        public bool AreAllObjectivesCompleted()
        {
            foreach (bool completed in _objectivesCompleted)
            {
                if (!completed)
                    return false;
            }

            return true;
        }
    }
}