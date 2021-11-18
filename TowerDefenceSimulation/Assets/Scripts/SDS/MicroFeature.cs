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

        private bool hasTower;
        public bool HasTower => hasTower;

        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                if(!hasTower) BuildTower();
                
                else DestroyTower();
            }
        }

        public void BuildTower()
        {
            spriteRender.color = Color.blue;
            hasTower= true;
        }

        public void DestroyTower()
        {
            spriteRender.color = Color.white;
            hasTower= false;
        }
    }
}
