using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int hypoSize;
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
        
        [SerializeField] private List<MicroFeature> allMicroFeatures = new List<MicroFeature>();
        public List<MicroFeature> AllMicroFeatures => allMicroFeatures;
        
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
            int mfIndex = 0;
            
            // Generate and display map
            for (int height = 0; height < mapHeight; height++)
            {
                for (int width = 0; width < mapWidth; width++)
                {
                    var hypo = Instantiate(hypothesisPrefab, hypoSpawnPos, Quaternion.Euler(0, 0, 0));
                    hypo.transform.parent = mapParent.transform;

                    var hypoComponent = hypo.GetComponent<Hypothesis>();
                    map.Add(hypoComponent);
                    
                    IndexMicroFeatures(mfIndex, hypoComponent.MicroFeatures, (map.Count-1), width, height);
                    mfIndex += hypoSize;
                    
                    hypoSpawnPos.x += hypoSize;
                }
                
                mfIndex += (hypoSize * mapWidth * hypoSize) - (hypoSize * mapWidth);

                hypoSpawnPos.x = startPos.x;
                hypoSpawnPos.y -= hypoSize;
            }

            allMicroFeatures = allMicroFeatures.OrderBy(feature => feature.MapIndex).ToList();

            if (towerSpawner.Dispersion == TowerSpawner.DispersionType.Random)
            {
                towerSpawner.RandomSpawn();
                mapGenerated = true;
            }
        }


        private void IndexMicroFeatures(int index, List<MicroFeature> microFeatures, int hypoIndex, int width, int height)
        {
            int counter = 0;
            int rowStart = index;
            
            foreach (MicroFeature mf in microFeatures)
            {
                mf.MapIndex = index;
                counter++;

                if (counter == hypoSize)
                {
                    index = rowStart + (hypoSize * mapWidth);
                    rowStart = index;
                    counter = 0;
                }

                else index++;
                
                allMicroFeatures.Add(mf);
                mf.SetNeighbours(mapWidth, mapHeight);
            }
        }


        public void TriggerTowerSpawner(int spawnStartIndex)
        {
            StartCoroutine(towerSpawner.ClusteredSpawn(spawnStartIndex));
            //towerSpawner.ClusteredSpawn(spawnStartIndex);
            mapGenerated = true;
        }
        

        public void ClearMap()
        {
            foreach (Hypothesis hypo in map)
            {
                Destroy(hypo.gameObject);
            }

            towers = 0;
            mapGenerated = false;
            allMicroFeatures.Clear();
            map.Clear();
            hypoSpawnPos = startPos;
        }
    }
}
