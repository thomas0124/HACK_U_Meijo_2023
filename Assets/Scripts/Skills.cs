using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Skills
{
    public struct SKILL
    {
        public int id;//スキルID
        public string name;//スキル名
        public int mag;//倍率
        public int special;//特殊　: AかCか判別用（A:0 , C:1）
        public int heal;//回復量（回復ではないなら０）
    }

    //SKILL構造体のcsvファイルを読み込む
    public List<SKILL> SKILL_read_csv(string name)
    {
        
        SKILL skill = new SKILL();
        List<SKILL> sk_list = new List<SKILL>();

        //CSVの読み込みに必要
        TextAsset csvFile;  // CSVファイル
        List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
        int height = 0;
        int i = 0;

        csvFile = Resources.Load("CSV/" + name) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
            height++;
        }
        for (i = 1; i < height; i++)
        {
            skill.id = int.Parse(csvDatas[i][0]);
            skill.name = csvDatas[i][1];
            skill.mag = int.Parse(csvDatas[i][2]);
            skill.special = int.Parse(csvDatas[i][3]);
            skill.heal = int.Parse(csvDatas[i][4]);

            //戻り値のリストに加える
            sk_list.Add(skill);
        }
        return sk_list;
    }
}
