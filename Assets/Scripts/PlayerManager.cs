﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public new string name;
    [SerializeField] PlayerStatusSO PlayerStatusSO = default;

    public BattleManager battleManager;

    //ユニット自身のパラメータ
    public int str;
    public int dex;
    public int agi;
    public int vit;
    public int men;

    //装備の影響等を計算して得られるパラメータ
    public int wt;
    public int hp;
    public int atk;
    public int def;
    public int weight;
    public int mob;

    //装備
    WeaponSO weapon;
    WeaponSO subWeapon;
    ShieldSO shild;
    ArmorSO armor;


    public Tilemap tilemap;
    private Vector3Int currentPosition;
    public Vector3 destination;//ワールド座標
    public Vector3Int destinationC;//セル座標
    public bool reachDestination = true;
    public bool continueMoving = false;

    public GameObject hitEffect;

    private void Start()
    {
        LoadStatus();
    }

    void LoadStatus()
    {
        weapon = PlayerStatusSO.runtimeWeapon;
        subWeapon = PlayerStatusSO.runtimeSubWeapon;
        shild = PlayerStatusSO.runtimeShield;
        armor = PlayerStatusSO.runtimeArmor;
//装備ごとの重量などはweapon.weightなどで取得できる
        
        str = PlayerStatusSO.runtimeStr;
        dex = PlayerStatusSO.runtimeDex;
        agi = PlayerStatusSO.runtimeAgi;
        vit = PlayerStatusSO.runtimeVit;
        men = PlayerStatusSO.runtimeMen;

        hp = PlayerStatusSO.runtimeHp;
        if (hp < 10) { hp = 10; }

        atk = (int)(str + weapon.atk * (1 + dex / 100));
        def = vit + shild.def + armor.def;
        weight = weapon.weight + subWeapon.weight + shild.weight + armor.weight;
        mob = (int)(agi/33+ Mathf.Log(agi*1.5f) - weight * 10 / (str + 1));//暫定的な式
        if (mob < 1) { mob = 1; }
    }

    public void MovePlayerPosition(Vector3 clickedPosition)//タイルマップから呼び出す
    {
    
        if (battleManager.ClickedMoveButton)
        {
            //if障害物があって移動できないマスをクリックした時は無視
            battleManager.ClickedMoveButton = false;
            battleManager.commandButtons.SetActive(false);
            reachDestination = false;//これがtrueになったら移動終了
            destinationC = tilemap.WorldToCell(clickedPosition);//目的地のワールド座標
            destination = tilemap.CellToWorld(destinationC);//目的地のセル座標
            currentPosition = tilemap.WorldToCell(transform.position);//ユニットの現在位置ワールド座標
            StopAllCoroutines();//コルーチンを止めておかないと移動押すたびに重ねて移動しようとしてしまう
            StartCoroutine(Moving(destination));
        }
    }

    public IEnumerator Moving(Vector3 destination)
     {
        int moveCount = 0;
        while (reachDestination == false&&moveCount<mob)//目的地に着くか、移動力mobの回数動くかするまで繰り返す
        {
            moveCount++;
            yield return new WaitForSeconds(0.1f);

            Vector3Int beforePosition = tilemap.WorldToCell(transform.position);//移動前のセル座標をメモ
            MovePlayer();
            currentPosition = tilemap.WorldToCell(transform.position);//移動後のセル座標をメモ
            battleManager.UpdateMap(beforePosition,currentPosition , 1);//移動前後のマスのmap状態を更新


            if (Vector3.Distance(destination, transform.position) < Mathf.Epsilon)
            {
                reachDestination = true;
            }
        }
        battleManager.playerDone = true;
        if (reachDestination == false)//目的地に着いていれば移動継続は無し、着いていなければ移動継続あり
        {
            continueMoving = true;
        }
        else
        {
            continueMoving = false;
        }
    }

    //目的地が6方向のいずれかを判断する
    void MovePlayer()
    {
        Vector3 diff = destination - transform.position;//差分はセル座標の差分を用いる
//        Debug.Log("diffは"+diff);
        if (diff.y == 0)//y座標の差が０の時、水平方向に移動
        {
            if (diff.x > 0)
            {
                MoveOnTile(Direction.Right);
            }
            else
            {
                MoveOnTile(Direction.Left);
            }
        }
        else if (diff.y > 0)
        {
            if (diff.x > 0 || diff.x == 0 && currentPosition.x <= 4)
            {
                MoveOnTile(Direction.UpRight);
            }
            else if (diff.x < 0 || diff.x == 0 && currentPosition.x > 4)
            {
                MoveOnTile(Direction.UpLeft);
            }            
        }
        else if (diff.y < 0)
        {
            if (diff.x > 0 || diff.x == 0 && currentPosition.x <= 4)
            {
                MoveOnTile(Direction.DownRight);
            }
            else if (diff.x < 0 || diff.x == 0 && currentPosition.x > 4)
            {
                MoveOnTile(Direction.DownLeft);
            }
        }
//            Debug.Log("目的地" + destination + "に未着。現在地は" + currentPosition);//ここがなぜか0.0.0
//            Debug.Log("目的地まで" + Vector3.Distance(destination, transform.position));
            return;
    }

    enum Direction
    {
        UpRight,
        Right,
        DownRight,
        DownLeft,
        Left,
        UpLeft,
    }

    void MoveOnTile(Direction direction)//６方向への動き方を定義している。y座標の偶奇で処理が異なる。
    {
        Vector3Int tablePos = tilemap.WorldToCell(transform.position);
        switch (direction)
        {
            case Direction.UpRight://右上に移動
                if (tablePos.y % 2 == 0)//移動前のy座標が偶数の時はy++、奇数の時はx++y++  
                {
                    tablePos.y++;
                }
                else
                {
                    tablePos.x++;
                    tablePos.y++;
                }
                break;
            case Direction.Right:
                tablePos.x++;
                break;
            case Direction.DownRight:
                if (tablePos.y % 2 == 0)
                {
                    tablePos.y--;
                }
                else
                {
                    tablePos.x++;
                    tablePos.y--;
                }
                break;
            case Direction.DownLeft:
                if (tablePos.y % 2 == 0)
                {
                    tablePos.x--;
                    tablePos.y--;
                }
                else
                {
                    tablePos.y--;
                }
                break;
            case Direction.Left:
                tablePos.x--;
                break;
            case Direction.UpLeft:
                if (tablePos.y % 2 == 0)
                {
                    tablePos.x--;
                    tablePos.y++;
                }
                else
                {
                    tablePos.y++;
                }
                break;
        }
        if (BattleManager.instance.IsWallorObj(tablePos))
        {
            reachDestination = true;
            Debug.Log("！目的地が進入不可能なので移動を終了しました");
        }
        else
        {
            transform.position = tilemap.GetCellCenterWorld(tablePos);
        }

    }
}
