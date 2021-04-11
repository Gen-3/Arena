using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public int id;
    [SerializeField] Text nameText = default;
    [SerializeField] Text typeText = default;
    [SerializeField] Text priceText = default;
    [SerializeField] Text weightText = default;

    EquipDataSO equipDataSO = default;

    public void Set(EquipDataSO equipDataSO)
    {
        this.equipDataSO = equipDataSO;
        nameText.text = equipDataSO.equipName;
        if(equipDataSO is WeaponSO)
        {
            if (((WeaponSO)equipDataSO).twoHand)
            {
                typeText.text = "両";
            }
            if (((WeaponSO)equipDataSO).through)
            {
                typeText.text = "投";
            }
            if (((WeaponSO)equipDataSO).bow && ((WeaponSO)equipDataSO).twoHand)
            {
                typeText.text = "射両";
            }
            if(((WeaponSO)equipDataSO).twoHand|| ((WeaponSO)equipDataSO).through|| ((WeaponSO)equipDataSO).bow)
            {
            }
            else { typeText.text = ""; }
        }
        priceText.text = equipDataSO.price.ToString();
        weightText.text = equipDataSO.weight.ToString();

    }

    public void OnClickThis()
    {
        ShopManager.instance.SelectItem(equipDataSO);
    }

    public void SendID()
    {

    }
}
