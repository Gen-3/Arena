using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class PlayerManager : Battler
{
    [SerializeField] PlayerStatusSO PlayerStatusSO = default;

    public BattleManager battleManager;

    //装備
    public WeaponSO weapon;
    public WeaponSO subWeapon1;
    public WeaponSO subWeapon2;
    public ShieldSO shield;
    public ArmorSO armor;


    public Tilemap tilemap;
    private Vector3Int currentCell;
    public bool reachDestination = true;
    public bool stacked = false;
    public bool continueMoving = false;
    public Vector3Int destinationAtContinueMoving;

    public GameObject hitEffect;

    public List<MagicBaseSO> magicList = new List<MagicBaseSO>();
    [SerializeField] ShopItemDatabaseSO weaponShopItemDatabaseSO;
    [SerializeField] ShopItemDatabaseSO shieldShopItemDatabaseSO;
    [SerializeField] TextManager textManager;

    [SerializeField] GameObject attackEffect;
    [SerializeField] GameObject boltEffect;
    [SerializeField] GameObject FireEffect;



    private void Start()
    {
        LoadStatus();
    }

    public void LoadStatus()
    {
        wt = 0;
        unitName = PlayerStatusSO.runtimePlayerName;
        //魔法
        magicList = PlayerStatusSO.magicList;

        weapon = PlayerStatusSO.runtimeWeapon;
        subWeapon1 = PlayerStatusSO.runtimeSubWeapon1;
        subWeapon2 = PlayerStatusSO.runtimeSubWeapon2;
        shield = PlayerStatusSO.runtimeShield;
        armor = PlayerStatusSO.runtimeArmor;
//装備ごとの重量などはweapon.weightなどで取得できる
        
        str = PlayerStatusSO.runtimeStr;
        dex = PlayerStatusSO.runtimeDex;
        agi = PlayerStatusSO.runtimeAgi;
        vit = PlayerStatusSO.runtimeVit;
        men = PlayerStatusSO.runtimeMen;

        flash = false;
        protect = false;
        slow = false;
        sleep = false;
        power = false;
        quick = false;
        silence = false;

        hp = PlayerStatusSO.runtimeHp;
        if (hp < 10) { hp = 10; }

        if (weapon.twoHand)
        {
            shield = shieldShopItemDatabaseSO.EquipList[0]as ShieldSO;
        }

        if (weapon != null)
        {
            atk = str + weapon.atk * (1 + dex / 100);
        }
        else
        {
            atk = str;
        }

        if (shield != null && armor != null)
        {
            def = vit + shield.def + armor.def;
            resistanceFire = shield.resistanceFire + armor.resistanceFire;
        }
        else if (shield == null && armor == null)
        {
            def = vit;
            resistanceFire = 0;
        }
        else if (shield == null)
        {
            def = vit + armor.def;
            resistanceFire = armor.resistanceFire;
        }
        else if (armor == null)
        {
            def = vit + shield.def;
            resistanceFire = shield.resistanceFire;
        }

        weight = 0;
        if (weapon != null) { weight += weapon.weight; }
        if (subWeapon1 != null) { weight += subWeapon1.weight; }
        if (subWeapon2 != null) { weight += subWeapon2.weight; }
        if (shield != null) { weight += shield.weight; }
        if (armor != null) { weight += armor.weight; }

        mob = 2 + agi / 15 - weight * 3 / (str + 1);//暫定的な式
        if (mob < 1) { mob = 1; }

        resistanceMagic = 0;
    }

    public void ReloadStatus()
    {
        if (weapon != null)
        {
            atk = (int)(str + weapon.atk * (1 + dex / 100));
        }
        else
        {
            atk = str;
        }

        if (weapon.twoHand)
        {
            shield = shieldShopItemDatabaseSO.EquipList[0]as ShieldSO;
        }

        weight = 0;
        if (weapon != null) { weight += weapon.weight; }
        if (subWeapon1 != null) { weight += subWeapon1.weight; }
        if (subWeapon2 != null) { weight += subWeapon2.weight; }
        if (shield != null) { weight += shield.weight; }
        if (armor != null) { weight += armor.weight; }
        mob = (int)(1 + agi / 15 - weight * 3 / (str + 1));//暫定的な式
        if (mob < 1) { mob = 1; }
    }

    /*//4月29日メモ：直接magicList[id].Esecute(Battler,List<Battler>)を呼べばいいから、ここは不要では？
    public void ExecutePlayerMagic(List<EnemyManager> targets)
    {
        foreach (Battler target in targets)
        magicList[battleManager.selectedMagicID].Execute(this, target);
    }
    */

    public override void ExecuteDirectAttack(Battler attacker, Battler target)
    {
        base.ExecuteDirectAttack(attacker, target);
    }

    public void ClickOnTile(Vector3 clickedPosition)//タイルマップから呼び出す。元「MovePlayerPosition()」
    {
        if (battleManager.ClickedMoveButton)//移動
        {
            //if障害物があって移動できないマスをクリックした時は無視
            battleManager.ClickedMoveButton = false;
            battleManager.commandButtons.SetActive(false);
            reachDestination = false;//これがtrueになったら移動終了
            currentCell = tilemap.WorldToCell(transform.position);//ユニットの現在位置ワールド座標
            StopAllCoroutines();//コルーチンを止めておかないと移動押すたびに重ねて移動しようとしてしまう
            StartCoroutine(Moving(tilemap.WorldToCell(clickedPosition)));
        }

        if (battleManager.ClickedAttackButton)//直接攻撃
        {
            targetPosition = tilemap.WorldToCell(clickedPosition);

            if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count != 0)
            {
                if (Vector3.Distance(transform.position, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition)[0].transform.position) <= 1)
                {
                    ExecuteDirectAttack(this, battleManager.GetEnemyOnTheTileOf(targetPosition)[0]);
                    battleManager.GetEnemyOnTheTileOf(targetPosition)[0].CheckHP();
                    battleManager.playerDone = true;
                }
            }
        }

        if (battleManager.ClickedMagicButton)//魔法
        {
            targetPosition = tilemap.WorldToCell(clickedPosition);

            if (battleManager.selectedMagicID == 7) //ファイアーストーム選択時、どのタイルであれ発動させる
            {
                if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count != 0)
                {
                    for (int i = BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count; i > 0; i--)
                    {
                        magicList[battleManager.selectedMagicID].Execute(this, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition)[i - 1]);
                    }
                    battleManager.selectedMagicID = default;
                    battleManager.playerDone = true;
                }
            }
            else if (battleManager.selectedMagicID == 11)//ライトニング選択時、どのタイルであれ発動させる
            {
                for (int i = battleManager.enemies.Count; i > 0; i--)
                {
                    magicList[battleManager.selectedMagicID].Execute(this, battleManager.enemies[i - 1]);
                }
                battleManager.selectedMagicID = default;
                battleManager.playerDone = true;
            }
            else if(battleManager.selectedMagicID ==1 || battleManager.selectedMagicID == 2 || battleManager.selectedMagicID == 4 || battleManager.selectedMagicID == 5
                || battleManager.selectedMagicID == 9 || battleManager.selectedMagicID == 12)//その他敵単体への魔法を選択時、敵のいるタイルでのみ処理する
            {
                if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count != 0)
                {
                    magicList[battleManager.selectedMagicID].Execute(this, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition)[0]);
                    battleManager.selectedMagicID = default;
                    battleManager.playerDone = true;
                }
            }
            else if (battleManager.selectedMagicID == 3 || battleManager.selectedMagicID == 6 || battleManager.selectedMagicID == 8)//プロテクト・パワー・クイックは自分のタイルのときのみ
            {
                if(tilemap.WorldToCell(clickedPosition) == tilemap.WorldToCell(transform.position))
                {
                    magicList[battleManager.selectedMagicID].Execute(this, this);
                    battleManager.selectedMagicID = default;
                    battleManager.playerDone = true;
                }
            }
            else if (battleManager.selectedMagicID == 10)//ディスペルは敵味方問わず発動
            {
                if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count != 0)
                {
                    magicList[battleManager.selectedMagicID].Execute(this, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition)[0]);
                    battleManager.selectedMagicID = default;
                    battleManager.playerDone = true;
                }
                else if (tilemap.WorldToCell(clickedPosition) == tilemap.WorldToCell(transform.position))
                {
                    magicList[battleManager.selectedMagicID].Execute(this, this);
                    battleManager.selectedMagicID = default;
                    battleManager.playerDone = true;
                }
            }
            if (battleManager.playerDone)
            {
                hp -= 2;
            }
        }
        if (battleManager.ClickedThrowButton)//投擲
        {
            targetPosition = tilemap.WorldToCell(clickedPosition);

            if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count != 0)
            {
                ExecuteDirectAttack(this, battleManager.GetEnemyOnTheTileOf(targetPosition)[0]);
                battleManager.GetEnemyOnTheTileOf(targetPosition)[0].CheckHP();

                weapon = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;//投げた装備を外してステータスを更新
                textManager.ReloadEquipStatus();
                ReloadStatus();

                battleManager.playerDone = true;
            }
        }
        if(battleManager.ClickedBowButton)//ボウ
        {
            targetPosition = tilemap.WorldToCell(clickedPosition);

            if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).Count != 0)
            {
                if (Vector3.Distance(transform.position, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition)[0].transform.position) > 1)
                {

                    ExecuteBowAttack(this, battleManager.GetEnemyOnTheTileOf(targetPosition)[0]);
                    battleManager.GetEnemyOnTheTileOf(targetPosition)[0].CheckHP();




                    battleManager.playerDone = true;
                }
            }
        }

    }

    public IEnumerator Moving(Vector3Int destinationCell)
     {
        int moveCount = 0;
        while (reachDestination == false&&moveCount<mob)//目的地に着くか、移動力mobの回数動くかするまで繰り返す
        {
            moveCount++;
            yield return new WaitForSeconds(0.1f);

            Vector3Int beforePosition = tilemap.WorldToCell(transform.position);//移動前のセル座標をメモ
                                                                                //            MovePlayer();



            MoveTo(destinationCell);





            currentCell = tilemap.WorldToCell(transform.position);//移動後のセル座標をメモ
            battleManager.UpdateMap(beforePosition,currentCell , 1);//移動前後のマスのmap状態を更新


            if (Vector3.Distance(tilemap.CellToWorld(destinationCell), transform.position) < Mathf.Epsilon)//目的地に到着したか確認
            {
                reachDestination = true;
            }
            if(beforePosition==currentCell)//移動できなかったか確認
            {
                stacked = true; 
            }
            else
            {
                stacked = false;
            }
        }
        battleManager.playerDone = true;
        if (!reachDestination&&!stacked)//目的地に着いている、または移動していない場合は移動継続は無し、移動したのに目的地についていれば移動継続あり
        {
            continueMoving = true;
            destinationAtContinueMoving = destinationCell;
        }
        else
        {
            continueMoving = false;
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

        foreach (Vector3Int checkCell in BattleManager.instance.GetAroundCell(currentCell))
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

    Vector3Int GetDestination(List<Vector3Int> accesibleCells, Vector3Int targetCell)
    {
        float minDistance = 999999999999;
        Vector3Int destination = new Vector3Int();
        foreach (Vector3Int searchCell in accesibleCells)
        {
            float distance = Vector3.Distance(tilemap.CellToWorld(searchCell), tilemap.CellToWorld(targetCell));
            if (minDistance > distance)
            {
                minDistance = distance;
                destination = searchCell;
            }
        }
        return destination;
    }

    /*
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
            if (diff.x > 0 || diff.x == 0 && currentCell.x <= 4)
            {
                MoveOnTile(Direction.UpRight);
            }
            else if (diff.x < 0 || diff.x == 0 && currentCell.x > 4)
            {
                MoveOnTile(Direction.UpLeft);
            }            
        }
        else if (diff.y < 0)
        {
            if (diff.x > 0 || diff.x == 0 && currentCell.x <= 4)
            {
                MoveOnTile(Direction.DownRight);
            }
            else if (diff.x < 0 || diff.x == 0 && currentCell.x > 4)
            {
                MoveOnTile(Direction.DownLeft);
            }
        }
//            Debug.Log("目的地" + destination + "に未着。現在地は" + currentPosition);//ここがなぜか0.0.0
//            Debug.Log("目的地まで" + Vector3.Distance(destination, transform.position));
            return;
    }*/

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
