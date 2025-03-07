using System;
using System.Collections.Generic;
using Boar;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
///     野猪的生成器，初始化时直接根据Prefab生成一定数量的野猪
///     当野猪死亡进行回收和重新复活野猪
///     相当于野猪的对象池
/// </summary>
public class EnemySpawner : SerializedMonoBehaviour {
    [OdinSerialize] private List<EnemySpawnAttribute> _enemySpawnAttributesList;

    [OdinSerialize] private Vector2 lt;
    [OdinSerialize] private Vector2 rb;

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
                    //当野猪死亡后会触发此事件 Respawn 
                    enemy.transform.position = RandomInRange();
                    enemy.gameObject.SetActive(true);
                };
            }
        }
    }


    /// <summary>
    ///     画出生成范围
    /// </summary>
    private void OnDrawGizmos() {
        // Spawn范围
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(lt.x, 0, lt.y), new Vector3(rb.x, 0, lt.y));
        Gizmos.DrawLine(new Vector3(rb.x, 0, lt.y), new Vector3(rb.x, 0, rb.y));
        Gizmos.DrawLine(new Vector3(rb.x, 0, rb.y), new Vector3(lt.x, 0, rb.y));
        Gizmos.DrawLine(new Vector3(lt.x, 0, rb.y), new Vector3(lt.x, 0, lt.y));
    }


    private Vector3 RandomInRange() {
        return new Vector3(Random.Range(lt.x, rb.x), 0, Random.Range(lt.y, rb.y));
    }
}


[Serializable]
public class EnemySpawnAttribute {
    [SerializeField] public EnemyController enemyController;
    public int spawnCountMax;
    [ShowInInspector] public readonly List<EnemyController> Pool = new();
}