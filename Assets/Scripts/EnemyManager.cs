using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    public Tilemap tilemap = default;

    public new string name;
    public PlayerManager player;

    //ユニットのパラメータ
    public int str;
    public int dex;
    public int agi;
    public int vit;
    public int men;
    //enemyは以下の値も計算ではなく既定値を用いる
    public int wt;
    public int hp;
    public int atk;
    public int def;
    public int mob;
    //的に固有の変数
    public int exp;
    public int gold;
    public int fame;
    //行動済みフラグ
    public bool done = false;


    //初期化
    public void Init(EnemyData enemyData)//データベースから読み出す（これをenemy0などの変数に入れ込む）
    {
        name = enemyData.name;
        str = enemyData.str;
        dex = enemyData.dex;
        agi = enemyData.agi;
        vit = enemyData.vit;
        men = enemyData.men;
        wt = 0;
        hp = enemyData.hp;
        atk = enemyData.atk;
        def = enemyData.def;
        mob = enemyData.mob;
        GetComponent<SpriteRenderer>().sprite = enemyData.sprite;    //画像を変える
        exp = enemyData.exp;
        gold = enemyData.gold;
        fame = enemyData.fame;
        
    }

    public void SetTarget(PlayerManager player)
    {
        this.player = player;
        
    }

    public void SetTileMap(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public void OnClickEnemy() //敵をクリックした際、フラグによってダメージの処理が異なる（近接攻撃、魔法、投擲、ボウ）
    {
        //近接攻撃
        if (BattleManager.instance.ClickedAttackButton == true)
        {
            //攻撃の処理
            if (Vector3.Distance(player.transform.position, transform.position) <= 1)
            {
                int hit = 70 + player.dex - agi;
                float rundomNumber = Random.Range(0f, 100f);
                //Debug.Log($"hit={hit}/rundomNumber={(int)rundomNumber}");
                if (rundomNumber < hit)
                {
                    float damage = ((float)Random.Range(player.atk * player.dex / 100,(float)player.atk) - this.def) / 5;
                    if (damage < 0) { damage = 0; }
                    this.hp -= (int)damage;
                    if (hp <= 0)
                    {
                        hp = 0;
                        Vector3Int lastPos = tilemap.WorldToCell(transform.position);
                        BattleManager.instance.map[lastPos.x, lastPos.y] = 0;
                        Destroy(this.gameObject);
                        BattleManager.instance.RemoveEnemy(this, exp, gold, fame);
                        wt = 0;
                        //exp,gold,fame値のプール加算
                    }
                    Debug.Log((int)damage + "ポイントのヒット！敵の残りHPは" + hp);
                }
                else
                {
                    Debug.Log($"ミス！");
                }

                //Playerを行動済みの状態にする
                BattleManager.instance.ClickedAttackButton = false;
                BattleManager.instance.commandButtons.SetActive(false);
                BattleManager.instance.playerDone = true;
            }
        }
        if (BattleManager.instance.ClickedMagicButton) { }
        if (BattleManager.instance.ClickedBowButton) { }
        if (BattleManager.instance.ClickedThrowButton) { }
    }

    public void MoveTo(Vector3 targetPositionWorld)
    {
        Vector3Int beforePosition = tilemap.WorldToCell(transform.position);

        Vector2 diff = targetPositionWorld - transform.position;//差分はワールド座標を用いる。普通はtargetPosition=player.transform.positionだが、将来的に逃げるAIも用意するのでtargetPositionを用意しています
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
            if (diff.x > 0)
            {
                MoveOnTile(Direction.UpRight);
            }
            else
            {
                MoveOnTile(Direction.UpLeft);
            }
        }
        else if (diff.y < 0)
        {
            if (diff.x > 0)
            {
                MoveOnTile(Direction.DownRight);
            }
            else
            {
                MoveOnTile(Direction.DownLeft);
            }
        }
        Vector3Int currentPosition = tilemap.WorldToCell(transform.position);
        BattleManager.instance.UpdateMap(beforePosition, currentPosition, 2);
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
            return;
        }
        else
        {
            transform.position = tilemap.GetCellCenterWorld(tablePos);
        }
    }

    public void DebugRemoveEnemy()
    {
        BattleManager.instance.RemoveEnemy(this, exp, gold, fame);
    }
}
