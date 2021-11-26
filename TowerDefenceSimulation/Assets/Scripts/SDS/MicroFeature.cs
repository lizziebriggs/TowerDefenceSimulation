using System.Collections.Generic;
using Map;
using UnityEngine;

namespace SDS
{
    public class MicroFeature : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRender;

        private int mapIndex;
        public int MapIndex
        {
            get => mapIndex;
            set => mapIndex = value;
        }

        private List<int> neighbourIndexes = new List<int>();
        public List<int> NeighbourIndexes
        {
            get => neighbourIndexes;
            set => neighbourIndexes = value;
        }

        private bool hasTower;
        public bool HasTower => hasTower;

        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                if (!MapGenerator.Instance.MapGenerated)
                {
                    MapGenerator.Instance.TriggerTowerSpawner(mapIndex);
                    return;
                }
                
                if(!hasTower) BuildTower();
                
                else DestroyTower();
            }
        }

        
        public void SetNeighbours(int mapWidth, int mapHeight)
        {
            neighbourIndexes.Add(mapIndex - (mapWidth * 5));
            neighbourIndexes.Add(mapIndex + 1);
            neighbourIndexes.Add(mapIndex + (mapWidth * 5));
            neighbourIndexes.Add(mapIndex - 1);
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
