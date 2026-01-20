using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("log", Title = "Log")]
    public class GameEventAssetBase : ScriptableObject, ILoggable
    {
        [SerializeField, Group("log"), PropertyOrder(0)] private bool _logEnabled = false;
        
        //
        [SerializeField, PropertyOrder(2), Group("debug"), TableList(AlwaysExpanded = true, HideAddButton = true, HideRemoveButton = true), ReadOnly] 
        private List<EventTuple> _listeners = new();
        [SerializeField, HideInInspector] private int _listenersDataVersion;

        //
        public bool LogEnabled => _logEnabled;
        public IReadOnlyList<EventTuple> Listeners => _listeners;
        protected List<EventTuple> MutableListeners => _listeners;

        // Costruisce i dati del listener con il nome dell'oggetto reale e il dettaglio script/metodo.
        protected EventTuple BuildListenerTuple(object target, string methodName)
        {
            // Recupera il riferimento Unity reale (se disponibile) insieme al fallback testuale per il debug.
            var callerReference = GetListenerObjectReference(target);
            var callerDisplay = GetListenerObjectName(target);

            // Compone il nome dello script e della funzione in un'unica stringa leggibile.
            var scriptName = GetListenerScriptName(target);
            var methodLabel = $"{scriptName} ({methodName})";

            // Ritorna la tupla con le informazioni formattate.
            return new EventTuple(callerReference, callerDisplay, methodLabel);
        }

        // Estrae il riferimento Unity serializzabile del caller quando possibile.
        private static Object GetListenerObjectReference(object target)
        {
            // Restituisce direttamente il riferimento Unity se il target lo implementa.
            if (target is Object unityObject)
            {
                return unityObject;
            }

            return null;
        }

        // Estrae il nome dell'oggetto reale associato al target del listener.
        private static string GetListenerObjectName(object target)
        {
            if (target == null)
            {
                return "Unknown";
            }

            if (target is Component component)
            {
                return component.gameObject.name;
            }

            if (target is GameObject gameObject)
            {
                return gameObject.name;
            }

            if (target is ScriptableObject scriptableObject)
            {
                return scriptableObject.name;
            }

            if (target is Object unityObject)
            {
                return unityObject.name;
            }

            return target.ToString();
        }

        // Estrae il nome dello script associato al target del listener.
        private static string GetListenerScriptName(object target)
        {
            if (target == null)
            {
                return "Static";
            }

            return target.GetType().Name;
        }

        //
        private void OnValidate()
        {
            _listeners ??= new List<EventTuple>();

            // Ripulisce una volta i dati legacy dei listener dopo il cambio del campo serializzato.
            if (_listenersDataVersion == 0)
            {
                _listeners.Clear();
                _listenersDataVersion = 1;
            }
        }
    }
}
