﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;//変数名はなんでもOK：どこからでもアクセスできる
    private void Awake()
    {
        instance = this;//どこからでもアクセスできるものに自分を入れる
    }

    public Tilemap tilemap = default;
    public EnemyManager enemyPrefab = default;

    List<EnemyManager> enemies = new List<EnemyManager>();//敵を一括管理

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

    //ボタン押下判定に使う
    public bool ClickedWaitButton = false;//待機
    public bool ClickedMoveButton = false;//移動
    public bool ClickedAttackButton = false;//近接攻撃
    public bool ClickedThrowButton = false;//投擲
    public bool ClickedBowButton = false;//ボウ
    public bool ClickedMagicButton = false;//魔法
    public bool ClickedChangeButton = false;//武器の交換
    public bool AttackButtonToggle;//近接攻撃ボタンを自動にするトグル

    int loopCount = 0;//デバッグ時のループカウント用
    public SceneTransitionManager sceneTransitionManager;

    public int[,] map = new int[11, 7];

    public GameObject hitEffect;

    [SerializeField] EnemyTableSO enemyTableSO = default;

    int rank = 0;
    int stage = 0;

    public PlayerStatusSO playerStatusSO;
    int expPool = 0;
    int goldPool = 0;
    int famePool = 0;

    public Slider HPSlider;
    private int MaxHP;

    private void Start()
    {
//        Random.InitState(0);
        Setup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))//Reset
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Arena");
        }
        if (Input.GetKeyDown(KeyCode.N))//NextStage
        {
            DebugStageClear();
        }
        if (Input.GetKeyDown(KeyCode.M))//MapDebug
        {
            MapDebug();
        }
        if (Input.GetKeyDown(KeyCode.P))//PoolDebug
        {
            PoolDebug();
        }
        if (Input.GetKeyDown(KeyCode.H))//HpSliderDebug
        {
            Debug.Log($"{player.hp}/{MaxHP}={HPSlider.value}");
        }
        if (Input.GetKeyDown(KeyCode.E))//EnemiesDebug
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Debug.Log(enemies[i]);
            }
        }

    }

    private void Setup()//将来的には、敵のセットリストを引数に渡して敵をセットする感じ？
    {
        StopAllCoroutines();
        //プレイヤーの座標とHP、装備を戦闘開始状態に戻す
        player.hp = playerStatusSO.runtimeHp;
        MaxHP = player.hp;
        HPSlider.value = (float)player.hp / (float)MaxHP;
        player.transform.position = tilemap.CellToWorld(new Vector3Int (0, 3,0));


        //Tableから敵の組み合わせを選ぶ
        int stageCountMax=enemyTableSO.enemyTable[rank * 5 + stage].enemyTable.Count;
        int r = Random.Range(0, stageCountMax);
        List<EnemyAndPosition> enemyGroups = enemyTableSO.enemyTable[rank*5 + stage].enemyTable[r].enemyList;
        foreach(EnemyAndPosition enemyAndPosition in enemyGroups)
        {
            EnemyData enemyData = enemyAndPosition.enemy.GetEnemy();
            Vector3Int enemyPosition = enemyAndPosition.position;
            EnemyManager enemy = SpawnEnemy(enemyPrefab, enemyPosition, enemyData);
            enemies.Add(enemy);
        }

        MapSetUp();

        StartCoroutine(Proceed());
    }

    void MapDebug()
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
    }

    void MapSetUp()
    {
        for (int i = 6; i >= 0; i--)//y軸方向に７マスなのでiが6,5,4,3,2,1,0の7回まわるようにしている
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                map[j, i]=0;
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

    EnemyManager SpawnEnemy(EnemyManager enemyPrefab,Vector3Int position, EnemyData enemyData)
    {
        //プレハブを用いない場合の敵の生成
        //EnemyData enemyData = enemyDatabaseEntity.enemies[id];//入れるデータをデータベースから持ってくる
        EnemyManager enemy = Instantiate(enemyPrefab);
        enemy.Init(enemyData);//容れ物にデータを入れる処理
        enemy.transform.position = tilemap.CellToWorld(position);
        //enemy.AddNewAction(OnClickEnemy);//クリック時に攻撃処理する関数を実体化したゲームオブジェクトのEnemyManagerから実行できるようにする

        enemy.SetTarget(player);
        enemy.SetTileMap(tilemap);

        return enemy;
    }

    IEnumerator Proceed()
    {
        //敵を全滅させてbattleEnd変数がtrueになるまでループ
        while (battleEnd == false)
        {
            //デバッグ用ループ回数カウント
            loopCount++;
//            Debug.Log("ループ" + loopCount + "回目");
//            MapDebug();//PlayerManagerから呼んだBattleManager.UpdateMapと時間差で動いてバグになるため、SetUp時に1回呼ぶだけに変更

            //各ユニットのwt変数にagi変数を加える
            player.wt += player.agi + 10 ;
            for(int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].hp > 0) { enemies[i].wt += enemies[i].agi; }
            }

            //WTの降順でソートしてWT最速のenemies[0]とPlayerのWTを比較
            enemies.Sort((a, b) => b.wt - a.wt);
            //Debug.Log(player.name + player.wt + "/" + enemies[0].name + enemies[0].wt + "/" + enemies[1].name + enemies[1].wt + "/" + enemies[2].name + enemies[2].wt + "/" + enemies[3].name + enemies[3].wt + "/" + enemies[4].name + enemies[4].wt + "/");

            //enemy.wtが最も大きければenemyActionを呼ぶ
            //将来的には、敵に行動パターンのint型変数をもたせて、その変数によっていくつかの行動パターンを持たせる
            if (enemies[0].wt > player.wt)
            {
//                Debug.Log(enemies[0] + "のターン");//敵のターン
                yield return new WaitForSeconds(0.5f);


                if (Vector3.Distance(player.transform.position, enemies[0].transform.position) <= 1)
                {
                    int hit = 70 + enemies[0].dex - player.agi;
                    float rundomNumber = Random.Range(0f, 100f);
                    //Debug.Log($"hit={hit}/rundomNumber={(int)rundomNumber}");
                    if (rundomNumber < hit)
                    {
                        //Debug.Log("距離が１より小さいので直接攻撃する");
                        float damage = (Random.Range((float)(enemies[0].atk * enemies[0].dex / 100),(float)(enemies[0].atk)) - player.def) / 5;
                        if (damage <= 0) { damage = 0; };
                        player.hp -= (int)damage;
                        HPSlider.value = (float)player.hp / (float)MaxHP;
                        Debug.Log(enemies[0].name + "の攻撃でPlayerは" + (int)damage + "のダメージ（残りHPは" + player.hp + "）");
                        if (player.hp < 0)
                        {
                            player.hp = 0;
                            StopAllCoroutines();
                            StartCoroutine(GameOver());
                        }
                    }
                    else
                    {
                        Debug.Log("回避！");
                    }
                }
                else
                {
                    //Debug.Log("距離が１より大きいので移動する");
                    int moveCount = 1;
                    while (moveCount <= enemies[0].mob && Vector3.Distance(player.transform.position, enemies[0].transform.position) > 1)
                    {
                        //Debug.Log(moveCount + "回目");

                        enemies[0].MoveTo(player.transform.position);



                        moveCount++;
                        yield return new WaitForSeconds(0.2f);
                    }
                }

                enemies[0].done = true;
                //Debug.Log(enemies[0].name + "の行動が終了しました");
            }

            //player.wtが最も大きければStartPlayersTurnを呼ぶ////////////////////////////////////////////////////////////////////////////////////////////プレイヤーのターン
            if (player.wt >= enemies[0].wt)
            {
                if (player.continueMoving)
                {
                    player.StartCoroutine(player.Moving(player.destination));
                    yield return new WaitUntil(() => playerDone);
                    commandButtons.SetActive(false);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    //Debug.Log("プレイヤーを行動可能にします（ボタンを表示して押されるまで待機します");
                    commandButtons.SetActive(true);

                    if (EnemyContact())
                    {
                        AttackButton.GetComponent<Button>().interactable = true;
                    }

                    if (tilemap.WorldToCell(player.transform.position) == new Vector3Int(0, 0, 0) ||//マップ四隅にいたら脱出ボタンを選択可能に
                        tilemap.WorldToCell(player.transform.position) == new Vector3Int(0, 6, 0) ||
                        tilemap.WorldToCell(player.transform.position) == new Vector3Int(10, 0, 0) ||
                        tilemap.WorldToCell(player.transform.position) == new Vector3Int(10, 6, 0))
                    {
                        QuitButton.GetComponent<Button>().interactable = true;
                    }

                    if (AttackButtonToggle == true)
                    {
                        OnClickAttackButton();
                    }

                    yield return new WaitUntil(() => playerDone);//プレイヤーがコマンドを入力するまで待機

                    //ここからプレイヤーコマンド終了後の処理
                    //                    AttackButton.GetComponent<Button>().interactable = false;
                    QuitButton.GetComponent<Button>().interactable = false;

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
            foreach(EnemyManager enemy in enemies)
            {
                if (enemy.done == true) { enemy.wt = 0; enemy.done = false; }
            }
            if (playerDone == true) { player.wt = 0; playerDone = false; }
        }
        yield return new WaitForSeconds(0.1f);
//        Debug.Log("ステージクリア");
        StageClear();
    }

    //撃破時にEnemyManagerをRemoveしてPoolに加算する
    public void RemoveEnemy(EnemyManager enemy,int exp,int gold,int fame)
    {
        expPool += exp;
        goldPool += gold;
        famePool += fame;
        enemies.Remove(enemy);
    }

    bool EnemyContact()
    {
        for(int i = 0; i < enemies.Count; i++)
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
/*        if (AttackButtonToggle)
        {
            Debug.Log("近接攻撃の連続トグルがONになっています");
        }
        else
        {
            Debug.Log("攻撃ボタンが押されました");
        }
*/    }
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

    public void OnClickQuitButton()
    {
        ClickedButtonReset();
        Debug.Log("脱出ボタンが押されました");
        QuitConfirmButtons.SetActive(true);
    }
    public void QuitConfirm()
    {
        sceneTransitionManager.LoadTo("Home");
    }
    public void QuitCancel()
    {
        ClickedButtonReset();
    }

    public GameObject gameover;
    IEnumerator GameOver()
    {
        playerStatusSO.runtimeExp += expPool;
        playerStatusSO.runtimeGold += goldPool;
        playerStatusSO.runtimeFame += famePool;
        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame)
        {
            playerStatusSO.runtimeMaxFame = playerStatusSO.runtimeFame;
        }
        Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool}の名声を得た");
        (expPool, goldPool, famePool) = (0, 0, 0);

        Destroy(player.gameObject);
        Destroy(commandButtons);
