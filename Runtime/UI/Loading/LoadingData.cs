using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUtils
{
    [Serializable]
    public class LoadingData
    {
        public GameObject Container;
        public TextMeshProUGUI Text;
        public Slider ProgressBar;
        public TextMeshProUGUI ProgressText;
        public GameObject Animation;
    }
}