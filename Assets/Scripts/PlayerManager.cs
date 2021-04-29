using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerManager : Battler
{
    [SerializeField] PlayerStatusSO PlayerStatusSO = default;

    public BattleManager battleManager;

    public int weight;

    //装備
    public WeaponSO weapon;
    public WeaponSO subWeapon1;
    public WeaponSO subWeapon2;
    public ShieldSO shield;
    public ArmorSO armor;


    public Tilemap tilemap;
    private Vector3Int currentPosition;
    public Vector3 destination;//ワールド座標
    public Vector3Int destinationC;//セル座標
    public bool reachDestination = true;
    public bool continueMoving = false;

    public GameObject hitEffect;

    List<MagicBaseSO> magicList = new List<MagicBaseSO>();

    //Battler target = default;

    private void Start()
    {
        LoadStatus();
    }

    void LoadStatus()
    {
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

        hp = PlayerStatusSO.runtimeHp;
        if (hp < 10) { hp = 10; }

        if (weapon != null)
        {
            atk = (int)(str + weapon.atk * (1 + dex / 100));
        }
        else
        {
            atk = str;
        }

        if (shield != null && armor != null)
        {
            def = vit + shield.def + armor.def;
        }
        else if (shield == null && armor == null)
        {
            def = vit;
        }
        else if (shield == null)
        {
            def = vit + armor.def;
        }
        else if (armor == null)
        {
            def = vit + shield.def;
        }

        weight = 0;
        if (weapon != null) { weight += weapon.weight; }
        if (subWeapon1 != null) { weight += subWeapon1.weight; }
        if (subWeapon2 != null) { weight += subWeapon2.weight; }
        if (shield != null) { weight += shield.weight; }
        if (armor != null) { weight += armor.weight; }

        mob = (int)(agi/33+ Mathf.Log(agi*1.5f) - weight * 10 / (str + 1));//暫定的な式
        if (mob < 1) { mob = 1; }
    }

    /*4月29日メモ：直接magicList[id].Esecute(Battler,Battler)を呼べばいいから、ここは不要では？
    public void ExecuteMagic()
    {
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
            destinationC = tilemap.WorldToCell(clickedPosition);//目的地のワールド座標
            destination = tilemap.CellToWorld(destinationC);//目的地のセル座標
            currentPosition = tilemap.WorldToCell(transform.position);//ユニットの現在位置ワールド座標
            StopAllCoroutines();//コルーチンを止めておかないと移動押すたびに重ねて移動しようとしてしまう
            StartCoroutine(Moving(destination));
        }

        if (battleManager.ClickedAttackButton)//直接攻撃
        {
            targetPosition = tilemap.WorldToCell(clickedPosition);

            if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition) != null)
            {
                if (Vector3.Distance(transform.position, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition).transform.position) <= 1)
                {
                    ExecuteDirectAttack(this, battleManager.GetEnemyOnTheTileOf(targetPosition));
                    battleManager.GetEnemyOnTheTileOf(targetPosition).CheckHP();
                    battleManager.playerDone = true;
                }
            }
        }

        if (battleManager.ClickedMagicButton)//魔法
        {
            if (battleManager.selectedMagicID == 2) //ファイアーストーム選択時、どのタイルであれ発動させる
            {
                targetPosition = tilemap.WorldToCell(clickedPosition);
                magicList[battleManager.selectedMagicID].Execute(this, target);
                battleManager.selectedMagicID = default;
                battleManager.playerDone = true;
            }
            else if (battleManager.selectedMagicID == 3)//ライトニング選択時、どのタイルであれ発動させる///////////////////////////////////////////////////////////
            {
                for (int i = 0; i < battleManager.enemies.Count; i++)
                {
                    magicList[battleManager.selectedMagicID].Execute(this, battleManager.enemies[i]);
                }

                battleManager.selectedMagicID = default;
                battleManager.playerDone = true;
            }
            else//その他の魔法を選択時、敵のいるタイルでのみ処理する
            {
                targetPosition = tilemap.WorldToCell(clickedPosition);

                if (BattleManager.instance.GetEnemyOnTheTileOf(targetPosition) != null)
                {
                    magicList[battleManager.selectedMagicID].Execute(this, BattleManager.instance.GetEnemyOnTheTileOf(targetPosition));
                    battleManager.selectedMagicID = default;
                    battleManager.playerDone = true;
                }
            }
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
