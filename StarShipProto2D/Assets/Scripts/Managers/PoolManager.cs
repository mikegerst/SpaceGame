using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private GameObject collectableContainer;
        [SerializeField] private GameObject collectablePrefab;
        [SerializeField] private List<GameObject> collectablePool;

        private void Start()
        {
            collectablePool = GenerateCollectables(10);
        }

        List<GameObject> GenerateCollectables(int amountOfCollectables)
        {
            for (var i = 0; i < amountOfCollectables; i++)
            {
                GenerateSingleCollectable();
            }
            
            return collectablePool;
        }

        GameObject GenerateSingleCollectable()
        {
            var collectable = Instantiate(collectablePrefab, collectableContainer.transform, true);
            collectable.SetActive(false);
            collectablePool.Add(collectable);
            return collectable;
        }

        public GameObject RequestCollectable()
        {
            foreach(var collectable in collectablePool)
            {
                if (!collectable.activeInHierarchy)
                {
                    collectable.SetActive(true);
                    return collectable;
                }
                    
            }


            var newSingle = GenerateSingleCollectable();
            return newSingle;
        }
        
    } 
}

