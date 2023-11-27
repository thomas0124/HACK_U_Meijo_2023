using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandRoulettePanel : MonoBehaviour
{
    public RoulettController roulettController;
    public GameObject panelUI;

    private bool isDisplay = false;

    //スタート時に自身のアクティブ状態を記録
    void Start()
    {
        isDisplay = panelUI.activeSelf;
    }

    //パネルを非表示にする
    public void HiddenPanel()
    {
        panelUI.SetActive(!isDisplay);
        isDisplay = false;
    }

    //パネルを表示する
    public void DisplayPanel()
    {
        panelUI.SetActive(!isDisplay);
        isDisplay = true;
    }

    //自身がアクティブになったらルーレットを開始する
    private void OnEnable()
    {
        roulettController.StartRoulett();
    }   
}
