using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        public void RandomSpawn()
        {
            for (int i = 0; i < towerPopulation; i++)
            {
                // Pick random micro-feature on a random hypothesis
                int randomHypoIndex = Random.Range(0, mapGenerator.Map.Count);
                Hypothesis randomHypo = mapGenerator.Map[randomHypoIndex].GetComponent<Hypothesis>();
            
                int randomMicroIndex = Random.Range(0, mapGenerator.HypothesisPrefab.GetComponent<Hypothesis>().MicroFeatures.Count);
                MicroFeature randomMicro = randomHypo.MicroFeatures[randomMicroIndex].GetComponent<MicroFeature>();

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

            // for (int t = 0; t < towerPopulation+1; t++)
            // {
            //     int currentMF = neighbourHeap[0];
            //     mapGenerator.AllMicroFeatures[currentMF].BuildTower();
            //
            //     foreach (int neighbour in mapGenerator.AllMicroFeatures[currentMF].NeighbourIndexes)
            //     {
            //         if (neighbour < 0 || neighbour >= mapGenerator.AllMicroFeatures.Count || mapGenerator.AllMicroFeatures[neighbour].HasTower)
            //             t--;
            //         
            //         else
            //         {
            //             mapGenerator.AllMicroFeatures[neighbour].BuildTower();
            //             neighbourHeap.Add(neighbour);
            //         }
            //     }
            //     
            //     if(neighbourHeap.Count > 0) neighbourHeap.RemoveAt(0);
            // }

            while (mapGenerator.Towers < towerPopulation)
            {
                int currentMF = neighbourHeap[0];
                if (!mapGenerator.AllMicroFeatures[currentMF].HasTower)
                    mapGenerator.AllMicroFeatures[currentMF].BuildTower();
            
                foreach (int neighbour in mapGenerator.AllMicroFeatures[currentMF].NeighbourIndexes)
                {
                    if (neighbour < 0 || neighbour >= mapGenerator.AllMicroFeatures.Count || mapGenerator.Towers == towerPopulation)
                        continue;
                    
                    if (!mapGenerator.AllMicroFeatures[neighbour].HasTower)
                        mapGenerator.AllMicroFeatures[neighbour].BuildTower();
                    neighbourHeap.Add(neighbour);

                    //yield return new WaitForSeconds(0.1f);
                }
                
                neighbourHeap.RemoveAt(0);
            }

            yield return null;
        }
    }
}
