using UnityEngine;

namespace SDS
{
    public class Agent : MonoBehaviour
    {
        public bool Status { get; set; }
        public int Hypothesis { get; set; } // Reference to index in hypothesis list
        public int MicroFeature { get; set; } // Reference to index in hypothesis' micro-feature list
        
        public bool Engaged { get; set; }
    }
}
