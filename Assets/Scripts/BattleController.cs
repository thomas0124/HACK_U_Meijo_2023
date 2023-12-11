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
    public static bool turn;//ターン判別：true->Ally ,false->Enemy
    private bool isStop = false;
    private bool isStart = false;
    private int damage;//与ダメージ
    private int resultValue;
    private List<Skills.SKILL> skills;//CSVから読み込んだスキル用の構造体リストを格納
    private int[] nowEnemyStatus = {0,0,0,0,0,0,0};//相手のH,A,B,C,D,S,Elementを記憶する

    private Skills sk;
    public  UIController ui;
    Character Ally;

    public GameObject roulettPanel;
    public Slider hpSlider_A;
    public Slider hpSlider_E;
    public Text hpText_A;
    public GameObject StartButton;
    public GameObject StopButton;
    public Text[] roulettTexts;
    public Text TurnObj;//ターン表示用

    //キャラクタークラス
    public class Character
    {
        public int H;//HP
        public int A;//攻撃力
        public int B;//防御
        public int C;//とくこう
        public int D;//とくぼう
        public int S;//すばやさ
        public int Element;//属性　：　0->火,　1->水,　2->草,　3->光,　4->闇
        public int Action;//ルーレットによって決まる行動の値 : 0->攻撃, 1->スキル1, 2->スキル２, 3->スキル３, 4->ミス, 5->ミス
        public int[] Skill_id = new int[3];//スキルID　: Resorces/skill.csvにあるID参照

        //引数ありのコンストラクタ
        public Character(int H, int A, int B, int C, int D, int S, int Element, int[] Skill_id){
            this.H = H;
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
            this.S = S;
            this.Element = Element;
            this.Skill_id = Skill_id;
        }
    }

    void Start()
    {

        int[] skill_id = {Random.Range(0, 25),Random.Range(0, 25),Random.Range(0, 25)};
        SetStatus(1000,300,300,300,300,300 + Random.Range(10, 100),Random.Range(0, 5),skill_id);



        //*仮でステータスを配分する=============================================
        if(Ally == null)
        {
            Debug.Log("Allyを仮生成");
            //仮でステータスを入れとく
            Ally = new Character(1000,300,300,300,300,300 + Random.Range(10, 100),Random.Range(0, 5),skill_id);
        }
        //*ここまで===========================================================*/

        sk = new Skills();
        //SKILL構造体のcsvファイルを読み込む
        skills = sk.SKILL_read_csv("skill");

        Debug.Log("<Ally>"+Ally.H+","+Ally.A+","+Ally.B+","+Ally.C+","+Ally.D+","+Ally.S+","+Ally.Element);

        TurnObj.enabled = false;
        ui.HiddenPanel(roulettPanel);//ルーレットのパネル非表示

        //初めのターンがどちらになるか判断
        if(Ally.S >= nowEnemyStatus[5])
        {
            turn = true;//初手は自分のターン
        }
        else
        {
            turn = false;//初手は相手のターン
        }

        //hpの初期化
        ChangeHPber(nowEnemyStatus);

        StartCoroutine(BattleCoroutine());//バトルのコルーチン起動
    }

    //バトルコルーチン本体
    public IEnumerator BattleCoroutine()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 2; j++){
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

                //ターン表示
                ChangeTurnObj(TurnObj);
                if(turn) TurnObj.text = "Your Turn";
                else TurnObj.text = "Rival Turn";
                yield return new WaitForSeconds(1f);//?1秒待機
                ChangeTurnObj(TurnObj);

                if(turn)
                {
                    if(isDead(Ally)) SendChangeScene();
                    ChangeRoulettText(Ally);

                    StartButton.SetActive(true);
                    StopButton.SetActive(false);

                    yield return StartCoroutine(AllyTurn());

                    SendChangeTurn();
                }
                else
                {
                    StartButton.SetActive(false);
                    StopButton.SetActive(false);

                    yield return new WaitUntil(() => turn);
                }
            }
        }
        SendChangeScene();
    }

    private IEnumerator AllyTurn()
    {
        SendDisplayPanel();                                 //*ルーレットのパネル表示

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

        setAction(Ally, ui.resultValue);

        ui.HiddenPanel(roulettPanel);                       //*ルーレットのパネル非表示

        damage = calDamage(Ally, nowEnemyStatus);           //*ダメージを計算

        nowEnemyStatus[0] -= damage;
        if(nowEnemyStatus[0] < 0) nowEnemyStatus[0] = 0;

        ChangeHPber(nowEnemyStatus);                        //*HPバーを変更する

        yield return new WaitForSeconds(1f);                //?1秒待機
        yield break;
    }

//アクションを記憶
    public void setAction(Character c, int action)
    {
        c.Action = action;
    }
//ルーレットパネル表示
    public void SendDisplayPanel()
    {
        photonView.RPC(nameof(RPCDisplayPanel), RpcTarget.All);
    }
    private void RPCDisplayPanel()
    {
        ui.DisplayPanel(roulettPanel);
    }

