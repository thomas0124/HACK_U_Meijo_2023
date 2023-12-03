using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    private bool turn;//ターン判別：true->Ally ,false->Enemy
    private int damage;//与ダメージ

    private Character Ally;
    private Character Enemy;
    private UIController ui;

    public Canvas roulettCanvas;
    public Slider hpSlider_A;
    public Slider hpSlider_E;
    public Text hpText_A;

    void Start()
    {
        //キャラクターのインスタンス生成
        Ally = new Character(120,80,60,70,50,70,0);
        Enemy = new Character(100,70,70,50,80,60,1);
        ui = new UIController();

        ui.HiddenPanel(roulettCanvas);//ルーレットのパネル非表示

        //初めのターンがどちらになるか判断
        if(Ally.S >= Enemy.S)
        {
            turn = true;//初手は自分のターン
        }
        else
        {
            turn = false;//初手は相手のターン
        }

        //hpの初期化
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
                yield return new WaitForSeconds(1f);//?1秒待機
                if(turn)
                {
                    yield return StartCoroutine(AllyTurn());
                }
                else
                {
                    yield return StartCoroutine(EnemyTurn());
                }
            }
        }
        SceneManager.LoadScene("Scene8");
    }

    private IEnumerator AllyTurn()
    {
        ui.DisplayPanel(roulettCanvas);                     //*ルーレットのパネル表示
        yield return new WaitForSeconds(1f);                //?1秒待機
        yield return StartCoroutine(ui.RunRoulett());          //*ルーレットが終わるまで待機
        Ally.Action = ui.getValue();                        //*ルーレットの結果を取得
        yield return new WaitForSeconds(1f);                //?1秒待機
        ui.HiddenPanel(roulettCanvas);                      //*ルーレットのパネル非表示
        damage = calDamage(Ally, Enemy, Ally.Action);       //*ダメージを計算
        Enemy.takeDamage_E(Enemy, damage, hpSlider_E);      //*Enemyにダメージを入れる
        yield return new WaitForSeconds(1f);                //?1秒待機
        turn = !turn;
    }

    private IEnumerator EnemyTurn()
    {
        ui.DisplayPanel(roulettCanvas);                     //*ルーレットのパネル表示
        yield return new WaitForSeconds(1f);                //?1秒待機
        yield return StartCoroutine(ui.RunRoulett());          //*ルーレットが終わるまで待機
        Enemy.Action = ui.getValue();                       //*ルーレットの結果を取得
        yield return new WaitForSeconds(1f);                //?1秒待機
        ui.HiddenPanel(roulettCanvas);                      //*ルーレットのパネル非表示
        damage = calDamage(Enemy, Ally, Enemy.Action);      //*ダメージを計算
        Ally.takeDamage_A(Ally, damage, hpSlider_A, hpText_A);//*Allyにダメージを入れる
        yield return new WaitForSeconds(1f);                //?1秒待機
        turn = !turn;
    }

    //与ダメージ量計算(c1 -> c2)
    private int calDamage(Character c1, Character c2, int action){
        /*
        ダメージ計算式(int) : ((B or D ÷ 3)-(A or C ÷ 2))*属性相性(1.5 or 1.0)
        A,Cは自身のステータス　　B,Dは相手のステータス
        */

        double damage;//与ダメージ量
        double comp;//属性相性(有効属性の時のみ1.5倍)

        comp = c1.Compatibility(c1.Element, c2.Element);

        if(action == 0)
        {
            damage = ((c2.B / 3) - (c1.A / 2)) * comp;
        }
        else if(action <= 3 && action >= 1)
        {
            //スキル倍率初期化
            c1.Mag = 1;
            c1.skil(action);
            damage = ((c2.D / 3) - (c1.C / 2)) * comp * c1.Mag;
        }
        else
        {
            damage = 0;
        }

        return (int)damage;
    }
}
