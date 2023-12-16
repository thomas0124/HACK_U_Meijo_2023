/*
バトルシーンの管理
自身のステータスー>Characterクラス , 相手のステータス->nowEnemyStatus


SetStatus(int h, int a, int b, int c, int d, int s, int element, int[] skill_id)
を外部から設定して自分のキャラクターのステータスを設定できます。 skill_id はCSVファイルで管理してるスキルのID
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class BattleController : MonoBehaviourPunCallbacks
{
    [HideInInspector] public static bool turn;//ターン判別：true->Ally ,false->Enemy
    public int roulettResult;
    private bool isStop = false;
    private bool isStart = false;
    private bool isDisplayPanel = false;
    private int damage;//与ダメージ
    private int resultValue;
    private List<Skills.SKILL> skills;//CSVから読み込んだスキル用の構造体リストを格納
    private int Action;
    private bool conf = false; //接続確認用
    private bool finishTakeDamage = false;//ダメージを反映したことを知らせるフラグ

    private Skills sk;
    public UIController ui;
    public CardInInventory Ally;
    private AfterKariLoading loading;

    public Image Ally_image;
    private Sprite creature;
    private int attribute;
    private int hp;
    private int atk;
    private int magatk;
    private int def;
    private int magdef;
    private int speed;
    private int[] skill1 = new int[] { 0, 0, 0, 0 };

    public Image Enemy_image;
    private Sprite Enemy_creature;
    private int Enemy_attribute;
    private int Enemy_hp;
    private int Enemy_atk;
    private int Enemy_magatk;
    private int Enemy_def;
    private int Enemy_magdef;
    private int Enemy_speed;
    private int[] Enemy_skill1;
    bool getEnemyStatus = false;
    bool getEnemySprite = false;

    public GameObject roulettPanel;
    public Slider hpSlider_A;
    public Slider hpSlider_E;
    public Text hpText_A;
    public GameObject StartButton;
    public GameObject StopButton;
    public Text[] roulettTexts;
    public Text TurnObj;//ターン表示用
    public AfterKariLoading afterKariLoading;

    private void Awake()
    {
        //SpriteSerializer.Register();
    }
    private void Start()
    {
        StartCoroutine(StartCor());
    }
    IEnumerator StartCor()
    {
        Debug.Log("<StartCor>");

        yield return new WaitUntil(() => afterKariLoading.eComp);

        Debug.Log("finished afterKariLoading.eComp");

        creature = Ally.cardStatusList[Ally.SelectCardId].creature;
        attribute = Ally.cardStatusList[Ally.SelectCardId].attribute;
        hp = Ally.cardStatusList[Ally.SelectCardId].hp;
        atk = Ally.cardStatusList[Ally.SelectCardId].atk;
        magatk = Ally.cardStatusList[Ally.SelectCardId].magatk;
        def = Ally.cardStatusList[Ally.SelectCardId].def;
        magdef = Ally.cardStatusList[Ally.SelectCardId].magdef;
        speed = Ally.cardStatusList[Ally.SelectCardId].speed;
        for (int i = 0; i < Ally.cardStatusList[Ally.SelectCardId].skill1.Length; i++)
        {
            skill1[i] = Ally.cardStatusList[Ally.SelectCardId].skill1[i];
        }

        Debug.Log("Ally data loaded");

        StartCoroutine(enemyStatus());

        TurnObj.gameObject.SetActive(false);
        SendHiddenPanel();//ルーレットパネル非表示

        yield return new WaitUntil(() => getEnemyStatus);
        yield return new WaitForSeconds(1.0f);
        getEnemyStatus = false;

        Debug.Log("Enemy data loaded");

        //SendSprite(creature);
        //yield return new WaitUntil(() => getEnemySprite);

        Debug.Log("Send EnemySprite");

        //画像を設定する
        Ally_image.sprite = creature;
        Enemy_image.sprite = Ally.enemySprite;

        //HPバーの最大値を設定
        hpSlider_A.maxValue = hp;
        hpSlider_E.maxValue = Enemy_hp;

        //SKILL構造体のcsvファイルを読み込む
        sk = new Skills();
        skills = sk.SKILL_read_csv("skill");

        //初めのターンがどちらになるか判断
        if (speed > Enemy_speed)
        {
            turn = true;//初手は自分のターン
        }
        else if(speed < Enemy_speed){
            turn = false;//初手は相手のターン
        }
        else
        {
            if (PhotonNetwork.IsMasterClient == true) 
            {
                turn = true;
            }
            else {
                turn = false;
            }
        }

        if(turn) Debug.Log("first My turn");
        else Debug.Log("first Enemy turn");

        //hpの初期化
        ChangeHPber(hp ,Enemy_hp);

        StartCoroutine(BattleCoroutine());//バトルのコルーチン起動

        Debug.Log("</StartCor>");
    }

    public IEnumerator enemyStatus()
    {
        Debug.Log("<enemyStatus>");

        SendCharacterStatus(attribute, hp, atk, magatk, def, magdef, speed, skill1);//相手側のEnemyにAllyのステータスが反映される
        yield return new WaitUntil(() => getEnemyStatus);
        Debug.Log("[Ally]" + hp + "," + atk + "," + def + "," + magatk + "," + magdef + "," + speed + "," + (int)attribute);
        Debug.Log("[Enemy]" + Enemy_hp + "," + Enemy_atk + "," + Enemy_def + "," + Enemy_magatk + "," + Enemy_magdef + "," + Enemy_speed + "," + (int)Enemy_attribute);
        
        Debug.Log("</enemyStatus>");
    }

    //バトルコルーチン本体
    public IEnumerator BattleCoroutine()
    {
        //?スタートボタンとストップボタンを非表示
        StartButton.SetActive(false);
        StopButton.SetActive(false);

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 2; j++){

                Debug.Log("<BattleCoroutine>");

                if(roulettPanel.activeSelf == false) SendHiddenPanel();
                yield return new WaitForSeconds(2.0f);
                //ターン表示
                yield return StartCoroutine(TurnText(turn));

                //ルーレットの状態を初期化
                isStart = false;
                isStop = false;

                if(turn  == true)
                {

                    if(isDead(hp)) SendChangeScene();
                    ChangeRoulettText(skill1);
                    Debug.Log("自分のターン");

                    yield return StartCoroutine(AllyTurn());

                    SendChangeTurn();
                }
                else
                {
                    //?スタートボタンとストップボタンを非表示
                    StartButton.SetActive(false);
                    StopButton.SetActive(false);

                    Debug.Log("相手のターン");
                    //SendStartCoroutine(); //相手の方でコルーチン起動
                    yield return new WaitUntil(() => turn);

                }
            }
        }
        SendChangeScene();
    }

    private IEnumerator AllyTurn()
    {
        Debug.Log("<AllyTurn>");

        SendDisplayPanel();                                 //*ルーレットパネル表示

        yield return new WaitForSeconds(1f);                //?1秒待機

        StartButton.SetActive(true);                        //*スタートボタン表示

        yield return new WaitUntil(() => conf);             //!相手側の処理が終わるまで待機
        conf = false;

        yield return new WaitUntil(() => isStart);          //*スタートボタンが押されるまで待機（スタートボタン非表示、ストップボタン表示）
        isStart = false;

        yield return new WaitUntil(() => conf);             //!相手側の処理が終わるまで待機
        conf = false;

        yield return new WaitForSeconds(1f);                //?1秒待機

        yield return new WaitUntil(() => isStop);           //*ストップボタンが押されるまで待機（ストップボタン非表示）
        isStop = false;

        StopButton.SetActive(false);

        yield return new WaitUntil(() => conf);             //!相手側の処理が終わるまで待機
        conf = false;

        yield return new WaitForSeconds(6f);                //?6秒待機

        setAction(ui.resultValue);
        
        SendHiddenPanel();                                  //*ルーレットのパネル非表示

        yield return new WaitUntil(() => conf);             //!相手側の処理が終わるまで待機
        conf = false;

        calDamage();                                        //*ダメージを計算,ダメージを与える

        yield return new WaitUntil(() => finishTakeDamage); //*ダメージを反映するまで待機
        finishTakeDamage = false;

        yield return new WaitForSeconds(1f);                //?1秒待機

        Debug.Log("</AllyTurn>");

        yield break;
    }

//Updateで処理の終了を監視する
    void FixedUpdate()
    {
        if(turn == true) return; //自分のターン中なら処理しない

        if(roulettPanel.activeSelf == true && isDisplayPanel == false) {//ルーレットパネルがアクティブ状態か調べる
            SendConf();
            isDisplayPanel = true;
        }

        if(roulettPanel.activeSelf == false && isDisplayPanel == true) {//ルーレットパネルが非アクティブ状態か調べる
            SendConf();
            isDisplayPanel = false;
        }
        
        if(isStart) SendConf();//ルーレットが開始されたとき

        if(isStop) SendConf();//ルーレットが停止した時

    }

//相手の方でコルーチン開始
    public void SendStartCoroutine()
    {
        photonView.RPC(nameof(RPCStartCoroutine), RpcTarget.All);
    }
    [PunRPC]
    private void RPCStartCoroutine()
    {
        StartCoroutine(AllyTurn());
    }

//RPCの処理が自分側で終わったことを通知する
    public void SendConf()
    {
        photonView.RPC(nameof(RPCSendConf), RpcTarget.Others);
    }
    [PunRPC]
    private void RPCSendConf()
    {
        this.conf = true;
    }

//アクションを記憶
    public void setAction(int action)
    {
        this.Action = action;
    }
//ルーレットパネル表示
    public void SendDisplayPanel()
    {
        photonView.RPC(nameof(RPCDisplayPanel), RpcTarget.All);
    }
    [PunRPC]
    private void RPCDisplayPanel()
    {
        ui.DisplayPanel(roulettPanel);
    }

//ルーレットパネル非表示
    public void SendHiddenPanel()
    {
        photonView.RPC(nameof(RPCHiddenPanel), RpcTarget.All);
    }
    [PunRPC]
    private void RPCHiddenPanel()
    {
        ui.HiddenPanel(roulettPanel);
    }

//HPバーの表記を共有
    public void ChangeHPber(int ally_h, int enemy_h)
    {
        photonView.RPC(nameof(RPCChangeHPber), RpcTarget.Others, ally_h, enemy_h);
    }
    [PunRPC]
    private void RPCChangeHPber(int ally_h, int enemy_h)
    {
        ui.ChangeHPSlider(enemy_h, ally_h, hpSlider_A, hpSlider_E, hpText_A);
        Debug.LogFormat("Ally.HP = {0} , Enemy.HP = {1}", enemy_h, ally_h);
    }
//属性相性を判別 : 入力　(攻撃属性,受ける属性)、返り値は攻撃倍率
    public double Compatibility(int e1, int e2)
    {
        //有効属性->１．5倍　, 無効属性or関係なしor属性が同じ->1倍

        //両方が火、水、草の場合
        if(e1 <= 2 && e2 <= 2){
            double tmp = (e1 - e2 + 3) % 3;
            switch(tmp)
            {
                case 2: return 1.5;//有効属性の時
                default: return 1.0;//属性が同じだった時or無効属性の時
            }
        }
        else if(e1*e2 >= 9 && e1*e2 <= 16)//両方が光、闇の場合
        {
            if(e1*e2 == 12) return 1.5;//属性が異なるときは有効属性となる
            else return 1.0;//属性が同じ場合
        }
        else//火、水、草に対して光、闇の場合(逆もあり)
        {
            return 1.0;//属性関係なし
        }
    }
//与ダメージ量計算(Ally -> Enemy)
    private void calDamage(){
        /*
        ダメージ計算式(int) : ((def or magdef ÷ 3)-(atk or magatk ÷ 2))*属性相性(1.5 or 1.0)
        atk,magatkは自身のステータス　　def,magdefは相手のステータス
        */
        
        int action = this.Action;
        double damage;//与ダメージ量
        int heal = 0;//ヒール量
        double comp;//属性相性(有効属性の時のみ1.5倍)
        int mag = 1;//スキル倍率
        Skills.SKILL skill = new Skills.SKILL();//発動するスキルを格納

        comp = Compatibility(attribute, (int)Enemy_attribute);

        if(action == 0)
        {
            Debug.Log("こうげき");
            damage = ((atk / 2) - (Enemy_def / 3)) * comp;
            if(damage <= 0) damage = 0;
        }
        else if(action <= 4 && action >= 1)
        {
            skill = skills[skill1[action - 1]];
            Debug.Log("スキル : [" + skill.name + "]");

            //スキルが回復系だった時は自身を回復して終了する
            if(skill.heal > 0) 
            {
                heal = skill.heal;

                Debug.Log("ヒール量 : " + heal);

                if((hp + heal) >= (int)hpSlider_A.maxValue) heal = 0;

                hp += heal;
                SendHP(hp, Enemy_hp);//自身のステータスを相手に再反映
                ui.ChangeHPSlider(hp, Enemy_hp, hpSlider_A, hpSlider_E, hpText_A);//自分側のHPバーを設定
                ChangeHPber(hp, Enemy_hp);//相手側のHPバーを設定
            }
            else
            {
                if(skill.special == 1)//スキルが攻撃系だった時
                {
                    mag = skill.mag;//スキルが特殊系の時

                    Debug.Log("mag : " + mag);

                    damage = ((magatk * mag / 2) - (Enemy_magdef / 3)) * comp;
                    if(damage <= 0) damage = 0;
                }
                else//スキルが物理系の時
                {
                    mag = skill.mag;

                    Debug.Log("mag : " + mag);

                    damage = ((atk * mag / 2) - (Enemy_def / 3)) * comp;
                    if(damage <= 0) damage = 0;
                }
            }
        }
        else
        {
            Debug.Log("ミス");
            damage = 0;
        }

        Debug.Log("ダメージ量 : " + damage);

        Enemy_hp -= (int)damage;
        if(Enemy_hp < 0) Enemy_hp = 0;

        SendHP(hp, Enemy_hp);//自身のステータスを相手に再反映

        ui.ChangeHPSlider(hp, Enemy_hp, hpSlider_A, hpSlider_E, hpText_A);//自分側のHPバーを設定
        ChangeHPber(hp, Enemy_hp);//相手側のHPバーを設定

        if(Enemy_hp == 0) SendChangeScene();

        finishTakeDamage = true;
    }

