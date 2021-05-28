using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] EnemyData enemyData = default;

    //データベース用のデータ、ゲーム中変更しない
    public EnemyData GetEnemy()
    {
        //ゲーム中に変更されるので、新規に作成して渡す
        return new EnemyData(enemyData);
    }
}

//敵データの雛形
[System.Serializable]//インスペクターから編集できるようになる
public class EnemyData
{
    public string name;
    public Sprite sprite;

    //ユニットのパラメータ
    public int str;
    public int dex;
    public int agi;
    public int vit;
    public int men;
    public int resistanceFire;
    public int resistanceMagic;

    //enemyは以下の値も計算ではなく既定値を用いる
    public int wt;
    public int hp;
    public int atk;
    public int def;
    public int mob;
    public int weight;
    //的に固有の変数
    public int exp;
    public int gold;
    public int fame;

    //新規作成時に実行するもの（コンストラクター）
    public EnemyData(EnemyData enemyData)
    {
        name = enemyData.name;
        sprite = enemyData.sprite;

        str = enemyData.str;
        dex = enemyData.dex;
        agi = enemyData.agi;
        vit = enemyData.vit;
        men = enemyData.men;
        resistanceFire = enemyData.resistanceFire;
        resistanceMagic = enemyData.resistanceMagic;

        wt = enemyData.wt;
        hp = enemyData.hp;
        atk = enemyData.atk;
        def = enemyData.def;
        mob = enemyData.mob;

        exp = enemyData.exp;
        gold = enemyData.gold;
        fame = enemyData.fame;
        weight = enemyData.weight;
    }
}