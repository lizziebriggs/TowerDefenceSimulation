using UnityEngine;

namespace SDS
{
    public class MicroFeature : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRender;
        public SpriteRenderer SpriteRender
        {
            get => spriteRender;
            set => spriteRender = value;
        }

        public bool HasTower { get; set; }
    }
}
