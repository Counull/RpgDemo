using System;
using System.Collections.Generic;
using Boar;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : SerializedMonoBehaviour {
    [OdinSerialize] private List<EnemySpawnAttribute> _enemySpawnAttributesList;

    [SerializeField] private Vector2 LT;
    [SerializeField] private Vector2 RB;


    private void Awake() { }


    private void Start() {
        for (var poolId = 0; poolId < _enemySpawnAttributesList.Count; poolId++) {
            var attribute = _enemySpawnAttributesList[poolId];
            attribute.Pool.Capacity = attribute.spawnCountMax;
            for (var instanceID = 0; instanceID < attribute.spawnCountMax; instanceID++) {
                var enemy = Instantiate(attribute.enemyController, RandomInRange(), Quaternion.identity);
                enemy.PoolId = poolId;
                enemy.InstanceId = instanceID;
                attribute.Pool.Add(enemy);
                enemy.ShouldRespawn += (poolId, instanceId) => {
                    //Respawn
                    enemy.transform.position = RandomInRange();
                    enemy.gameObject.SetActive(true);
                };
            }
        }
    }


    Vector3 RandomInRange() {
        return new Vector3(Random.Range(LT.x, RB.x), 0, Random.Range(LT.y, RB.y));
    }


    private void OnDrawGizmos() {
        // Spawn范围
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(LT.x, 0, LT.y), new Vector3(RB.x, 0, LT.y));
        Gizmos.DrawLine(new Vector3(RB.x, 0, LT.y), new Vector3(RB.x, 0, RB.y));
        Gizmos.DrawLine(new Vector3(RB.x, 0, RB.y), new Vector3(LT.x, 0, RB.y));
        Gizmos.DrawLine(new Vector3(LT.x, 0, RB.y), new Vector3(LT.x, 0, LT.y));
    }
}


[Serializable]
public class EnemySpawnAttribute {
    [SerializeField] public EnemyController enemyController;
    public int spawnCountMax;
    [HideInInspector] public readonly List<EnemyController> Pool = new();
}