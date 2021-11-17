using System.Collections.Generic;
using SDS;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Values")]
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private GameObject mapParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject hypothesis;
    [SerializeField] private GameObject enemy;
    public GameObject Enemy => enemy;
    
    private Vector2 hypoSpawnPos = new Vector2(0, 0);

    private List<Hypothesis> map = new List<Hypothesis>();
    public List<Hypothesis> Map => map;
    
    private bool mapGenerated;
    public bool MapGenerated => mapGenerated;

    private void Awake()
    {
        for (int height = 0; height < mapHeight; height++)
        {
            for (int width = 0; width < mapWidth; width++)
            {
                var hypo = Instantiate(hypothesis, hypoSpawnPos, Quaternion.Euler(0, 0, 0));
                hypo.transform.parent = mapParent.transform;
                map.Add(hypo.GetComponent<Hypothesis>());
                    
                hypoSpawnPos.x += 5;
            }

            hypoSpawnPos.x = 0;
            hypoSpawnPos.y -= 5;
        }

        mapGenerated = true;
    }
}
