using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject mainCollectable;
        public float spawnTimeReset = 10f;
        public float minRandomMovement = 10f;
        public float maxRandomMovement = 1000f;

        float spawnTime;

        [SerializeField] private Vector2 spawnLocation;
        // Start is called before the first frame update
        void Start()
        {
            spawnTime = spawnTimeReset;
        }

        // Update is called once per frame
        void Update()
        {
            spawnTime -= Time.deltaTime;

            if (spawnTime <= 0)
            {
                SpawnCollectable();
                spawnTime = Random.Range(spawnTimeReset/4,spawnTimeReset);
            }
        }

        void SpawnCollectable()
        {
            spawnLocation += (Random.Range(0F,5f) > 2.2f)?Vector2.right *Time.deltaTime * Random.Range(minRandomMovement,maxRandomMovement): Vector2.left * Time.deltaTime * Random.Range(minRandomMovement, maxRandomMovement);

            spawnLocation.x = Mathf.Clamp(spawnLocation.x, -22, 22);

            var collectable = PoolManager.Instance.RequestCollectable();

            collectable.transform.position = spawnLocation;
            //Instantiate(mainCollectable, spawnLocation, Quaternion.identity);
        }
    }
}
