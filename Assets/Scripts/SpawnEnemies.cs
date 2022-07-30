using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using TMPro;


public class SpawnEnemies : MonoBehaviour, IPoolGenerator<BaseEnemy> , ICheckpoint
{
    ObjPool<BaseEnemy> enemyPool;
    public BaseEnemy zombie;
    [Range(100,5000)]
    public float RangeToSpawn;
    public int amount=1;
    public float timeToSpawn=0.3f;
    Transform player;
    List<BaseEnemy> _spawnedEnemies= new List<BaseEnemy>();
    
   


    

    private void Start()
    {
        player=FindObjectOfType<PlayerModel>().transform;
        StartCoroutine(Spawn());
    }

    private void Awake()
    {
        enemyPool = new ObjPool<BaseEnemy>(Factory, BaseEnemy.TurnOn, BaseEnemy.TurnOff, amount, false);
        
        
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToSpawn);
            if (Vector3.Distance(player.position, transform.position) < 2500)
            {
               var enemy= enemyPool.GetObj();
                if (enemy != null) _spawnedEnemies.Add(enemy);
                
            }
        }
    }
  
    public void Recycle(BaseEnemy obj)
    {
        enemyPool.Recycle(obj);
    }

    public BaseEnemy Factory()
    {
        Vector3 randomPosition = Random.insideUnitSphere*RangeToSpawn;
        randomPosition.y = 0;
       return Instantiate(zombie, transform.position+randomPosition, transform.rotation).SetRecycleAction(Recycle);
    }

    public void Save()
    {
       
        
    }
  
   
    public void Restore()
    {
        foreach (BaseEnemy item in _spawnedEnemies)
        {
            if (item.gameObject.activeSelf)
            {
                item.StopAllCoroutines();
                item.StartCoroutine(item.RecycleCR());
                
                

            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    UnityEditor.Handles.color = Color.yellow;
    //    UnityEditor.Handles.DrawWireDisc(transform.position, transform.up,RangeToSpawn );


    //}

}
