using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SDS
{
    public class StochasticDiffusionSearch : MonoBehaviour
    {
        public enum RecruitmentModes
        {
            Passive=0,
            Active=1,
            Dual=2,
            ContextSensitive=3,
            ContextFree=4
        }
        
        [SerializeField] private MapGenerator mapGenerator;

        [Header("SDS Values")]
        [SerializeField] private int populationSize;
        [SerializeField] private int maxIterations;

        [Header("UI Output")]
        [SerializeField] private Text outputLog;
    
        private List<Hypothesis> searchSpace = new List<Hypothesis>();
        private readonly List<Agent> agents = new List<Agent>();

        private RecruitmentModes recruitment;
        public RecruitmentModes Recruitment
        {
            get => recruitment;
            set => recruitment = value;
        }

        private int itr = 0;
        private int activeAgents;
        private int activityPercentage;

        private bool playSDS;
        public bool PlaySDS
        {
            get => playSDS;
            set => playSDS = value;
        }


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
                
                // Reset agent's engagement for the diffusion phase
                agent.Engaged = false;
            }
        }

        
        private void DiffusionPhase()
        {
            switch (recruitment)
            {
                case RecruitmentModes.Passive:
                    Passive();
                    break;
                
                case RecruitmentModes.Active:
                    Active();
                    break;
                
                case RecruitmentModes.Dual:
                    Dual();
                    break;
                
                case RecruitmentModes.ContextSensitive:
                    ContextSensitive();
                    break;
                
                case RecruitmentModes.ContextFree:
                    ContextFree();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // PASSIVE RECRUITMENT:
        // Inactive agents look for other agents to communicate with, if the selected agent
        // is active, they share their hypothesis with the inactive agent
        
        private void Passive()
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

        // ACTIVE RECRUITMENT:
        // Active agents look for other agents to communicate with, if the selected agent
        // is not active or engaged, the active agent shares their hypothesis and the 
        // selected agent is set as engaged. Then if an agent is neither active or engaged,
        // they are given a new random hypothesis
        
        private void Active()
        {
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];

                // Get active agents to communicate with another agent
                if (agent.Status)
                {
                    var randomAgent = agents[Random.Range(0, populationSize - 1)];

                    // Share hypothesis is selected agent is inactive and not engaged
                    if (!randomAgent.Status && !randomAgent.Engaged)
                    {
                        randomAgent.Hypothesis = agent.Hypothesis;
                        randomAgent.Engaged = true;
                    }
                }
            }

            // Give any left over agents a new hypothesis
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];
                
                if(!agent.Status && !agent.Engaged)
                    agent.Hypothesis = Random.Range(0, searchSpace.Count-1);
            }
        }

        // DUEL RECRUITMENT:
        // Both active and inactive agents look for other agents to communicate with.
        // If an active agents selects an inactive agent that isn't engaged, it
        // shares its hypothesis and the inactive agent is set as engaged. If an
        // inactive agent selects an active agent, the active agent shares its
        // hypothesis and the inactive one is set as engaged. Then if an agent is
        // neither active or engaged, they are given a new random hypothesis
        
        private void Dual()
        {
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];

                // Get active agents to communicate with another agent
                if (agent.Status)
                {
                    var randomAgent = agents[Random.Range(0, populationSize - 1)];

                    // Share hypothesis is selected agent is inactive and not engaged
                    if (!randomAgent.Status && !randomAgent.Engaged)
                    {
                        randomAgent.Hypothesis = agent.Hypothesis;
                        randomAgent.Engaged = true;
                    }
                }
                
                // Get inactive agents to communicate with another agent
                else
                {
                    var randomAgent = agents[Random.Range(0, populationSize - 1)];
                    
                    // Random shares hypothesis if it's active and not engaged
                    if (randomAgent.Status && !randomAgent.Engaged)
                    {
                        agent.Hypothesis = randomAgent.Hypothesis;
                        agent.Engaged = true;
                    }
                }
            }

            // Give any left over agents a new hypothesis
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];
                
                if (!agent.Status && !agent.Engaged)
                    agent.Hypothesis = Random.Range(0, searchSpace.Count-1);
            }
        }

        // CONTEXT SENSITIVE:
        // Active agents look for other agents to communicate with, if the selected agent
        // is also active and has the same hypothesis, the selecting active agent is set
        // as inactive and they are given a new hypothesis
        
        private void ContextSensitive()
        {
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];

                // Get active agents to communicate with another agent
                if (agent.Status)
                {
                    var randomAgent = agents[Random.Range(0, populationSize - 1)];
                    
                    // Agent becomes inactive and gets new hypothesis if random has same hypothesis
                    if (randomAgent.Status && agent.Hypothesis == randomAgent.Hypothesis)
                    {
                        agent.Status = false;
                        agent.Hypothesis = Random.Range(0, searchSpace.Count-1);
                    }
                }
            }
        }

        // CONTEXT FREE:
        // Active agents look for other agents to communicate with, if the selected agent
        // is also agent, the selecting active agents is set to inactive and they are given
        // a new hypothesis, regardless of whether the selected agent has the same
        // hypothesis or not
        
        private void ContextFree()
        {
            for (int i = 0; i < populationSize; i++)
            {
                var agent = agents[i];

                // Get active agents to communicate with another agent
                if (agent.Status)
                {
                    var randomAgent = agents[Random.Range(0, populationSize - 1)];
                    
                    // Agent becomes inactive and gets new hypothesis if random is active
                    if (randomAgent.Status)
                    {
                        agent.Status = false;
                        agent.Hypothesis = Random.Range(0, searchSpace.Count-1);
                    }
                }
            }
        }
    }
}
