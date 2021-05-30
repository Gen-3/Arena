﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;//変数名はなんでもOK：どこからでもアクセスできる
    private void Awake()
    {
        instance = this;//どこからでもアクセスできるものに自分を入れる
    }

    public Tilemap tilemap = default;
    public EnemyManager enemyPrefab = default;

    public List<EnemyManager> enemies = new List<EnemyManager>();//敵を一括管理

    public PlayerManager player;//プレイヤー
    public bool playerDone = false;//プレイヤーの行動終了判定

    public GameObject MoveButton;
    public GameObject AttackButton;
    public GameObject ThrowButton;
    public GameObject BowButton;
    public GameObject MagicButton;
    public GameObject ChangeButton;
    public GameObject QuitButton;
    public GameObject commandButtons;//コマンドボタンの表示（入力待機の可否）に使う
    public GameObject QuitConfirmButtons;//脱出コマンドの確認ボタン
    public bool battleEnd = false;//敵殲滅時のクリア判定

    //魔法個別ボタン
    [SerializeField] GameObject magic01;
    [SerializeField] GameObject magic02;
    [SerializeField] GameObject magic03;
    [SerializeField] GameObject magic04;
    [SerializeField] GameObject magic05;
    [SerializeField] GameObject magic06;
    [SerializeField] GameObject magic07;
    [SerializeField] GameObject magic08;
    [SerializeField] GameObject magic09;
    [SerializeField] GameObject magic10;
    [SerializeField] GameObject magic11;
    [SerializeField] GameObject magic12;

    //ボタン押下判定に使う
    public bool ClickedWaitButton = false;//待機
    public bool ClickedMoveButton = false;//移動
    public bool ClickedAttackButton = false;//近接攻撃
    public bool ClickedThrowButton = false;//投擲
    public bool ClickedBowButton = false;//ボウ
    public bool ClickedMagicButton = false;//魔法
    public bool ClickedChangeButton = false;//武器の交換
    public bool AttackButtonToggle;//近接攻撃ボタンを自動にするトグル

    public SceneTransitionManager sceneTransitionManager;

    public int[,] map = new int[11, 7];

    public GameObject hitEffect;

    [SerializeField] EnemyTableSO enemyTableSO = default;

    int rank = 0;
    public int stage = 0;

    public PlayerStatusSO playerStatusSO;
    public int expPool = 0;
    public int goldPool = 0;
    public int famePool = 0;

    public Slider HPSlider;
    public int MaxHP;

    [SerializeField] GameObject selectRankPanel = default;

    [SerializeField] ShopItemDatabaseSO weaponShopItemDatabaseSO = default;
    [SerializeField] ShopItemDatabaseSO shieldShopItemDatabaseSO = default;
    [SerializeField] ShopItemDatabaseSO armorShopItemDatabaseSO = default;

    [SerializeField] GameObject annauncePanel = default;
    [SerializeField] Text roundNumber = default;
    [SerializeField] Text playerName = default;
    [SerializeField] Text enemyName = default;
    [SerializeField] Image enemyImage = default;

    public int FameAtEntry=default;

    public int selectedMagicID = default;

    [SerializeField] GameObject changePanel = default;

    private void Start()
    {
        Debug.Log("デバッグ用コマンド　S：全ステータスを40にセットする");
        Debug.Log("デバッグ用コマンド　[：全ステータスを＋５");
        Debug.Log("デバッグ用コマンド　]：全ステータスをー５");
        Debug.Log("デバッグ用コマンド　N：敵を全滅させてステージを進める");
        Debug.Log("デバッグ用コマンド　R：Arenaシーンをリロード");
        Debug.Log("デバッグ用コマンド　M：MAP情報を表示");
        Debug.Log("デバッグ用コマンド　P：プレイヤーの情報を表示");
        Debug.Log("デバッグ用コマンド　E：敵の情報を表示");
        Debug.Log("デバッグ用コマンド　D：デバッグモード　ゲームオーバーにならない");

        selectRankPanel.SetActive(true);
    }

    public void SelectRank(int selectedRank)
    {
        FameAtEntry = playerStatusSO.runtimeFame;
        rank = selectedRank;
        Setup(rank);
        selectRankPanel.SetActive(false);
    }


    private void Setup(int rank)//将来的には、敵のセットリストを引数に渡して敵をセットする感じ？
    {
        playerStatusSO.runtimeMatchAmount += 1;
        StopAllCoroutines();

        //プレイヤーの座標とHP、装備を戦闘開始状態に戻す
        playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
        player.LoadStatus();
        player.transform.position = tilemap.CellToWorld(new Vector3Int(0, 3, 0));

        //画面表示をリセットする
        TextManager.instance.ReloadEquipStatus();
        HPSlider.value = (float)player.hp / (float)playerStatusSO.runtimeHp;

        //魔法レベルに応じて魔法ボタンの表示を消す
        magic01.SetActive(false);
        magic02.SetActive(false);
        magic03.SetActive(false);
        magic04.SetActive(false);
        magic05.SetActive(false);
        magic06.SetActive(false);
        magic07.SetActive(false);
        magic08.SetActive(false);
        magic09.SetActive(false);
        magic10.SetActive(false);
        magic11.SetActive(false);
        magic12.SetActive(false);
        if (playerStatusSO.runtimeMagicLevel >= 1)
        {
            magic01.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 5)
        {
            magic02.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 10)
        {
            magic03.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 15)
        {
            magic04.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 20)
        {
            magic05.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 25)
        {
            magic06.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 30)
        {
            magic07.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 40)
        {
            magic08.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 50)
        {
            magic09.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 60)
        {
            magic10.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 80)
        {
            magic11.SetActive(true);
        }
        if (playerStatusSO.runtimeMagicLevel >= 99)
        {
            magic12.SetActive(true);
        }


        //Tableから敵の組み合わせを選ぶ
        int stageCountMax = enemyTableSO.enemyTable[rank * 5 + stage].enemyTable.Count;
        int r = Random.Range(0, stageCountMax);
        List<EnemyAndPosition> enemyGroups = enemyTableSO.enemyTable[rank * 5 + stage].enemyTable[r].enemyList;
        foreach (EnemyAndPosition enemyAndPosition in enemyGroups)
        {
            EnemyData enemyData = enemyAndPosition.enemy.GetEnemy();
            Vector3Int enemyPosition = enemyAndPosition.position;
            EnemyManager enemy = SpawnEnemy(enemyPrefab, enemyPosition, enemyData);
            enemies.Add(enemy);
        }

        MapSetUp();

        StartCoroutine(Proceed());
    }

    void MapSetUp()
    {
        for (int i = 6; i >= 0; i--)//y軸方向に７マスなのでiが6,5,4,3,2,1,0の7回まわるようにしている
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                map[j, i] = 0;
            }
        }
        map[10, 1] = 9;
        map[10, 3] = 9;
        map[10, 5] = 9;

        Vector3Int cellPos;
        foreach (EnemyManager enemy in enemies)
        {
            cellPos = tilemap.WorldToCell(enemy.transform.position);
            map[cellPos.x, cellPos.y] = 2;
        }

        cellPos = tilemap.WorldToCell(player.transform.position);
        map[cellPos.x, cellPos.y] = 1;
    }

    public void UpdateMap(Vector3Int from, Vector3Int to, int type)
    {
        map[from.x, from.y] = 0;
        map[to.x, to.y] = type;
    }

    public bool IsWallorObj(Vector3Int target)//セル座標で判定する
    {
        if (target.x < 0 || map.GetLength(0) <= target.x)
        {
            return true;
        }
        if (target.y < 0 || map.GetLength(1) <= target.y)
        {
            return true;
        }
        if (map[target.x, target.y] == 0)
        {
            return false;
        }
        return true;
    }

    public List<EnemyManager> GetEnemyOnTheTileOf(Vector3Int cellPosition)
    {
        if (selectedMagicID == 7)//ファイアストーム
        {
            return enemies.FindAll((EnemyManager obj) => GetAroundCell(cellPosition).Contains(obj.currentCell));
        }
        else if (selectedMagicID == 11)//ライトニング
        {
            return enemies;
        }
        else//エナジーボルトなど敵１体を選択する魔法
        {
            return enemies.FindAll((EnemyManager obj) => obj.currentCell == cellPosition);
        }
    }

    public List<Vector3Int> GetAroundCell(Vector3Int centerPos)
    {
        List<Vector3Int> aroundCell = new List<Vector3Int>();

        aroundCell.Add(centerPos);
        if (centerPos.y % 2 == 0)
        {
            aroundCell.Add(centerPos + new Vector3Int(-1, 1, 0));
            aroundCell.Add(centerPos + new Vector3Int(0, 1, 0));
            aroundCell.Add(centerPos + new Vector3Int(-1, 0, 0));
            aroundCell.Add(centerPos + new Vector3Int(1, 0, 0));
            aroundCell.Add(centerPos + new Vector3Int(-1, -1, 0));
            aroundCell.Add(centerPos + new Vector3Int(0, -1, 0));
        }
        else
        {
            aroundCell.Add(centerPos + new Vector3Int(0, 1, 0));
            aroundCell.Add(centerPos + new Vector3Int(1, 1, 0));
            aroundCell.Add(centerPos + new Vector3Int(-1, 0, 0));
            aroundCell.Add(centerPos + new Vector3Int(1, 0, 0));
            aroundCell.Add(centerPos + new Vector3Int(0, -1, 0));
            aroundCell.Add(centerPos + new Vector3Int(1, -1, 0));
        }
        return aroundCell;
    }


    EnemyManager SpawnEnemy(EnemyManager enemyPrefab, Vector3Int position, EnemyData enemyData)
    {
        EnemyManager enemy = Instantiate(enemyPrefab);
        enemy.Init(enemyData);//容れ物にデータを入れる処理
        enemy.transform.position = tilemap.CellToWorld(position);

        enemy.SetTarget(player);
        enemy.SetTileMap(tilemap);
        enemy.SetCurrentPosition();

        return enemy;
    }







    IEnumerator Proceed()
    {
        roundNumber.text = $"第{stage + 1}試合";
        playerName.text = player.unitName;
        enemyName.text = enemies[0].unitName;
        enemyImage.sprite = enemies[0].GetComponent<SpriteRenderer>().sprite;

        annauncePanel.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        annauncePanel.SetActive(false);

        //敵を全滅させてbattleEnd変数がtrueになるまでループ
        while (battleEnd == false)
        {
            if (!player.sleep)
            {
                float quickCoefficient;
                if (player.quick)
                {
                    quickCoefficient = 1.5f;
                }
                else
                {
                    quickCoefficient = 1;
                }
                float slowCoefficient;
                if (player.slow)
                {
                    slowCoefficient = 0.5f;
                }
                else
                {
                    slowCoefficient = 1;
                }
                if (player.agi - player.weight >= 10)
                {
                    player.wt += (player.agi - player.weight) * quickCoefficient * slowCoefficient;
                }
                else
                {
                    player.wt += 10 * quickCoefficient * slowCoefficient;
                }
            }

            foreach (EnemyManager enemy in enemies)
            {
                if (!enemy.sleep)
                {
                    float quickCoefficient;
                    if (enemy.quick)
                    {
                        quickCoefficient = 1.5f;
                    }
                    else
                    {
                        quickCoefficient = 1;
                    }
                    float slowCoefficient;
                    if (enemy.slow)
                    {
                        slowCoefficient = 0.5f;
                    }
                    else
                    {
                        slowCoefficient = 1;
                    }
                    enemy.wt += (enemy.agi - enemy.weight) * quickCoefficient * slowCoefficient;
                }
            }


            

            //WTの降順でソートしてWT最速のenemies[0]とPlayerのWTを比較、enemies[0]の方が早いか等しければenemies[0]のターン処理を開始
            enemies.Sort((a, b) => (int)b.wt - (int)a.wt);

            if (enemies[0].wt > player.wt)//敵のターン////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                enemies[0].StartCoroutine(enemies[0].StartEnemyTurn());
                yield return new WaitUntil(() => enemies[0].done);

                //敵のターン終了時に、プレイヤーのHPバーを更新して戦闘不能判定
                HPSlider.value = (float)player.hp / (float)playerStatusSO.runtimeHp;
                if (player.hp < 0)
                {
                    player.hp = 0;
                    StopAllCoroutines();
                    GameOver();
                }
            }

            //player.wtが最も大きければStartPlayersTurnを呼ぶ////////////////////////////////////////////////////////////////////////////////////////////プレイヤーのターン
            if (player.wt >= enemies[0].wt)
            {
                if (player.continueMoving)//前のターンの移動を継続中の場合
                {
                    player.StartCoroutine(player.Moving(player.destinationAtContinueMoving));
                    yield return new WaitUntil(() => playerDone);
                    commandButtons.SetActive(false);
                    yield return new WaitForSeconds(0.2f);
                }
                else//移動継続中でない場合
                {
                    if (player.weapon != null)
                    {
                        ChangeButton.GetComponent<Button>().interactable = true;

                        if (EnemyContact())//敵接触時、近接攻撃ボタンをオン、魔法攻撃・ボウボタンをオフ
                        {
                            AttackButton.GetComponent<Button>().interactable = true;
                            MagicButton.GetComponent<Button>().interactable = false;
                            BowButton.GetComponent<Button>().interactable = false;
                            if (AttackButtonToggle == true)
                            {
                                OnClickAttackButton();
                            }
                        }
                        else
                        {
                            AttackButton.GetComponent<Button>().interactable = false;

                            if (player.weapon.bow)//メインWがボウのときはボウコマンドボタンを表示
                            {
                                BowButton.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                BowButton.GetComponent<Button>().interactable = false;
                            }

                            if (player.hp > 2)
                            {
                                MagicButton.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                MagicButton.GetComponent<Button>().interactable = false;
                            }
                        }

                        if (player.weapon.through)//メインWが投擲武器のときは投擲コマンドボタンを表示
                        {
                            ThrowButton.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            ThrowButton.GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        if (EnemyContact())//敵接触時、近接攻撃ボタンをオン、魔法攻撃・ボウボタンをオフ
                        {
                            AttackButton.GetComponent<Button>().interactable = true;
                            MagicButton.GetComponent<Button>().interactable = false;
                            BowButton.GetComponent<Button>().interactable = false;
                            ThrowButton.GetComponent<Button>().interactable = false;

                            if (AttackButtonToggle == true)
                            {
                                OnClickAttackButton();
                            }
                        }
                        else
                        {
                            AttackButton.GetComponent<Button>().interactable = false;
                            MagicButton.GetComponent<Button>().interactable = true;
                            BowButton.GetComponent<Button>().interactable = false;
                            ThrowButton.GetComponent<Button>().interactable = false;
                        }
                    }

                    if (tilemap.WorldToCell(player.transform.position) == new Vector3Int(0, 0, 0) ||//マップ四隅にいたら脱出ボタンを選択可能に
                            tilemap.WorldToCell(player.transform.position) == new Vector3Int(0, 6, 0) ||
                            tilemap.WorldToCell(player.transform.position) == new Vector3Int(10, 0, 0) ||
                            tilemap.WorldToCell(player.transform.position) == new Vector3Int(10, 6, 0))
                    {
                        QuitButton.GetComponent<Button>().interactable = true;
                    }

                    //hpが2以下のとき魔法ボタンを表示しない
                    if (player.hp <= 2)
                    {
                        MagicButton.GetComponent<Button>().interactable = false;
                    }

                    commandButtons.SetActive(true);

                    yield return new WaitUntil(() => playerDone);//プレイヤーがコマンドを入力するまで待機
                    HPSlider.value = (float)player.hp / (float)playerStatusSO.runtimeHp;
                    ClickedButtonReset();
                    commandButtons.SetActive(false);
                    yield return new WaitForSeconds(1);
                }
            }


            //敵を全滅させると変数battleEndをtrueにします
            if (enemies.Count == 0)
            {
                Debug.Log("敵を殲滅");
                battleEnd = true;
                yield return new WaitForSeconds(1);
            }


            //行動したユニットのwtを0に戻します
            foreach (EnemyManager enemy in enemies)
            {
                if (enemy.done == true) { enemy.wt = 0; enemy.done = false; }
            }
            if (playerDone == true) { player.wt = 0; playerDone = false; }
        }

        //while (battleEnd == false)に対応
        yield return new WaitForSeconds(0.1f);
        StageClear();
    }

    //撃破時にEnemyManagerをRemoveしてPoolに加算する
    public void RemoveEnemy(EnemyManager enemy, int exp, int gold, int fame)
    {
        expPool += exp;
        goldPool += gold;
        famePool += fame;
        enemies.Remove(enemy);
    }

    bool EnemyContact()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= 1)
            {
                return true;
            }
        }
        return false;
    }












    //各ボタンを押したときの処理
    public void ClickedButtonReset()
    {
        ClickedWaitButton = false;
        ClickedMoveButton = false;
        ClickedAttackButton = false;
        ClickedThrowButton = false;
        ClickedBowButton = false;
        ClickedMagicButton = false;
        ClickedChangeButton = false;
        QuitConfirmButtons.SetActive(false);
        selectMagicPanel.SetActive(false);
        changePanel.SetActive(false);
    }
    //待機に関する関数
    public void OnClickWaitButton()
    {
        ClickedButtonReset();
        ClickedWaitButton = true;
        //        Debug.Log("待機ボタンが押されました");
        playerDone = true;
    }

    //移動に関する関数
    public void OnClickMoveButton()
    {
        ClickedButtonReset();
        ClickedMoveButton = true;
        //        Debug.Log("移動ボタンが押されました");
    }

    //近接攻撃に関する関数
    public void OnClickAttackButton()
    {
        ClickedButtonReset();
        ClickedAttackButton = true;
    }

    public void SwitchAttackButtonToggle()
    {
        ClickedButtonReset();
        if (AttackButtonToggle == false)//トグルを入れた場合
        {
            AttackButtonToggle = true;
            OnClickAttackButton();
        }
        else//トグルを外した場合
        {
            AttackButtonToggle = false;
            ClickedAttackButton = false;
            Debug.Log("連続攻撃トグルが外されました");
        }
    }

    public GameObject selectMagicPanel;
    //魔法に関する関数
    public void OnClickMagicButton()
    {
        ClickedButtonReset();
        ClickedMagicButton = true;

        selectMagicPanel.SetActive(true);
    }
    public void OnClickSelectMagicButton(int ID)
    {
        selectedMagicID = ID;
        selectMagicPanel.SetActive(false);
    }
    public void CanselMagic()
    {
        selectMagicPanel.SetActive(false);
        selectedMagicID = default;
    }

    public WeaponSO beforeWeapon;
    public WeaponSO beforeSub1;
    public WeaponSO beforeSub2;
    public WeaponChangePanel weaponChangePanel;
    public void OnClickChangeButton()
    {
        ClickedButtonReset();
        weaponChangePanel.DisplayPlayersWeapon();
        changePanel.SetActive(true);
        beforeWeapon = player.weapon;
        beforeSub1 = player.subWeapon1;
        beforeSub2 = player.subWeapon2;
    }
    public void OnClickDecideChange()
    {
        if (player.weapon.twoHand)
        {
            player.shield = shieldShopItemDatabaseSO.EquipList[0] as ShieldSO;
        }
        textManager.ReloadEquipStatus();
        player.ReloadStatus();
        playerDone = true;
    }
    public void OnClickCanselChange()
    {
        //変更前の状態を記憶しておいて戻す
        player.weapon = beforeWeapon;
        player.subWeapon1 = beforeSub1;
        player.subWeapon2 = beforeSub2;
//        weaponChangePanel.dropAreas[0].GetComponesntInChildren<DragObj> = beforeWeapon;
  //      weaponChangePanel.dropAreas[1].GetComponentInChildren<DragObj> = beforeSub1;
    //    weaponChangePanel.dropAreas[2].GetComponentInChildren<DragObj> = beforeSub2;

        ClickedButtonReset();
        changePanel.SetActive(false);
    }

    public void OnClickThrowButton()
    {
        ClickedButtonReset();
        ClickedThrowButton = true;
    }

    public void OnClickBowButton()
    {
        ClickedButtonReset();
        ClickedBowButton = true;
    }


    public void OnClickQuitButton()
    {
        ClickedButtonReset();
        Debug.Log("脱出ボタンが押されました");
        QuitConfirmButtons.SetActive(true);
    }

    public GameObject quitPanel;
    public GameObject quit2Panel;
    public void QuitConfirm()
    {
        playerStatusSO.runtimeExp += expPool;
        playerStatusSO.runtimeGold += goldPool;
        playerStatusSO.runtimeFame += famePool - 80;
        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame)
        {
            playerStatusSO.runtimeMaxFame = playerStatusSO.runtimeFame;
        }

        Destroy(player.gameObject);
        Destroy(commandButtons);

        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame - 100)
        {
            textManager.Quit();

            Debug.Log("逃げ出した……");
            Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool - 80}の名声を得た");
            (expPool, goldPool, famePool) = (0, 0, 0);

            QuitConfirmButtons.SetActive(false);
            quitPanel.SetActive(true);
        }
        else
        {
            if(debugMode)
            {
                Debug.Log("名声低下によるゲームオーバー処理を飛ばしました（本来ゲームオーバー）");
                textManager.Quit();

                Debug.Log("逃げ出した……");
                Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool - 80}の名声を得た");
                (expPool, goldPool, famePool) = (0, 0, 0);

                QuitConfirmButtons.SetActive(false);
                quitPanel.SetActive(true);
            }
            else
            {
                Debug.Log("?????????????????????????");
                textManager.Quit2();
                //名声が地に堕ちてゲームオーバー処理
                playerStatusSO.runtimePlayerName = default;
                playerStatusSO.runtimeStr = default;
                playerStatusSO.runtimeDex = default;
                playerStatusSO.runtimeAgi = default;
                playerStatusSO.runtimeVit = default;
                playerStatusSO.runtimeMen = default;
                playerStatusSO.runtimeHp = default;
                playerStatusSO.runtimeMagicLevel = default;
                playerStatusSO.runtimeWeapon = default;
                playerStatusSO.runtimeSubWeapon1 = default;
                playerStatusSO.runtimeSubWeapon2 = default;
                playerStatusSO.runtimeShield = default;
                playerStatusSO.runtimeArmor = default;
                playerStatusSO.runtimeGold = default;
                playerStatusSO.runtimeExp = default;
                playerStatusSO.runtimeFame = default;
                playerStatusSO.runtimeMaxFame = default;
                playerStatusSO.runtimeMatchAmount = default;
                playerStatusSO.runtimeWinAmount = default;

                Debug.Log("名声は地に落ちた……");
                QuitConfirmButtons.SetActive(false);
                quit2Panel.SetActive(true);
            }
        }
    }
    public void QuitCancel()
    {
        ClickedButtonReset();
    }

    public GameObject gameoverPanel;
    public GameObject gameover2Panel;
    bool debugMode=false;
    public void GameOver()
    {
        playerStatusSO.runtimeExp += expPool;
        playerStatusSO.runtimeGold += goldPool;
        playerStatusSO.runtimeFame += famePool;
        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame)
        {
            playerStatusSO.runtimeMaxFame = playerStatusSO.runtimeFame;
        }

        Destroy(player.gameObject);
        Destroy(commandButtons);
        //ここで死亡判定、引退判定
        if (Random.Range(-100f, 100f) < player.vit)
        {
            textManager.GameOver();

            Debug.Log("敗退した（死亡ではない）");
            Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool}の名声を得た");
            (expPool, goldPool, famePool) = (0, 0, 0);

            gameoverPanel.SetActive(true);
        }
        else
        {
            if (debugMode)
            {
                textManager.GameOver();

                Debug.Log($"死亡判定によるゲームオーバー処理を飛ばしました（本来ゲームオーバー）");
                (expPool, goldPool, famePool) = (0, 0, 0);

                gameoverPanel.SetActive(true);
            }
            else
            {
                textManager.GameOver2();

                playerStatusSO.runtimePlayerName = default;
                playerStatusSO.runtimeStr = default;
                playerStatusSO.runtimeDex = default;
                playerStatusSO.runtimeAgi = default;
                playerStatusSO.runtimeVit = default;
                playerStatusSO.runtimeMen = default;
                playerStatusSO.runtimeHp = default;
                playerStatusSO.runtimeMagicLevel = default;
                playerStatusSO.runtimeWeapon = default;
                playerStatusSO.runtimeSubWeapon1 = default;
                playerStatusSO.runtimeSubWeapon2 = default;
                playerStatusSO.runtimeShield = default;
                playerStatusSO.runtimeArmor = default;
                playerStatusSO.runtimeGold = default;
                playerStatusSO.runtimeExp = default;
                playerStatusSO.runtimeFame = default;
                playerStatusSO.runtimeMaxFame = default;
                playerStatusSO.runtimeMatchAmount = default;
                playerStatusSO.runtimeWinAmount = default;

                Debug.Log("死亡した（キャラクターロスト）");
                gameover2Panel.SetActive(true);
            }
        }
    }

    public void StageClear()
    {
        battleEnd = false;

        playerStatusSO.runtimeWinAmount += 1;

        if (stage % 5 != 4)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)//念の為enemies[]の中身を消してる？
            {
                Destroy(enemies[i].gameObject);
                enemies.Remove(enemies[i]);
            }
            stage++;
            Setup(rank);
        }
        else
        {
            Destroy(commandButtons);
            StartCoroutine(RankClear());
        }

    }

    public GameObject rankClear;
    IEnumerator RankClear()
    {
        switch (rank)
        {
            case 0:
                goldPool = 500;
                break;
            case 1:
                goldPool = 1000;
                break;
            case 2:
                goldPool = 2000;
                break;
            case 3:
                goldPool = 3000;
                break;
            case 4:
                goldPool = 5000;
                break;
        }

        playerStatusSO.runtimeExp += expPool;
        playerStatusSO.runtimeGold += goldPool;
        playerStatusSO.runtimeFame += famePool;
        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame)
        {
            playerStatusSO.runtimeMaxFame = playerStatusSO.runtimeFame;
        }

        textManager.RankClear();

        rankClear.SetActive(true);//あとで修正する。メッセージウインドウを出すなど。
        Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool}の名声を得た");
        (expPool, goldPool, famePool) = (0, 0, 0);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        sceneTransitionManager.LoadTo("Home");
    }




    [SerializeField] TextManager textManager;
    private void Update()//デバッグ用コマンド！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    {
        if (Input.GetKeyDown(KeyCode.R))//Reset
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Arena");
            Debug.Log("デバッグコマンド：Arenaシーンを再読み込みしました");
        }
        if (Input.GetKeyDown(KeyCode.N))//NextStage
        {
            playerStatusSO.runtimeWinAmount += 1;
            if (stage % 5 != 4)
            {
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    Destroy(enemies[i].gameObject);
                    enemies[i].DebugRemoveEnemy();
                }
                stage++;
                Setup(rank);
            }
            else
            {
                Destroy(commandButtons);
                StartCoroutine(RankClear());
            }
            Debug.Log("デバッグコマンド：ステージを進めました");
        }
        if (Input.GetKeyDown(KeyCode.M))//MapDebug
        {
            for (int i = 6; i >= 0; i--)//y軸方向に７マスなのでiが6,5,4,3,2,1,0の7回まわるようにしている
            {
                string line = "";
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    line += map[j, i];
                }
                Debug.Log(line);
            }
            Debug.Log("デバッグコマンド：マップ情報を表示しました");
        }
        if (Input.GetKeyDown(KeyCode.H))//HpSliderDebug
        {
            Debug.Log($"{player.hp}/{playerStatusSO.runtimeHp}={HPSlider.value}");
            Debug.Log("デバッグコマンド：HPSliderの情報を表示しました");
        }
        if (Input.GetKeyDown(KeyCode.E))//EnemiesDebug
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Debug.Log($"{enemies[i]}(No.{i})の名前は{enemies[i].unitName}");
            }
            Debug.Log("デバッグコマンド：敵のリストを表示しました");
        }
        if (Input.GetKeyDown(KeyCode.P))// PlayerStatusLogDebug
        {
            Debug.Log("ステータスのデバッグ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Debug.Log("wt=" + player.wt);
            Debug.Log("hp=" + player.hp);
            Debug.Log("atk=" + player.atk);
            Debug.Log("def=" + player.def);
            Debug.Log("weight=" + player.weight);
            Debug.Log("mob=" + player.mob);
            Debug.Log("str=" + player.str);
            Debug.Log("dex=" + player.dex);
            Debug.Log("agi=" + player.agi);
            Debug.Log("vit=" + player.vit);
            Debug.Log("weapon=" + player.weapon);
            Debug.Log("sub1=" + player.subWeapon1);
            Debug.Log("sub2=" + player.subWeapon2);
            Debug.Log("shield=" + player.shield);
            Debug.Log("armor=" + player.armor);
            Debug.Log($"expPoolは{expPool}、goldPoolは{goldPool}、famePoolは{famePool}");
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            playerStatusSO.runtimeStr += 5;
            playerStatusSO.runtimeDex += 5;
            playerStatusSO.runtimeAgi += 5;
            playerStatusSO.runtimeVit += 5;
            playerStatusSO.runtimeMen += 5;
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
            player.LoadStatus();
            Debug.Log($"ステータス＋５({playerStatusSO.runtimeStr})");
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            playerStatusSO.runtimeStr -= 5;
            playerStatusSO.runtimeDex -= 5;
            playerStatusSO.runtimeAgi -= 5;
            playerStatusSO.runtimeVit -= 5;
            playerStatusSO.runtimeMen -= 5;
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
            player.LoadStatus();
            Debug.Log($"ステータス−５({playerStatusSO.runtimeStr})");
        }

        if (Input.GetKeyDown(KeyCode.S))//SetStatus:デバッグでいきなりArenaシーンを呼び出したときに能力値をセットするためのもの
        {
            playerStatusSO.runtimePlayerName = "テストプレイなう";
            playerStatusSO.runtimeStr = 40;
            playerStatusSO.runtimeDex = 40;
            playerStatusSO.runtimeAgi = 40;
            playerStatusSO.runtimeVit = 40;
            playerStatusSO.runtimeMen = 40;
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
            playerStatusSO.runtimeWeapon = weaponShopItemDatabaseSO.EquipList[3] as WeaponSO;//3ハンドアックス 4ショートソード 6ウォーハンマー 8ロングソード 10ボウ 11クロスボウ
            playerStatusSO.runtimeSubWeapon1 = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            playerStatusSO.runtimeSubWeapon2 = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            playerStatusSO.runtimeShield = shieldShopItemDatabaseSO.EquipList[0] as ShieldSO;//1バックラー 2ラウンドシールド 3カイトシールド　4ラージシールド　5タワシ
            playerStatusSO.runtimeArmor = armorShopItemDatabaseSO.EquipList[0] as ArmorSO;//1レザーアーマー 2リングメイル 3チェインメイル 4フルプレート　
            playerStatusSO.runtimeMagicLevel = 99;
            player.LoadStatus();
            debugMode = true;
            Debug.Log("デバッグ用ステータスをセットしました");
        }
        if (Input.GetKeyDown(KeyCode.C))//ConsoleTest:コンソールの表示送りテスト
        {
            textManager.UpdateConsole(((int)Random.Range(0,101)).ToString());
        }

        if (Input.GetKeyDown(KeyCode.D))//DebugModeオン
        {
            if (debugMode)
            {
                debugMode = false;
                Debug.Log("デバッグモード：オフ　ゲームオーバー処理は飛ばされません");
            }
            else
            {
                debugMode = true;
                Debug.Log("デバッグモード：オン　ゲームオーバー処理を飛ばします");
            }
        }
    }
}