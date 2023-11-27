using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;


public class Elements : MonoBehaviour{

    public int fire = 0;//火
    public int water = 1;//水
    public int grass = 2;//草
    public int shine = 3;//光
    public int dark = 4;//闇

    //ランダムで属性を与える
    public int getElements(){
        int randomElement = Range(0, 5);
        return randomElement;
    }

    //属性相性を判別 : 入力　(攻撃属性,受ける属性)、返り値は攻撃倍率
    public double Compatibility(int e1, int e2)
    {
        //有効属性->１．２倍　, 無効属性->0.8倍 , 関係なしor属性が同じ->1倍

        //両方が火、水、草の場合
        if(e1 <= 2 && e2 <= 2){
            double tmp = (e1 - e2 + 3) % 3;
            switch(tmp)
            {
                case 0: return 1.0;//属性が同じだった時
                case 1: return 0.8;//無効属性の時
                case 2: return 1.2;//有効属性の時
                default: return 1.0;
            }
        }
        else if(e1*e2 >= 9 && e1*e2 <= 16)//両方が光、闇の場合
        {
            if(e1*e2 == 12) return 1.2;//属性が異なるときは有効属性となる
            else return 1.0;//属性が同じ場合
        }
        else//火、水、草に対して光、闇の場合(逆もあり)
        {
            return 1.0;//属性関係なし
        }
    }
    
}