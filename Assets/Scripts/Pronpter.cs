using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pronpter : MonoBehaviour
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



    public static Pronpter instance;
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
        rankClearText3.text = $"名声点：{playerStatusSO.runtimeFame - BattleManager.instance.famePool}→{playerStatusSO.runtimeFame}";
    }

    public PlayerStatusSO playerStatusSO;
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
        gameOvertext1.text = $"{playerStatusSO.runtimePlayerName}は{BattleManager.instance.stage}回戦で敗退しました";
        gameOvertext2.text = $"経験点：＋{BattleManager.instance.expPool}";
        gameOvertext3.text = $"金貨：＋{BattleManager.instance.goldPool}";
        gameOvertext4.text = $"名声点：{BattleManager.instance.FameAtEntry}→{playerStatusSO.runtimeFame}";
    }
    public void GameOver2()
    {
        gameOver2text1.text = $"{playerStatusSO.runtimePlayerName}は{BattleManager.instance.stage}回戦で死亡しました...";
        gameOver2text2.text = $"最終成績：{playerStatusSO.runtimeMatchAmount}戦{playerStatusSO.runtimeWinAmount}勝";
        gameOver2text3.text = $"最終名声点：{playerStatusSO.runtimeFame}";
    }
    public void Quit()
    {
        gameOvertext1.text = $"{playerStatusSO.runtimePlayerName}は{BattleManager.instance.stage}回戦で敗退しました";
        gameOvertext2.text = $"経験点：＋{BattleManager.instance.expPool}";
        gameOvertext3.text = $"金貨：＋{BattleManager.instance.goldPool}";
        gameOvertext4.text = $"名声点：{BattleManager.instance.FameAtEntry}→{playerStatusSO.runtimeFame}";
    }
    public void Quit2()
    {
        quit2text1.text = $"{playerStatusSO.runtimePlayerName}の名声は地に堕ちた...";
        quit2text2.text = $"最終成績：{playerStatusSO.runtimeMatchAmount}戦{playerStatusSO.runtimeWinAmount}勝";
    }
}
