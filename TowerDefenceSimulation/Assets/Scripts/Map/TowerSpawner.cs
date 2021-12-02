using System.Collections;
using System.Collections.Generic;
using SDS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class TowerSpawner : MonoBehaviour
    {
        public enum DispersionType { Random, Clustered }

        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private StochasticDiffusionSearch sds;
    
        [Header("Spawn Settings")]
        [SerializeField] private int towerPopulation;
        [SerializeField] private DispersionType dispersion;
    
        public int TowerPopulation
        {
            get => towerPopulation;
            set => towerPopulation = value;
        }
    
        public DispersionType Dispersion
        {
            get => dispersion;
            set => dispersion = value;
        }

        [Header("Clustered Settings")]
        [SerializeField] [Range(0f, 1f)] private float dispersionThreshold;
        [SerializeField] private float concentration;
        [SerializeField] private float spawningSpeed;

        public float DispersionThreshold
        {
            get => dispersionThreshold;
            set => dispersionThreshold = value;
        }

        public float Concentration
        {
            get => concentration;
            set => concentration = value;
        }


        public void RandomSpawn()
        {
            for (int i = 0; i < towerPopulation; i++)
            {
                // Pick random micro-feature
                int randomMicroIndex = Random.Range(0, mapGenerator.AllMicroFeatures.Count);
                MicroFeature randomMicro = mapGenerator.AllMicroFeatures[randomMicroIndex];

                // If micro-feature already has a tower on it, try again
                if (randomMicro.HasTower) i--;
            
                // Otherwise, place tower on micro-feature
                else
                    randomMicro.BuildTower();
            }
        }


        public IEnumerator ClusteredSpawn(int clusterStartIndex)
        {
            List<int> neighbourHeap = new List<int> {clusterStartIndex};
            
            // Keep spawning towers until population is met
            while (mapGenerator.Towers < towerPopulation)
            {
                int currentMF = neighbourHeap[0];
                if (!mapGenerator.AllMicroFeatures[currentMF].HasTower)
                    mapGenerator.AllMicroFeatures[currentMF].BuildTower();
            
                foreach (int neighbour in mapGenerator.AllMicroFeatures[currentMF].NeighbourIndexes)
                {
                    // Make sure neighbour is within the bounds
                    if (neighbour < 0 || neighbour >= mapGenerator.AllMicroFeatures.Count || mapGenerator.Towers == towerPopulation)
                        continue;

                    // Check if tower should be spawned with threshold
                    float gapThreshold = Random.Range(0f, 1f);
                    if (gapThreshold > dispersionThreshold && neighbourHeap.Count > concentration)
                        continue;
                    
                    if (!mapGenerator.AllMicroFeatures[neighbour].HasTower)
                        mapGenerator.AllMicroFeatures[neighbour].BuildTower();
                    neighbourHeap.Add(neighbour);

                    yield return new WaitForSeconds(spawningSpeed);
                }
                
                neighbourHeap.RemoveAt(0);
            }

            yield return null;
        }
    }
}
