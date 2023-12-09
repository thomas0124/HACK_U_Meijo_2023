using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BattleController : MonoBehaviourPunCallbacks
{
    private bool turn;//ターン判別：true->Ally ,false->Enemy
    private bool isStop = false;
    private bool isStart = false;
    private int damage;//与ダメージ
    private int resultValue;

    private Character Ally;//別シーンからステータスを取得
    private Character Enemy;
    public  UIController ui;

    public GameObject roulettPanel;
    public Slider hpSlider_A;
    public Slider hpSlider_E;
    public Text hpText_A;
    public GameObject StartButton;
    public GameObject StopButton;

    

    void Start()
    {
        SetStatus();//自分と敵のステータスを設定

        Debug.Log("<Ally>"+Ally.H+","+Ally.A+","+Ally.B+","+Ally.C+","+Ally.D+","+Ally.S+","+Ally.Element+","+Ally.Action);

        Debug.Log("<Enemy>"+Enemy.H+","+Enemy.A+","+Enemy.B+","+Enemy.C+","+Enemy.D+","+Enemy.S+","+Enemy.Element+","+Enemy.Action);

        ui.HiddenPanel(roulettPanel);//ルーレットのパネル非表示

        //初めのターンがどちらになるか判断
        if(Ally.S >= Enemy.S)
        {
            turn = true;//初手は自分のターン
        }
        else
        {
            turn = false;//初手は相手のターン
        }

        Debug.Log(Ally.H);
        //hpの初期化
        hpSlider_A.maxValue = Ally.H;
        hpSlider_E.maxValue = Enemy.H;
        ui.ChangeAllySlider(Ally.H, hpSlider_A, hpText_A);
        ui.ChangeEnemySlider(Enemy.H, hpSlider_E);

        StartCoroutine(BattleCoroutine());//バトルのコルーチン起動
    }

    //バトルコルーチン本体
    public IEnumerator BattleCoroutine()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 2; j++){
                yield return new WaitForSeconds(1f);//?1秒待機(ターン切り替え時の演出入れるかも)
                if(turn)
                {

                    yield return StartCoroutine(AllyTurn());
                    if(Enemy.isDead()) SceneManager.LoadScene("Scene8");
                }
                else
                {
                    //ルーレットボタンを非表示にする
                    StartButton.SetActive(false);
                    StopButton.SetActive(false);

                    yield return StartCoroutine(EnemyTurn());
                    if(Ally.isDead()) SceneManager.LoadScene("Scene8");
                }
            }
        }
        SceneManager.LoadScene("Scene8");
    }

    private IEnumerator AllyTurn()
    {
        ui.DisplayPanel(roulettPanel);                      //*ルーレットのパネル表示

        StopButton.SetActive(false);                        //*ストップボタン非表示
        StartButton.SetActive(true);                        //*スタートボタン表示

        yield return new WaitUntil(() => isStart);          //?スタートボタンが押されるまで待機
        isStart = false;

        StartButton.SetActive(false);                       //*スタートボタンをストップボタンに切り替え
        StopButton.SetActive(true);

        yield return new WaitUntil(() => isStop);           //?ストップボタンが押されるまで待機
        isStop = false;

        StopButton.SetActive(false);                        //*ストップボタン非表示

        yield return new WaitForSeconds(6f);                //?6秒待機

        Ally.setAction(Ally, ui.resultValue);

        ui.HiddenPanel(roulettPanel);                       //*ルーレットのパネル非表示

        damage = calDamage(Ally, Enemy);                    //*ダメージを計算
        Enemy.takeDamage_E(Enemy, damage, hpSlider_E);      //*Enemyにダメージを入れる
        yield return new WaitForSeconds(1f);                //?1秒待機

        turn = !turn;
        yield break;
    }

    private IEnumerator EnemyTurn()
    {
        ui.DisplayPanel(roulettPanel);                      //*ルーレットのパネル表示

        yield return new WaitUntil(() => isStart);          //?スタートボタンが押されるまで待機
        isStart = false;

        //ここにルーレットを停止するRPC

        yield return new WaitUntil(() => isStop);           //?ストップボタンが押されるまで待機
        isStop = false; 

        yield return new WaitForSeconds(6f);                //?6秒待機

        Enemy.setAction(Enemy, ui.resultValue);

        ui.HiddenPanel(roulettPanel);                       //*ルーレットのパネル非表示

        damage = calDamage(Enemy, Ally);                    //*ダメージを計算
        Ally.takeDamage_A(Ally, damage, hpSlider_A, hpText_A);//*Allyにダメージを入れる
        yield return new WaitForSeconds(1f);                //?1秒待機

        turn = !turn;
        yield break;
    }

    //与ダメージ量計算(c1 -> c2)
    private int calDamage(Character c1, Character c2){
        /*
        ダメージ計算式(int) : ((B or D ÷ 3)-(A or C ÷ 2))*属性相性(1.5 or 1.0)
        A,Cは自身のステータス　　B,Dは相手のステータス
        */
        
        int action = c1.Action;
        double damage;//与ダメージ量
        double comp;//属性相性(有効属性の時のみ1.5倍)

        comp = c1.Compatibility(c1.Element, c2.Element);

        if(action == 0)
        {
            Debug.Log("こうげき");
            damage = ((c1.A / 2) - (c2.B / 3)) * comp;
        }
        else if(action <= 3 && action >= 1)
        {
            Debug.Log("スキル" + action);
            //スキル倍率初期化
            c1.Mag = 1;
            c1.skil(action);
            damage = ((c1.A / 2) - (c2.B / 3)) * comp * c1.Mag;
        }
        else
        {
            Debug.Log("ミス");
            damage = 0;
        }

        return (int)damage;
    }

    //自分のターンの時にルーレットを止める指示を両方に出す
    private void SendStopRoulett()
    {
        photonView.RPC(nameof(RPCOnStopButton), RpcTarget.All, null);
    }

    //自分のターンの時にルーレットを動かす指示を両方に出す
    private void SendStartRoulett()
    {
        photonView.RPC(nameof(RPCOnStartButton), RpcTarget.All, null);
    }
    

    //ボタンが押された時にコルーチンの　処理が進むようにする(相手と同期)
    [PunRPC]
    public void RPCOnStopButton()
    {
        isStop = true;
    }

    //ボタンが押された時にコルーチンの　処理が進むようにする(相手と同期)
    [PunRPC]
    public void RPCOnStartButton()
    {
        isStart = true;
    }

    //相手に自分のキャラのステータスを送る
    //相手側では送ったステータスでEnemyを初期化
    private void SendCharacterStatus(Character ally)
    {
        photonView.RPC(nameof(RPCsetCharacter), RpcTarget.Others, ally);
    }

    [PunRPC]
    public void RPCsetCharacter(Character ally)
    {
        this.Enemy = ally;
    }

    public void SetStatus()
    {
        //仮でステータスを初期化
        Ally = new Character(0,0,0,0,0,0,0,0);
        Enemy = new Character(0,0,0,0,0,0,0,0);

        SendCharacterStatus(Ally);//相手側のEnemyにAllyのステータスが反映される

    }
}
