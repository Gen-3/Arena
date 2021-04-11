using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopItemDatabaseSO shopItemDatabase = default;
    [SerializeField] GameObject listPanel = default;
    [SerializeField] ShopItemButton itemButtonPrefab = default;

    [SerializeField] GameObject confirmPanel = default;
    [SerializeField] Text confirmText = default;
    [SerializeField] GameObject confirmButton = default;

    EquipDataSO selectedEquipSO = default;

    [SerializeField] PlayerStatusSO playerStatusSO = default;

    public static ShopManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ShowItemList();
    }

    public void ShowItemList()
    {
        listPanel.SetActive(true);
        foreach(EquipDataSO equipDataSO in shopItemDatabase.EquipList)
        {
            ShopItemButton itemButton = Instantiate(itemButtonPrefab, listPanel.transform);
            itemButton.Set(equipDataSO);
        }
    }

    public void CloseItemList()
    {
        listPanel.SetActive(false);
    }

    //アイテムの購入
    public void BuyItem()
    {
        if(selectedEquipSO is WeaponSO)
        {
            if (playerStatusSO.TrySetWeapon(selectedEquipSO as WeaponSO))//as節抜いたらどうなる？
            {
                Debug.Log("武器の装備に成功（空きがあった）");
            }
            else
            {
                Debug.Log("どれと交換するか確認");
                //現在の手持ち武器3つを表示
            }
        }
        if (selectedEquipSO is ShieldSO)
        {
            if (playerStatusSO.TrySetShield(selectedEquipSO as ShieldSO))
            {
                Debug.Log("盾の装備に成功（空きがあった）");
            }
            else
            {
                Debug.Log("盾を交換していいか確認");
            }
        }
        if(selectedEquipSO is ArmorSO)
        {
            if(playerStatusSO.TrySetArmor(selectedEquipSO as ArmorSO))
            {
                Debug.Log("鎧の装備に成功（空きがあった）");
            }
            else
            {
                Debug.Log("鎧を交換していいか確認");
            }
        }
        confirmPanel.SetActive(false);
    }

    public void SelectItem(EquipDataSO equipDataSO)
    {
        selectedEquipSO = equipDataSO;
        //        Debug.Log(selectedEquipSO.equipName + "を選択");
    }

    //アイテムの購入
    public void OnConfirmBuy()
    {
        confirmText.text = $"{selectedEquipSO.equipName}を買いますか？";
        Debug.Log(selectedEquipSO.equipName);
        confirmPanel.SetActive(true);
    }

    public void OnYesBuy()
    {
        BuyItem();
    }

    public void OnNoBuy()
    {
        confirmPanel.SetActive(false);
    }

    public void OnBack()
    {
        SceneManager.LoadScene("Town");        
    }
}
