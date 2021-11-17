using System.Collections.Generic;
using UnityEngine;

namespace SDS
{
    public class StochasticDiffusionSearch : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;

        [Header("SDS Values")]
        [SerializeField] private int populationSize;
        [SerializeField] private int maxIterations;
    
        private List<Hypothesis> searchSpace = new List<Hypothesis>();
        private List<Agent> agents = new List<Agent>();

        private void Start()
        {
            if (mapGenerator.MapGenerated)
            {
                searchSpace = mapGenerator.Map;
            }
            
            InitialiseAgents();
        }

        private void InitialiseAgents()
        {
            for (int i = 0; i < populationSize; i++)
            {
                // Create and instantiate new agent
                var agent = Instantiate(mapGenerator.Enemy, Vector3.zero, Quaternion.Euler(0, 0, 0));
                var agentComponent = agent.GetComponent<Agent>();
                agents.Add(agentComponent);

                // Set up agent and hypothesis
                agentComponent.Status = false;
                agentComponent.Hypothesis = Random.Range(0, searchSpace.Count-1);
                
                // Move agent to hypothesis
                agent.gameObject.transform.position = searchSpace[agentComponent.Hypothesis].transform.position;
            }
        }
    }
}
