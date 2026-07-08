using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class DebugInfo : MonoBehaviour
    {
        [ReadOnly, TextArea(5, 20)] public string Info;
    }
}
