using System.Collections.Generic;
using SDS;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    
    [Header("Map Values")]
    [SerializeField] private Vector2 startPos;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private int towerPopulation;
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
    
    public int TowerPopulation
    {
        get => towerPopulation;
        set => towerPopulation = value;
    }

    [Header("Prefabs")]
    [SerializeField] private GameObject hypothesis;
    [SerializeField] private GameObject enemy;
    public GameObject Enemy => enemy;
    
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
                var hypo = Instantiate(hypothesis, hypoSpawnPos, Quaternion.Euler(0, 0, 0));
                hypo.transform.parent = mapParent.transform;
                map.Add(hypo.GetComponent<Hypothesis>());
                    
                hypoSpawnPos.x += 5;
            }

            hypoSpawnPos.x = startPos.x;
            hypoSpawnPos.y -= 5;
        }

        mapGenerated = true;
        SpawnTowers();
    }

    
    private void SpawnTowers()
    {
        for (int i = 0; i < towerPopulation; i++)
        {
            // Pick random micro-feature on a random hypothesis
            int randomHypoIndex = Random.Range(0, map.Count);
            Hypothesis randomHypo = map[randomHypoIndex].GetComponent<Hypothesis>();
            
            int randomMicroIndex = Random.Range(0, hypothesis.GetComponent<Hypothesis>().MicroFeatures.Count);
            MicroFeature randomMicro = randomHypo.MicroFeatures[randomMicroIndex].GetComponent<MicroFeature>();

            // If micro-feature already has a tower on it, try again
            if (randomMicro.HasTower) i--;
            
            // Otherwise, place tower on micro-feature
            else
                randomMicro.BuildTower();
        }
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
