using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DailyChest : MonoBehaviour
{
   private int _money;
   private Vector3 _spawnPosition;
   private GameObject _prefab;
   private GameObject _coin;

   private void Start()
   {
      _coin = Resources.Load<GameObject>("GoldenCoin");
      _money = Random.Range(100, 200);
   }

   private void FixedUpdate()
   {
      if (WorldManager.Instance.dayState == WorldManager.DayState.NIGHT && !gameObject.CompareTag("Dead"))
         StartCoroutine(DestroyChest());
   }

   public static void SpawnChest()
   {
      bool spawnOnLeftSide = Random.Range(0, 50) < 25;
      Instantiate(Resources.Load<GameObject>("DailyChest"),
         spawnOnLeftSide ? new Vector2(Random.Range(-80f, -40f), 3f) : new Vector2(Random.Range(40f, 80f), 3f),
         Quaternion.identity);
   }

   public void CollectMoney()
   {
      while (_money > 0)
      {
         Instantiate(_coin, transform.position + Random.insideUnitSphere * 0.1f, Quaternion.identity);
            _money--;
      }

      StartCoroutine(DestroyChest());
   }

   private IEnumerator DestroyChest()
   {      
      yield return new WaitForSeconds(3f);
      gameObject.tag = "Dead";
      Destroy(gameObject, 5f);
   }
}
