using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class EventTuple
    {
        [SerializeField, Group("Caller"), HideLabel] public GameObject CallerGameObject;
        [SerializeField, Group("Caller Script"), HideLabel] public MonoBehaviour CallerScript;
        [SerializeField, Group("Method Name"), HideLabel] public string MethodName;
    }
}