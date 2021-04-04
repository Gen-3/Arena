using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopItemDatabaseSO shopItemDatabase = default;
    [SerializeField] GameObject listPanel = default;
    [SerializeField] ShopItemButton itemButtonPrefab = default;

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
        //
    }
}
