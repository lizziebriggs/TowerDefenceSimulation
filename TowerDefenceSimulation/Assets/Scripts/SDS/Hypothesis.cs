using System.Collections.Generic;
using UnityEngine;

namespace SDS
{
    public class Hypothesis : MonoBehaviour
    {
        [SerializeField] private List<MicroFeature> microFeatures = new List<MicroFeature>();
        public List<MicroFeature> MicroFeatures => microFeatures;
    }
}