//ここで死亡判定、引退判定
        gameover.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        sceneTransitionManager.LoadTo("Home");
    }

    public void DebugStageClear()
    {


        if (stage % 5 != 4)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Destroy(enemies[i].gameObject);
                enemies[i].DebugRemoveEnemy();
            }
            stage++;
            Setup();
        }
        else
        {
            Destroy(commandButtons);
            StartCoroutine(RankClear());
        }

    }
    public void StageClear()
    {
        battleEnd = false;

        if (stage % 5 != 4)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Destroy(enemies[i].gameObject);
                enemies.Remove(enemies[i]);
            }
            stage++;
            Setup();
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
        playerStatusSO.runtimeExp += expPool;
        playerStatusSO.runtimeGold += goldPool;
        playerStatusSO.runtimeFame += famePool;
        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame)
        {
            playerStatusSO.runtimeMaxFame = playerStatusSO.runtimeFame;
        }
        rankClear.SetActive(true);//あとで修正する。メッセージウインドウを出すなど。
        Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool}の名声を得た");
        (expPool,goldPool,famePool) = (0,0,0);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        sceneTransitionManager.LoadTo("Home");
    }

    void PoolDebug()
    {
        Debug.Log($"expPoolは{expPool}、goldPoolは{goldPool}、famePoolは{famePool}");
    }

}