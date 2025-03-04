using System;
using System.Collections.Generic;
using Boar;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour {
    [ShowInInspector] private Dictionary<string, EnemySpawnAttribute> _enemySpawnAttributes;
}

[Serializable]
public struct EnemySpawnAttribute {
    [ShowInInspector] public EnemyController enemyController;

    public uint spawnCountMax;
    [HideInInspector] public uint spawnCountCurrent;
}