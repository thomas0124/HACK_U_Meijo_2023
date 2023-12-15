using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultController : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public bool turn = true;
    
    void Start()
    {
        if(BattleController.turn)
        {
            resultText.color = new Color(1.0f, 0.8f, 0.0f, 1.0f);
            resultText.text = "YOU WIN!";//自分が勝った時
        }
        else{
            resultText.color = new Color(0.1f, 0.0f, 1.0f, 1.0f);
            resultText.text = "TOU LOSE";//自分が勝った時
        }
    }

    //シーン2に移動する
    public void OnClickedButton2()
    {
        SceneManager.LoadScene("GenerationScene");
    }

    //MainSceneに移動する
    public void OnClickedButton5()
    {
        SceneManager.LoadScene("MainScene");
    }
}
