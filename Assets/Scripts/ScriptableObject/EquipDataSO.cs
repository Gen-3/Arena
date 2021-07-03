using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public abstract class WeaponSO : ScriptableObject
{
    public string equipName;
    public int weight;
    public int price;
}