//HPバーの表記を共有
    public void ChangeHPber(int[] nowEnemyStatus)
    {
        photonView.RPC(nameof(RPCChangeHPber), RpcTarget.All, nowEnemyStatus);
    }
    [PunRPC]
    private void RPCChangeHPber(int[] nowEnemyStatus)
    {
        hpSlider_A.maxValue = Ally.H;
        hpSlider_E.maxValue = nowEnemyStatus[0];
        ui.ChangeAllySlider(Ally.H, hpSlider_A, hpText_A);
        ui.ChangeEnemySlider(nowEnemyStatus[0], hpSlider_E);
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
//与ダメージ量計算(c1 -> c2)
    private int calDamage(Character c1, int[] nowEnemyStatus){
        /*
        ダメージ計算式(int) : ((B or D ÷ 3)-(A or C ÷ 2))*属性相性(1.5 or 1.0)
        A,Cは自身のステータス　　B,Dは相手のステータス
        */
        
        int action = c1.Action;
        double damage;//与ダメージ量
        double comp;//属性相性(有効属性の時のみ1.5倍)
        int mag = 1;//スキル倍率
        Skills.SKILL skill = new Skills.SKILL();//発動するスキルを格納

        comp = Compatibility(c1.Element, nowEnemyStatus[6]);

        if(action == 0)
        {
            Debug.Log("こうげき");
            damage = ((c1.A / 2) - (nowEnemyStatus[2] / 3)) * comp;
        }
        else if(action <= 3 && action >= 1)
        {
            skill = skills[c1.Skill_id[action - 1]];
            Debug.Log(skill.name);
            if(skill.special == 1)
            {
                mag = skill.mag;//スキルが特殊系の時
                damage = ((c1.C * mag / 2) - (nowEnemyStatus[4] / 3)) * comp;
            } 
            else
            {
                mag = skill.mag;//スキルが物理系の時
                damage = ((c1.A * mag / 2) - (nowEnemyStatus[2] / 3)) * comp;
            }
        }
        else
        {
            Debug.Log("ミス");
            damage = 0;
        }

        return (int)damage;
    }

//ルーレットのスキル表記を変更する

    public void ChangeRoulettText(Character c)
    {
        photonView.RPC(nameof(RPCChangeRoulettText), RpcTarget.All, c);
    }
    [PunRPC]
    private void RPCChangeRoulettText(Character c)
    {
        Skills.SKILL skill = new Skills.SKILL();//発動するスキルを格納
        for(int i = 0; i < 3; i++)
        {
            skill = skills[c.Skill_id[i]];

            roulettTexts[i + 1].text = skill.name;
        }
    }

//HPが０以下になったらtrueを返す
    public bool isDead(Character c)
    {
        if(c.H <= 0){
            c.H = 0;
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
    

//ストップボタンが押された時にコルーチンの　処理が進むようにする(RPC)
    [PunRPC]
    private void RPCOnStopButton()
    {
        isStop = true;
    }

    //ボタンが押された時にコルーチンの　処理が進むようにする(相手と同期)
    [PunRPC]
    private void RPCOnStartButton()
    {
        isStart = true;
    }

//ターンを変更する
    private void SendChangeTurn()
    {
        photonView.RPC(nameof(RPCChangeTurn), RpcTarget.All);
    }

    [PunRPC]
    private void RPCChangeTurn()
    {
        if(turn) turn = false;
        else turn = true;
    }

    
//相手に自分のキャラのステータスを送る
    //相手側では送ったステータスでnowEnemyStatusを初期化
    private void SendCharacterStatus(int H, int A, int B, int C, int D, int S, int Element)
    {
        photonView.RPC(nameof(RPCsetCharacter), RpcTarget.Others, H, A, B, C, D, S, Element);
    }

    [PunRPC]
    void RPCsetCharacter(int H, int A, int B, int C, int D, int S, int Element)
    {
        nowEnemyStatus[0] = H;
        nowEnemyStatus[1] = A;
        nowEnemyStatus[2] = B;
        nowEnemyStatus[3] = C;
        nowEnemyStatus[4] = D;
        nowEnemyStatus[5] = Element;
    }

//この関数でAllyのステータスをインプットする
    public void SetStatus(int h, int a, int b, int c, int d, int s, int element, int[] skill_id)
    {
        Ally = new Character(h,a,b,c,d,s,element,skill_id);

        SendCharacterStatus(Ally.H, Ally.A, Ally.B, Ally.C, Ally.D, Ally.S, Ally.Element);//相手側のEnemyにAllyのステータスが反映される
    }
//シーンを変更する
    private void SendChangeScene()
    {
        photonView.RPC(nameof(RPCChangeScene), RpcTarget.All);
    }
    [PunRPC]
    private void RPCChangeScene()
    {
        SceneManager.LoadScene("Scene8");
    }
}
