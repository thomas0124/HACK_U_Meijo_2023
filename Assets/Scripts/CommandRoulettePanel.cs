using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRoulettePanel : MonoBehaviour
{
    public RoulettControll roulettControll;

    //自身がアクティブになったらルーレットを開始する
    private void OnEnable()
    {
        roulettControll.StartRoulett();
    }
}
