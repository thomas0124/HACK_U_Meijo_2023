using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RouletteButton : MonoBehaviour
{

    //CommandRouletteを呼び出し
    public RoulettControll roulettecontroll;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedButton()
    {
        roulettecontroll.stopRoulett();
    }
}
