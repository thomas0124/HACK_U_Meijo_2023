using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ComicUI
{
    public class UIPopupController : MonoBehaviour
    {

        public TextMeshProUGUI messageText;
        public Button confirmButton;
        public Button cancelButton;

        // For UI Button onClick
        public void SetPopupData(string arg)
        {
            var data = arg.Split(", ");
            string message = data[0];
            string confirmText = data[1];
            string cancelText = data[2];
            SetPopupData(message, confirmText, cancelText);
        }

        public void SetPopupData(string message, string confirmText, string cancelText)
        {
            messageText.text = message.ToUpper();
            // Hide the confirm button if the text is empty
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmButton.gameObject.SetActive(false);
            }
            else
            {
                confirmButton.gameObject.SetActive(true);
                confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = confirmText;
            }

            // Hide the cancel button if the text is empty
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelButton.gameObject.SetActive(false);
            }
            else
            {
                cancelButton.gameObject.SetActive(true);
                cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = cancelText;
            }

            ShowPopup();
        }

        private void ShowPopup()
        {
            gameObject.SetActive(true);
        }

        public void HidePopup()
        {
            gameObject.SetActive(false);
        }
    }
}