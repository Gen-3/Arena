using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] Text consoleText5;
    [SerializeField] Text consoleText4;
    [SerializeField] Text consoleText3;
    [SerializeField] Text consoleText2;
    [SerializeField] Text consoleText1;


    [SerializeField] PlayerManager player;
    [SerializeField] Text weaponText;
    [SerializeField] Text sub1Text;
    [SerializeField] Text sub2Text;
    [SerializeField] Text shieldText;
    [SerializeField] Text armorText;



    public static TextManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void UpdateConsole(string text)
    {
        consoleText5.text = consoleText4.text;
        consoleText4.text = consoleText3.text;
        consoleText3.text = consoleText2.text;
        consoleText2.text = consoleText1.text;
        consoleText1.text = text;
    }

    public void ReloadEquipStatus()
    {
        if (player.weapon != null) { this.weaponText.text = player.weapon.equipName; }
        if (player.subWeapon1 != null) { this.sub1Text.text = player.subWeapon1.equipName; }
        if (player.subWeapon2 != null) { this.sub2Text.text = player.subWeapon2.equipName; }
        if (player.shield != null) { this.shieldText.text = player.shield.equipName; }
        if (player.armor != null) { this.armorText.text = player.armor.equipName; }
    }


    [SerializeField] Text rankClearText1;
    [SerializeField] Text rankClearText2;
    [SerializeField] Text rankClearText3;
    public void RankClear()
    {
        rankClearText1.text = $"経験点：＋{BattleManager.instance.expPool}";
        rankClearText2.text = $"金貨：＋{BattleManager.instance.goldPool}";
        rankClearText3.text = $"名声点：{PlayerStatusSO.Entity.runtimeFame - BattleManager.instance.famePool}→{PlayerStatusSO.Entity.runtimeFame}";
    }

//    public PlayerStatusSO playerStatusSO;
    [SerializeField] Text gameOvertext1;
    [SerializeField] Text gameOvertext2;
    [SerializeField] Text gameOvertext3;
    [SerializeField] Text gameOvertext4;
    [SerializeField] Text gameOver2text1;
    [SerializeField] Text gameOver2text2;
    [SerializeField] Text gameOver2text3;
    [SerializeField] Text quit2text1;
    [SerializeField] Text quit2text2;

    public void GameOver()
    {
        gameOvertext1.text = $"{PlayerStatusSO.Entity.runtimePlayerName}は{BattleManager.instance.stage+1}回戦で敗退しました";
        gameOvertext2.text = $"経験点：＋{BattleManager.instance.expPool}";
        gameOvertext3.text = $"金貨：＋{BattleManager.instance.goldPool}";
        gameOvertext4.text = $"名声点：{BattleManager.instance.fameAtEntry}→{PlayerStatusSO.Entity.runtimeFame}";
    }
    public void GameOver2()
    {
        gameOver2text1.text = $"{PlayerStatusSO.Entity.runtimePlayerName}は{BattleManager.instance.stage + 1}回戦で死亡しました...";
        gameOver2text2.text = $"最終成績：{PlayerStatusSO.Entity.runtimeMatchAmount}戦{PlayerStatusSO.Entity.runtimeWinAmount}勝";
        gameOver2text3.text = $"最終名声点：{PlayerStatusSO.Entity.runtimeFame}";
    }
    public void Quit()
    {
        gameOvertext1.text = $"{PlayerStatusSO.Entity.runtimePlayerName}は{BattleManager.instance.stage + 1}回戦で敗退しました";
        gameOvertext2.text = $"経験点：＋{BattleManager.instance.expPool}";
        gameOvertext3.text = $"金貨：＋{BattleManager.instance.goldPool}";
        gameOvertext4.text = $"名声点：{BattleManager.instance.fameAtEntry}→{PlayerStatusSO.Entity.runtimeFame}";
    }
    public void Quit2()
    {
        quit2text1.text = $"{PlayerStatusSO.Entity.runtimePlayerName}の名声は地に堕ちた...";
        quit2text2.text = $"最終成績：{PlayerStatusSO.Entity.runtimeMatchAmount}戦{PlayerStatusSO.Entity.runtimeWinAmount}勝";
    }

    [SerializeField] Text gameClearText;
    public void GameClear()
    {
        gameClearText.text = $"{PlayerStatusSO.Entity.runtimePlayerName}の名は";
    }


    [SerializeField] Text magicExplain;
    public void SelectMagicExplain(int id)
    {
        switch (id)
        {
        default:
                magicExplain.text = "";
                break;
            case 1:
                magicExplain.text = "敵１体を選択して使用\n対象を攻撃する";
                break;
            case 2:
                magicExplain.text = "敵１体を選択して使用\n対象の命中率を下げる";
                break;
            case 3:
                magicExplain.text = "自分を選択して使用\n自分の防御力を上げる";
                break;
            case 4:
                magicExplain.text = "敵１体を選択して使用\n対象の素早さを下げる";
                break;
            case 5:
                magicExplain.text = "敵１体を選択して使用\n対象を眠らせる";
                break;
            case 6:
                magicExplain.text = "自分を選択して使用\n自分の攻撃力を上げる";
                break;
            case 7:
                magicExplain.text = "中心となるマスを選択して使用\n中心とその周囲の計７セルにいる全ての敵を攻撃する";
                break;
            case 8:
                magicExplain.text = "自分を選択して使用\n自分の行動速度を上げる";
                break;
            case 9:
                magicExplain.text = "敵１体を選択して使用\n対象の魔法を封じる";
                break;
            case 10:
                magicExplain.text = "自分または敵１体を選択して使用\n対象にかかっている魔法の効果を打ち消す";
                break;
            case 11:
                magicExplain.text = "任意のセルを選択して使用\n全ての敵を攻撃する";
                break;
            case 12:
                magicExplain.text = "敵１体を選択して使用\n敵は死ぬ";
                break;
        }
    }
}
