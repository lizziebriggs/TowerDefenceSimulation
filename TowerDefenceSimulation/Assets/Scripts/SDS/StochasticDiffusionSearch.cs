using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SDS
{
    public class StochasticDiffusionSearch : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;

        [Header("SDS Values")]
        [SerializeField] private int populationSize;
        [SerializeField] private int maxIterations;

        [Header("UI Output")]
        [SerializeField] private Text outputLog;
    
        private List<Hypothesis> searchSpace = new List<Hypothesis>();
        private readonly List<Agent> agents = new List<Agent>();

        private int itr = 0;
        private int activeAgents;
        private int activityPercentage;

        private bool playSDS;

        
        private void Start()
        {
            outputLog.text = "";
            
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
                agent.transform.parent = mapGenerator.EnemiesParent.transform;
                var agentComponent = agent.GetComponent<Agent>();
                agents.Add(agentComponent);

                // Set up agent and hypothesis
                agentComponent.Status = false;
                agentComponent.Hypothesis = Random.Range(0, searchSpace.Count-1);
                
                // Move agent to hypothesis
                agent.gameObject.transform.position = searchSpace[agentComponent.Hypothesis].transform.position;
            }
        }

        
        private void Update()
        {
            if (itr == maxIterations || !playSDS) return;
            
            activeAgents = 0;
                
            TestPhase();
            DiffusionPhase();
                
            // Display activity statuses
            activityPercentage = activeAgents * 100 / populationSize;
            outputLog.text += "Iteration: " + itr + " Active agents: " + activityPercentage + "%\n";

            itr++;
        }

        
        public void PlaySDS()
        {
            playSDS = true;
        }

        public void PauseSDS()
        {
            playSDS = false;
        }

        
        private void TestPhase()
        {
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];
                var agentHypo = searchSpace[agent.Hypothesis];

                // Pick random micro-feature for agent to explore
                var microFeature = agentHypo.MicroFeatures[Random.Range(0, agentHypo.MicroFeatures.Count - 1)];
                
                // Move agent to micro feature
                agent.gameObject.transform.position = microFeature.gameObject.transform.position;
                
                // Evaluate hypothesis
                if (microFeature.HasTower)
                {
                    agent.Status = true;
                    activeAgents++;
                }
                else
                    agent.Status = false;
            }
        }

        
        private void DiffusionPhase()
        {
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];

                // Get inactive agents to communicate with another agent
                if (!agent.Status)
                {
                    var randomAgent = agents[Random.Range(0, populationSize - 1)];
                    
                    // If random agent is active, give agent the random agent's hypothesis
                    agent.Hypothesis = randomAgent.Status ? randomAgent.Hypothesis : Random.Range(0, searchSpace.Count - 1);
                }
            }
        }
    }
}
