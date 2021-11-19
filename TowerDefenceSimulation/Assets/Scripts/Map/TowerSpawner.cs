using System;
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
        [SerializeField] private int clusterSize;
    
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


        public void SpawnTowers()
        {
            switch (dispersion)
            {
                case DispersionType.Random:
                    RandomSpawn();
                    break;
            
                case DispersionType.Clustered:
                    ClusteredSpawn();
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void RandomSpawn()
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


        private void ClusteredSpawn()
        {
            for (int i = 0; i < towerPopulation; i++)
            {
                // Pick random micro-feature on a random hypothesis
                int randomHypoIndex = Random.Range(0, mapGenerator.Map.Count);
                Hypothesis randomHypo = mapGenerator.Map[randomHypoIndex].GetComponent<Hypothesis>();
            
                int randomMicroIndex = Random.Range(0, mapGenerator.HypothesisPrefab.GetComponent<Hypothesis>().MicroFeatures.Count);
                MicroFeature randomMicro = randomHypo.MicroFeatures[randomMicroIndex].GetComponent<MicroFeature>();

                if (randomMicro.HasTower) i--;

                else
                {
                    // Set randomMicro as start of cluster
                    var clusterStart = randomMicro;
                    clusterStart.BuildTower();
                
                    // Spawn cluster
                    for (int j = 0; j < clusterSize; j++)
                    {
                        // Place towers in cluster
                    }
                }
            }
        }
    }
}
