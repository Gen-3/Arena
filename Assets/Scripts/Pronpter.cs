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

}