//ルーレットのスキル表記を変更する

    public void ChangeRoulettText(int[] skill1)
    {
        photonView.RPC(nameof(RPCChangeRoulettText), RpcTarget.All, skill1);
    }
    [PunRPC]
    private void RPCChangeRoulettText(int[] skill1)
    {
        Skills.SKILL skill = new Skills.SKILL();//発動するスキルを格納
        for (int i = 0; i < 4; i++)
        {
            skill = skills[skill1[i]];
            roulettTexts[i + 1].text = skill.name;
        }
    }

//HPが０以下になったらtrueを返す
    public bool isDead(int hp)
    {
        if(hp <= 0){
            hp = 0;
            return true;
        }else{
            return false;
        }
    }

//ターン表記を変更する
    private void ChangeTurnObj(Text turnObj)
    {
        if (turnObj.enabled)
        {
            turnObj.enabled = false;
        }
        else
        {
            turnObj.enabled = true;
        }
    }

//ルーレットの開始と停止の指示を両方に出す
    public void SendStopRoulett()
    {
        photonView.RPC(nameof(RPCOnStopButton), RpcTarget.All, null);
    }
    public void SendStartRoulett()
    {
        photonView.RPC(nameof(RPCOnStartButton), RpcTarget.All, null);
    }
    
//ルーレットの結果を共有する（resultValueを統一する）
    public void SendResultValue(int value)
    {
        photonView.RPC(nameof(RPCResultValue), RpcTarget.All, value);
    }
    [PunRPC]
    void RPCResultValue(int value)
    {
        this.roulettResult = value;
    }

