using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultController : MonoBehaviour
{
    public Text resultText;
    
    void Start()
    {
        resultText = resultText.GetComponent<Text>();//テキストコンポーネントを取得

        if(BattleController.turn)
        {
            resultText.text = "YOU WIN!";//自分が勝った時
        }
        else{
            resultText.text = "TOU LOSE";//自分が勝った時
        }
    }

    //シーン2に移動する
    public void OnClickedButton2()
    {
        SceneManager.LoadScene("Scene2");
    }

    //MainSceneに移動する
    public void OnClickedButton5()
    {
        SceneManager.LoadScene("MainScene");
    }
}
