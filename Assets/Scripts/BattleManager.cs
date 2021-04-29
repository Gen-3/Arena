using System.Collections;
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
    int stage = 0;

    public PlayerStatusSO playerStatusSO;
    int expPool = 0;
    int goldPool = 0;
    int famePool = 0;

    public Slider HPSlider;
    public int MaxHP;

    int addWtPoint = default;

    [SerializeField] GameObject selectRankPanel = default;

    private void Start()
    {
        selectRankPanel.SetActive(true);
    }

    public void SelectRank(int selectedRank)
    {
        rank = selectedRank;
        Setup(rank);
        selectRankPanel.SetActive(false);
    }


    private void Setup(int rank)//将来的には、敵のセットリストを引数に渡して敵をセットする感じ？
    {
        StopAllCoroutines();
        //プレイヤーの座標とHP、装備を戦闘開始状態に戻す
        playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
        player.hp = playerStatusSO.runtimeHp;
        MaxHP = player.hp;
        HPSlider.value = (float)player.hp / (float)MaxHP;
        player.transform.position = tilemap.CellToWorld(new Vector3Int(0, 3, 0));

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

    public EnemyManager GetEnemyOnTheTileOf(Vector3Int cellPosition)
    {
        return enemies.Find((EnemyManager obj) => obj.currentPosition == cellPosition);
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
        //敵を全滅させてbattleEnd変数がtrueになるまでループ
        while (battleEnd == false)
        {
            addWtPoint = player.agi - player.weight;
            if (addWtPoint < 10)
            {
                addWtPoint = 10;
            }

            player.wt += addWtPoint;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].hp > 0) { enemies[i].wt += enemies[i].agi; }
            }

            //WTの降順でソートしてWT最速のenemies[0]とPlayerのWTを比較、enemies[0]の方が早いか等しければenemies[0]のターン処理を開始
            enemies.Sort((a, b) => b.wt - a.wt);

            if (enemies[0].wt > player.wt)//敵のターン////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                enemies[0].StartCoroutine(enemies[0].StartEnemyTurn());
                yield return new WaitUntil　(() => enemies[0].done);

                //敵のターン終了時に、プレイヤーのHPバーを更新して戦闘不能判定
                HPSlider.value = (float)BattleManager.instance.player.hp / (float)BattleManager.instance.MaxHP;
                if (player.hp < 0)
                {
                    player.hp = 0;
                    StopAllCoroutines();
                    StartCoroutine(GameOver());
                }
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
                    if (EnemyContact())//敵接触時、近接攻撃ボタンをオン、魔法攻撃・ボウボタンをオフ
                    {
                        AttackButton.GetComponent<Button>().interactable = true;
                        MagicButton.GetComponent<Button>().interactable = false;
                        BowButton.GetComponent<Button>().interactable = false;
                        ThrowButton.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        MagicButton.GetComponent<Button>().interactable = true;
                        BowButton.GetComponent<Button>().interactable = true;
                        ThrowButton.GetComponent<Button>().interactable = true;
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

                    commandButtons.SetActive(true);

                    yield return new WaitUntil(() => playerDone);//プレイヤーがコマンドを入力するまで待機
                    Debug.Log("プレイヤーのターンが終了");
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
    public int selectedMagicID = default;
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
        //「丸々は逃げ出した」的なコンソールと、名声が下がる処理
        StartCoroutine(QuitConfirmCoroutine());
    }
    IEnumerator QuitConfirmCoroutine()
    {
        playerStatusSO.runtimeExp += expPool;
        playerStatusSO.runtimeGold += goldPool;
        playerStatusSO.runtimeFame += famePool-80;
        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame)
        {
            playerStatusSO.runtimeMaxFame = playerStatusSO.runtimeFame;
        }

        Destroy(player.gameObject);
        Destroy(commandButtons);

        if (playerStatusSO.runtimeFame > playerStatusSO.runtimeMaxFame - 100)
        {
            Debug.Log("逃げ出した……");
            Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool - 80}の名声を得た");
            (expPool, goldPool, famePool) = (0, 0, 0);

            quitPanel.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            sceneTransitionManager.LoadTo("Home");
        }
        else
        {
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

            Debug.Log("名声は地に落ちた……");
            quit2Panel.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            sceneTransitionManager.LoadTo("Home_FirstTime");
        }
    }
    public void QuitCancel()
    {
        ClickedButtonReset();
    }

    public GameObject gameoverPanel;
    public GameObject gameover2Panel;
    public IEnumerator GameOver()
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
        if (Random.Range(-100f, 100f) > player.vit)
        {
            Debug.Log("敗退した（死亡ではない）");
            Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool}の名声を得た");
            (expPool, goldPool, famePool) = (0, 0, 0);

            gameoverPanel.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            sceneTransitionManager.LoadTo("Home");
        }
        else
        {

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

            Debug.Log("死亡した（キャラクターロスト）");
            gameover2Panel.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            sceneTransitionManager.LoadTo("Home_FirstTime");
        }
    }

    public void StageClear()
    {
        battleEnd = false;

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
                goldPool = 500 ;
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

        rankClear.SetActive(true);//あとで修正する。メッセージウインドウを出すなど。
        Debug.Log($"プレイヤーは{expPool}の経験値、{goldPool}のゴールド、{famePool}の名声を得た");
        (expPool, goldPool, famePool) = (0, 0, 0);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        sceneTransitionManager.LoadTo("Home");
    }

    void PoolDebug()
    {
        Debug.Log($"expPoolは{expPool}、goldPoolは{goldPool}、famePoolは{famePool}");
    }




    private void Update()//デバッグ用コマンド！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    {
        if (Input.GetKeyDown(KeyCode.R))//Reset
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Arena");
        }
        if (Input.GetKeyDown(KeyCode.N))//NextStage
        {
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
        if (Input.GetKeyDown(KeyCode.D))// StatusLogDebug
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
            Debug.Log("addWtPoint=" + addWtPoint);
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            playerStatusSO.runtimeStr += 5;
            playerStatusSO.runtimeDex += 5;
            playerStatusSO.runtimeAgi += 5;
            playerStatusSO.runtimeVit += 5;
            playerStatusSO.runtimeMen += 5;
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
            Debug.Log("ステータス＋５");
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            playerStatusSO.runtimeStr -= 5;
            playerStatusSO.runtimeDex -= 5;
            playerStatusSO.runtimeAgi -= 5;
            playerStatusSO.runtimeVit -= 5;
            playerStatusSO.runtimeMen -= 5;
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
            Debug.Log("ステータス−５");
        }

        if (Input.GetKeyDown(KeyCode.S))//デバッグでいきなりArenaシーンを呼び出したときに能力値をセットするためのもの
        {
            playerStatusSO.runtimePlayerName = "テストプレイなう";
            playerStatusSO.runtimeStr = 40;
            playerStatusSO.runtimeDex = 40;
            playerStatusSO.runtimeAgi = 40;
            playerStatusSO.runtimeVit = 40;
            playerStatusSO.runtimeMen = 40;
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;

            SceneManager.LoadScene("Arena");
        }
    }

}