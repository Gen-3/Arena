using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTableSO : ScriptableObject
{
    public List<EnemyGroupParent> enemyTable = new List<EnemyGroupParent>();
}

[System.Serializable]
public class EnemyGroupParent
{
    public List<EnemyGroup> enemyTable = new List<EnemyGroup>();
}

[System.Serializable]
public class EnemyGroup
{
    public List<EnemyAndPosition> enemyList = new List<EnemyAndPosition>();
}

[System.Serializable]
public class EnemyAndPosition
{
    public EnemySO enemy;
    public Vector3Int position;
}