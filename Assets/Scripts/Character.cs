using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Random;

public class Character
{
    UIController ui = new UIController();
    public int H;//HP
    public int A;//攻撃力
    public int B;//防御
    public int C;//とくこう
    public int D;//とくぼう
    public int S;//すばやさ
    public int Element;//属性　：　0->火,　1->水,　2->草,　3->光,　4->闇
    public int Action;//ルーレットによって決まる行動の値 : 0->攻撃, 1->スキル1, 2->スキル２, 3->スキル３, 4->ミス
    public double Mag = 1;//スキル倍率(初期値１)

    public Character(int H, int A, int B, int C, int D, int S, int Element){
        this.H = H;
        this.A = A;
        this.B = B;
        this.C = C;
        this.D = D;
        this.S = S;
        this.Element = Element;
    }

    /*
    public int getAction()
    {
        //ルーレットのパネル表示,ルーレットスタート
        ui.DisplayPanel();
        Action = ui.getValue();
        //ルーレットのパネル非表示
        ui.HiddenPanel();

        return Action;
    }
    */

    //Enemy -> Ally
    public void takeDamage_A(Character c, int d, Slider hpSlider_A, Text hpText_A)
    {
        c.H -= d;
        ui.ChangeAllySlider(c.H, hpSlider_A, hpText_A);
    }

    //Ally -> Enemy
    public void takeDamage_E(Character c, int d, Slider hpSlider_E)
    {
        c.H -= d;
        ui.ChangeEnemySlider(c.H, hpSlider_E);
    }

    //HPが０以下になったらtrueを返す
    public bool isDead()
    {
        if(H <= 0){
            H = 0;
            return true;
        }else{
            return false;
        }
    }

    //スキル１ スキル倍率1.2倍
    public void skil(int action)
    {
        switch(action)
        {
            case 1:
                Mag = 1.2;break;
            case 2:
                Mag = 1.2;break;
            case 3:
                Mag = 1.3;break;
        }
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

    
}