//ストップボタンが押された時にコルーチンの　処理が進むようにする(RPC)
    [PunRPC]
    private void RPCOnStopButton()
    {
        this.isStop = true;
        ui.OnStopButton();

        if(turn)//押したらボタンを非表示にする
        {
            StopButton.SetActive(false);
        }
    }

    //ボタンが押された時にコルーチンの　処理が進むようにする(相手と同期)
    [PunRPC]
    private void RPCOnStartButton()
    {
        this.isStart = true;
        ui.OnStartButton();

        if(turn)//押したらスタートボタンを消してストップボタンに変える
        {
            StartButton.SetActive(false);
            StopButton.SetActive(true);
        }
    }

//ターンを変更する
    private void SendChangeTurn()
    {
        photonView.RPC(nameof(RPCChangeTurn), RpcTarget.All);
    }

    [PunRPC]
    private void RPCChangeTurn()
    {
        if(turn == true) turn = false;
        else turn = true;
    }

//相手に自分のキャラのステータスを送る
    private void SendCharacterStatus(int eattribute, int ehp, int eatk, int emagatk, int edef, int emagdef, int espeed, int[]eskill1)
    {
        photonView.RPC(nameof(RPCsetCharacter), RpcTarget.Others, eattribute, ehp, eatk, emagatk, edef, emagdef, espeed, eskill1);
    }

    [PunRPC]
    void RPCsetCharacter(int eattribute, int ehp, int eatk, int emagatk, int edef, int emagdef, int espeed, int[] eskill)
    {
        Enemy_attribute = eattribute;
        Enemy_hp = ehp;
        Enemy_atk = eatk;
        Enemy_magatk = emagatk;
        Enemy_def = edef;
        Enemy_magdef = emagdef;
        Enemy_speed = espeed;
        Enemy_skill1 = eskill;
        getEnemyStatus = true;
    }

    private void SendHP(int hp, int ehp)
    {
        photonView.RPC(nameof(RPCsetCharacter), RpcTarget.Others, hp, ehp);
    }

    [PunRPC]
    void RPCsetCharacter(int ehp, int hp)
    {
        this.hp = hp;
        Enemy_hp = ehp;

        Debug.Log("[Enemy]" + Enemy_hp + "," + Enemy_atk + "," + Enemy_def + "," + Enemy_magatk + "," + Enemy_magdef + "," + Enemy_speed + "," + (int)Enemy_attribute);
    }

