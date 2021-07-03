using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopItemDatabaseSO : ScriptableObject
{
    //一覧
    public List<WeaponSO> EquipList = new List<WeaponSO>();
}
