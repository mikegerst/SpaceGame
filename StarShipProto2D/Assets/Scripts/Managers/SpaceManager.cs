using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class SpaceManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> backgrounds;
        [SerializeField] private float distanceLimit = 300.0f;
        [SerializeField] private float backGroundDistanceFromPlayer = 100f;
        

        private List<GameObject> backgroundPool;
        private float playerDistanceFromCenter;
        private GameObject currentBackground;
        
        private void Start()
        {
            InvokeRepeating(nameof(CreateTheStarsAhead),1,1 );
            
            backgroundPool = new List<GameObject>();
            
            foreach (var instanceBackground in backgrounds.Select(background 
                    => Instantiate(background, new Vector3(0, 0, -5), Quaternion.identity)))
            {
                instanceBackground.SetActive(false);
                instanceBackground.transform.SetParent(gameObject.transform);
                backgroundPool.Add(instanceBackground);
            }

            currentBackground = backgroundPool[Random.Range(0, backgroundPool.Count)];
            currentBackground.transform.position = Vector3.zero;
            currentBackground.SetActive(true);
            
            
        }
        
        private void CreateTheStarsAhead()
        {
            if (!(Vector2.Distance(currentBackground.transform.position, GameManager.Player.transform.position) >=
                  distanceLimit))
            {
                //Debug.Log(Vector2.Distance(currentBackground.transform.position, GameManager.Player.transform.position));
                return;
            }
                
            ;
            var position = currentBackground.transform.position;
            var position1 = GameManager.Player.transform.position;
            Debug.Log($"DistanceFromChanging: {Vector2.Distance(position, position1)}");
            currentBackground = backgroundPool.Find(x => !x.activeInHierarchy);
                
            Debug.Log($"DistanceFromCurrentBackground: {Vector2.Distance(position, position1)}");
            Debug.Log($"CurrentBack: {currentBackground.name} {Time.time}");

            position = position1;
            position += GameManager.Player.transform.up * backGroundDistanceFromPlayer;
            currentBackground.transform.position = position;

            currentBackground.SetActive(true);
        }


    }
}