//スプライト送信
    private void SendSprite(Sprite sprite)
    {
        photonView.RPC(nameof(SetSprite), RpcTarget.Others, sprite);
    }
    [PunRPC]
    void SetSprite(Sprite sprite)
    {
        Debug.Log(sprite);
        Enemy_creature = sprite;
        getEnemySprite = true;
    }

//シーンを変更する
    private void SendChangeScene()
    {
        photonView.RPC(nameof(RPCChangeScene), RpcTarget.All);
        
    }
    [PunRPC]
    private void RPCChangeScene()
    {
        if(hp > Enemy_hp) turn = true;
        else turn = false;


        
        SceneManager.LoadScene("Scene8");
    }

//ターン表示のアニメーション
    private IEnumerator TurnText(bool Turn)
    {

        Debug.Log("<TurnText>");

        TurnObj.gameObject.SetActive(false);
        TurnObj.transform.localScale = new Vector3(0, 0, 0);
        TurnObj.gameObject.SetActive(true);
        TurnObj.transform.DOScale(new Vector3(1.0f, 1.0f, 0), 0.5f).SetEase(Ease.OutBounce);
        if (Turn == true) TurnObj.text = "Your Turn";
        else TurnObj.text = "Rival Turn";
        yield return new WaitForSeconds(1.2f);//0.8秒待機
        TurnObj.gameObject.SetActive(false);

        Debug.Log("</TurnText>");

        yield break;
    }
}
