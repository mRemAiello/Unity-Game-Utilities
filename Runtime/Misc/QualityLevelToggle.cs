using System;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    public class QualityLevelToggle : MonoBehaviour
    {
        [SerializeField] private int _mask;
        [SerializeField] private string[] _validLevels;

        private void Start()
        {
            UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            string currentQualityLevel = QualitySettings.names[QualitySettings.GetQualityLevel()];
            gameObject.SetActive(_validLevels.Contains(currentQualityLevel));
        }
    }
}