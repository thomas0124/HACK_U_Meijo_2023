using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject PanelUI;

    //パネルを非表示にする
    public void HiddenPanel()
    {
        PanelUI.SetActive(false);
    }

    //パネルを表示する
    public void DisplayPanel()
    {
        PanelUI.SetActive(true);
    }
}
