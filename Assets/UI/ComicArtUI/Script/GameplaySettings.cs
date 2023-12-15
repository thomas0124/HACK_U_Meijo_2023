using TMPro;
using UnityEngine;

namespace ComicUI
{
    public class GameplaySettings : MonoBehaviour
    {
        public string[] IndicatorOptions;
        public TextMeshProUGUI SelectedIndicator;
        public int CurrentSelectedIndex;

        public void OnArrowClick(int value)
        {
            CurrentSelectedIndex += value;

            if (CurrentSelectedIndex >= IndicatorOptions.Length)
            {
                CurrentSelectedIndex = 0;
            }
            else if (CurrentSelectedIndex < 0)
            {
                CurrentSelectedIndex = IndicatorOptions.Length - 1;
            }

            SelectedIndicator.text = IndicatorOptions[CurrentSelectedIndex];
        }

        public void SetIndicator(int index)
        {
            SelectedIndicator.text = IndicatorOptions[index];
            Debug.Log($"Received index {index} and set text {IndicatorOptions[index]}");
        }
    }
}
