using Map;
using UnityEngine;

namespace SDS
{
    public class MicroFeature : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRender;

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
            MapGenerator.Instance.Towers++;
            spriteRender.color = Color.blue;
            hasTower= true;
        }

        public void DestroyTower()
        {
            MapGenerator.Instance.Towers--;
            spriteRender.color = Color.white;
            hasTower= false;
        }
    }
}
