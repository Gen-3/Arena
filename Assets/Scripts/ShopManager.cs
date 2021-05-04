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

    [SerializeField] GameObject exchangePanel = default;
    [SerializeField] GameObject exchangeWeaponPanel = default;
    [SerializeField] Text exchangeWeaponText = default;
    [SerializeField] Text exchangeSubWeapon1Text = default;
    [SerializeField] Text exchangeSubWeapon2Text = default;

    EquipDataSO selectedEquipSO = default;

    [SerializeField] PlayerStatusSO playerStatusSO = default;

    [SerializeField] Text goldAmount = default;
 
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
        goldAmount.text = playerStatusSO.runtimeGold.ToString();
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
        if (playerStatusSO.runtimeGold < selectedEquipSO.price)
        {
            Debug.Log("所持金がたりない");
            return;
        }

        if (selectedEquipSO is WeaponSO)
        {
            if (playerStatusSO.TrySetWeapon(selectedEquipSO as WeaponSO))//as節抜いたらどうなる？
            {
                Debug.Log("武器の装備に成功（空きがあった）");
                playerStatusSO.runtimeGold -= selectedEquipSO.price;
                selectedEquipSO = default;
            }
            else
            {
                Debug.Log("どれと交換するか確認");
                exchangeWeaponText.text = playerStatusSO.runtimeWeapon.equipName;
                exchangeSubWeapon1Text.text = playerStatusSO.runtimeSubWeapon1.equipName;
                exchangeSubWeapon2Text.text = playerStatusSO.runtimeSubWeapon2.equipName;
                exchangeWeaponPanel.SetActive(true);
            }
        }
        if (selectedEquipSO is ShieldSO)
        {
            if (playerStatusSO.TrySetShield(selectedEquipSO as ShieldSO))
            {
                Debug.Log("盾の装備に成功（空きがあった）");
                playerStatusSO.runtimeGold -= selectedEquipSO.price;
                selectedEquipSO = default;
            }
            else
            {
                Debug.Log("盾を交換していいか確認");
                exchangePanel.SetActive(true);
            }
        }
        if(selectedEquipSO is ArmorSO)
        {
            if(playerStatusSO.TrySetArmor(selectedEquipSO as ArmorSO))
            {
                Debug.Log("鎧の装備に成功（空きがあった）");
                playerStatusSO.runtimeGold -= selectedEquipSO.price;
                selectedEquipSO = default;
            }
            else
            {
                Debug.Log("鎧を交換していいか確認");
                exchangePanel.SetActive(true);
            }
        }
        goldAmount.text = playerStatusSO.runtimeGold.ToString();
        confirmPanel.SetActive(false);
    }

    public void SelectItem(EquipDataSO equipDataSO)
    {
        //ボタンをクリックしたときに、引数としてそのボタンのequipDataSOを渡させてここでShopManagerのequipDataSOを書き換える
        selectedEquipSO = equipDataSO;
    }



    //アイテムの購入
    private void OnConfirmBuy()
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



    //アイテムの入れ替え（盾・鎧）
    private void OnYesExchange()
    {
        if(selectedEquipSO is ShieldSO)
        {
            playerStatusSO.runtimeShield = selectedEquipSO as ShieldSO;
        }
        else if(selectedEquipSO is ArmorSO)
        {
            playerStatusSO.runtimeArmor = selectedEquipSO as ArmorSO;
        }
        playerStatusSO.runtimeGold -= selectedEquipSO.price;
        goldAmount.text = playerStatusSO.runtimeGold.ToString();
        selectedEquipSO = default;
        exchangePanel.SetActive(false);
    }
    public void OnNoExchange()
    {
        exchangePanel.SetActive(false);
    }


    private string slot = default;
    //武器の入れ替え
    public void SelectSlotOfExchangeWeapon(string selectedSlot)//"main"/"sub1"/"sub2"//交換する部位をクリックしたとき
    {
        Debug.Log(selectedSlot+"がクリックされた");
        slot = selectedSlot;
    }
    public void OnYesExchangeWeapon()//交換する部位をクリックした後にYesボタンを押したとき
    {

        Debug.Log("selectedEquipSOは"+selectedEquipSO);
        Debug.Log("slotは"+slot);
        Debug.Log("OnYesExchangeWeapon()が呼ばれた");
        if (slot == "main")
        {
            playerStatusSO.runtimeWeapon = selectedEquipSO as WeaponSO;
            Debug.Log("メインウェポンを入れ替えました");
            playerStatusSO.runtimeGold -= selectedEquipSO.price;
            selectedEquipSO = default;
            exchangeWeaponPanel.SetActive(false);
        }
        if (slot == "sub1")
        {
            playerStatusSO.runtimeSubWeapon1 = selectedEquipSO as WeaponSO;
            Debug.Log("サブウェポン1を入れ替えました");
            playerStatusSO.runtimeGold -= selectedEquipSO.price;
            selectedEquipSO = default;
            exchangeWeaponPanel.SetActive(false);
        }
        if (slot == "sub2")
        {
            playerStatusSO.runtimeSubWeapon2 = selectedEquipSO as WeaponSO;
            Debug.Log("サブウェポン2を入れ替えました");
            playerStatusSO.runtimeGold -= selectedEquipSO.price;
            selectedEquipSO = default;
            exchangeWeaponPanel.SetActive(false);
        }
        slot = default;
        goldAmount.text = playerStatusSO.runtimeGold.ToString();
    }
    public void OnNoExchangeWeapon()
    {
        exchangeWeaponPanel.SetActive(false);
    }


    public void OnBack()
    {
        SceneManager.LoadScene("Town");        
    }
}
