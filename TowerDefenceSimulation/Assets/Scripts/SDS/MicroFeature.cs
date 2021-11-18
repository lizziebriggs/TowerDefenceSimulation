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

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(!HasTower)
                {
                    spriteRender.color = Color.blue;
                    HasTower= true;
                }
                
                else
                {
                    spriteRender.color = Color.white;
                    HasTower= false;
                }
            }
        }
    }
}
