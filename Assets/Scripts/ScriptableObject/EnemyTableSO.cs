using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTableSO : ScriptableObject
{
    public List<EnemyGroupParent> enemyGroupParent = new List<EnemyGroupParent>();//最も上位で全てを含む一覧リスト、SO自体
}

[System.Serializable]
public class EnemyGroupParent
{
    public List<EnemyGroupSetList> enemyGroupsSetList = new List<EnemyGroupSetList>();//そのステージで登場する敵グループの候補
}

[System.Serializable]
public class EnemyGroupSetList
{
    public List<EnemyAndPosition> enemyAndPositions = new List<EnemyAndPosition>();//
}

[System.Serializable]
public class EnemyAndPosition
{
    public EnemySO enemy;
    public Vector3Int position;
}