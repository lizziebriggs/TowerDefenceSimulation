using System.Collections.Generic;
using SDS;
using UnityEngine;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        public static MapGenerator Instance;

        [SerializeField] private TowerSpawner towerSpawner;
    
        [Header("Map Values")]
        [SerializeField] private Vector2 startPos;
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        [SerializeField] private GameObject mapParent;
        [SerializeField] private GameObject enemiesParent;
        public GameObject EnemiesParent => enemiesParent;
    
        public int MapWidth
        {
            get => mapWidth;
            set => mapWidth = value;
        }
    
        public int MapHeight
        {
            get => mapHeight;
            set => mapHeight = value;
        }

        [Header("Prefabs")]
        [SerializeField] private GameObject hypothesisPrefab;
        [SerializeField] private GameObject enemyPrefab;

        public GameObject HypothesisPrefab => hypothesisPrefab;
        public GameObject EnemyPrefab => enemyPrefab;
    
        private Vector2 hypoSpawnPos;

        private List<Hypothesis> map = new List<Hypothesis>();
        public List<Hypothesis> Map => map;

        private int towers;
        public int Towers
        {
            get => towers;
            set => towers = value;
        }

        private bool mapGenerated;
        public bool MapGenerated => mapGenerated;

    
        private void Awake()
        {
            Instance = GetComponent<MapGenerator>();
        
            hypoSpawnPos = startPos;
            GenerateMap();
        }

    
        public void GenerateMap()
        {
            // Generate and display map
            for (int height = 0; height < mapHeight; height++)
            {
                for (int width = 0; width < mapWidth; width++)
                {
                    var hypo = Instantiate(hypothesisPrefab, hypoSpawnPos, Quaternion.Euler(0, 0, 0));
                    hypo.transform.parent = mapParent.transform;
                    map.Add(hypo.GetComponent<Hypothesis>());
                    
                    hypoSpawnPos.x += 5;
                }

                hypoSpawnPos.x = startPos.x;
                hypoSpawnPos.y -= 5;
            }

            mapGenerated = true;
            towerSpawner.SpawnTowers();
        }
        

        public void ClearMap()
        {
            foreach (Hypothesis hypo in map)
            {
                Destroy(hypo.gameObject);
            }

            towers = 0;
            map.Clear();
            hypoSpawnPos = startPos;
        }
    }
}
