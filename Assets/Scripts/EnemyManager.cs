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
        resistanceFire = enemyData.resistanceFire;
        resistanceMagic = enemyData.resistanceMagic;
        
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
        switch (unitName)
        {
            case "グレムリン":
            case "ドレイク":
            case "バグ"://優先順位は　①距離1.5〜3なら炎を吐く　②距離3以上なら距離3まで接近　③距離1.5未満（接触状態）なら逃げようとして無理なら直接攻撃　　　
                if (1.5 <= Vector3.Distance(player.transform.position, this.transform.position) && Vector3.Distance(player.transform.position, this.transform.position) <= 4)//①
                {
                    ExecuteFireAttack(this, player);
                    yield return new WaitForSeconds(1f);

                }
                else if(4 < Vector3.Distance(player.transform.position, this.transform.position))//②
                {
                    int moveCount = 1;
                    while (moveCount <= mob && Vector3.Distance(player.transform.position, transform.position) > 4)
                    {
                        if (player == null) { break; }
                        MoveTo(tilemap.WorldToCell(player.transform.position));
                        moveCount++;
                        yield return new WaitForSeconds(0.1f);
                    }
                    done = true;
                }
                else//③
                {
                    int moveCount = 1;
                    Vector3 beforeposition = transform.position;
                    while (moveCount <= mob)
                    {
                        if (player == null) { break; }
                        MoveAwayFrom(tilemap.WorldToCell(player.transform.position));//移動しようとして無理やったら直接攻撃するようにする
                        moveCount++;
                        yield return new WaitForSeconds(0.1f);
                    }

                    Vector3 currentPosition = transform.position;
                    if (beforeposition == currentPosition)
                    {
                        ExecuteDirectAttack(this,player);
                        yield return new WaitForSeconds(1f);

                    }
                    done = true;
                }
                break;

            case "レッサーデーモン":
            case "グレイデビル":
            case "アークデーモン"://PlayerよりMenが高ければ魔法連射、そうでないなら接近されるまで魔法を撃って接近されたら直接攻撃
                if (Vector3.Distance(player.transform.position, this.transform.position) < 1.5)
                {
                    if (men > player.men)
                    {
                        player.magicList[1].Execute(this, player);
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        ExecuteDirectAttack(this, player);
                        yield return new WaitForSeconds(1f);
                    }
                }
                else
                {
                    player.magicList[1].Execute(this, player);
                    yield return new WaitForSeconds(1f);
                }
                break;

            case "ウィル":
            case "リッチ": //優先順位は　①距離1.5未満（接触状態）なら逃げようとして無理なら魔法攻撃　②距離1.5以上なら魔法攻撃　　　
                if (Vector3.Distance(player.transform.position, this.transform.position) <= 1.5)//①
                {
                    int moveCount = 1;
                    Vector3 beforeposition = transform.position;
                    while (moveCount <= mob)
                    {
                        if (player == null) { break; }
                        MoveAwayFrom(tilemap.WorldToCell(player.transform.position));//移動しようとして無理やったらエナジーボルトを撃つようにする
                        moveCount++;
                        yield return new WaitForSeconds(0.1f);
                    }

                    Vector3 currentPosition = transform.position;
                    if (beforeposition == currentPosition)
                    {
                        player.magicList[1].Execute(this, player);//魔法を打つのにplayerの魔法リストを借りているだけ
                        yield return new WaitForSeconds(1f);
                    }
                    done = true;
                }
                else//②
                {
                    player.magicList[1].Execute(this, player);//魔法を打つのにplayerの魔法リストを借りているだけ
                    yield return new WaitForSeconds(1f);
                }
                break;

            default: //その他近接系の敵全般のルーチン。優先順位は　①距離１なら直接攻撃　②距離２以上なら接近
                if (Vector3.Distance(player.transform.position, this.transform.position) < 1.5)
                {
                    ExecuteDirectAttack(this, player);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    int moveCount = 1;
                    while (moveCount <= mob && Vector3.Distance(player.transform.position, transform.position) >= 1.5)
                    {
                        if (player == null) { break; }
                        MoveTo(tilemap.WorldToCell(player.transform.position));
                        moveCount++;
                        yield return new WaitForSeconds(0.1f);
                    }
                    done = true;
                }
                break;
        }

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
        List<Vector3Int> accessibleCells = GetAccessibleCells();

        //取得したマスの中で最も目的地との距離が小さいマスを求める
        Vector3Int destination = GetCellMinimumDistance(accessibleCells, targetCell);
        //自分の座標に代入
        transform.position = tilemap.CellToWorld(destination);

        currentCell = tilemap.WorldToCell(transform.position);
        BattleManager.instance.UpdateMap(beforeCell, currentCell, 2);
    }

    public void MoveAwayFrom(Vector3Int targetCell)//MoveTo()と反対に目的地から遠ざかる動き。
    {
        Vector3Int beforeCell = tilemap.WorldToCell(transform.position);

        //移動可能なマスを取得
        List<Vector3Int> accessibleCells = GetAccessibleCells();

        //取得したマスの中で最も目的地との距離が大きいマスを求める
        Vector3Int destination = GetCellMaximumDistance(accessibleCells, targetCell);
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

    Vector3Int GetCellMinimumDistance(List<Vector3Int> accesibleCells,Vector3Int targetCell)
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

    Vector3Int GetCellMaximumDistance(List<Vector3Int> accesibleCells, Vector3Int targetCell)
    {
        float maxDistance = 0;
        Vector3Int destination = new Vector3Int();
        foreach (Vector3Int searchCell in accesibleCells)
        {
            float distance = Vector3.Distance(tilemap.CellToWorld(searchCell), player.transform.position);
            if (maxDistance < distance)
            {
                maxDistance = distance;
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
