using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : Battler
{
    public Tilemap tilemap = default;

    public PlayerManager player;

    //敵に固有の変数
    public int exp;
    public int gold;
    public int fame;
    //行動済みフラグ
    public bool done = false;

    public Vector3Int currentCell = default;


    //初期化
    public void Init(EnemyData enemyData)//データベースから読み出す（これをenemy0などの変数に入れ込む）
    {
        unitName = enemyData.name;//これもいらんっぽい？
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
        target = player;
    }

    public void SetTileMap(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public void SetCurrentPosition()
    {
        currentCell = tilemap.WorldToCell(transform.position);

    }

    public IEnumerator StartEnemyTurn()//敵のタイプによってこの内容は異なる。
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= 1)
        {
            ExecuteDirectAttack(this, player);
        }
        else
        {
            int moveCount = 1;
            while (moveCount <= mob && Vector3.Distance(player.transform.position, transform.position) > 1)
            {
                if (player == null) { break; }
                MoveTo(tilemap.WorldToCell(player.transform.position));
                moveCount++;
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);
        done = true;
    }

    public void CheckHP()
    {
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

    }
    
    public void MoveTo(Vector3Int targetCell)//この引数は近接タイプの敵は常に「player.transform.position」になる
    {
        Vector3Int beforeCell = tilemap.WorldToCell(transform.position);

        //移動可能なマスを取得
        List<Vector3Int> AccessibleCells = GetAccessibleCells();
        //取得したマスの中で最も目的地との距離が小さいマスを求める
        Vector3Int destination = GetDestination(AccessibleCells, targetCell);
        //自分の座標に代入
        transform.position = tilemap.CellToWorld(destination);

        currentCell = tilemap.WorldToCell(transform.position);
        BattleManager.instance.UpdateMap(beforeCell, currentCell, 2);
    }

    List<Vector3Int> GetAccessibleCells()
    {
        List<Vector3Int> accessibleCells = new List<Vector3Int>();

        foreach(Vector3Int checkCell in BattleManager.instance.GetAroundCell(currentCell))
        {
            if (BattleManager.instance.IsWallorObj(checkCell))
            {
                continue;
            }
            else
            {
                accessibleCells.Add(checkCell);
            }
            accessibleCells.Add(currentCell);
        }
        return accessibleCells;
    }

    Vector3Int GetDestination(List<Vector3Int> accesibleCells,Vector3Int targetCell)
    {
        float minDistance = 999999999999;
        Vector3Int destination = new Vector3Int();
        foreach(Vector3Int searchCell in accesibleCells)
        {
            float distance = Vector3.Distance(tilemap.CellToWorld(searchCell), player.transform.position);
            if (minDistance > distance)
            {
                minDistance = distance;
                destination = searchCell;
            }
        }
        return destination;
    }
    

    /*
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
        }
    */
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
            case Direction.UpRight:
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

    public void DebugRemoveEnemy()//battleManagerのデバッグコマンドでfor文でenemies[]の中身を消していくためにこちらに記述が必要
    {
        BattleManager.instance.RemoveEnemy(this, exp, gold, fame);
    }
}
