using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{

    public RoulettControll roulettControll;
    public GameObject PanelUI;

    private bool isDisplay;

    // Start is called before the first frame update
    void Start()
    {
        isDisplay = PanelUI.activeSelf;
        Debug.Log(isDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        if (isDisplay)
        {
            PanelUI.SetActive(!isDisplay);
            isDisplay = false;
        }
        else
        {
            PanelUI.SetActive(!isDisplay);
            isDisplay = true;
        }
    }
}
