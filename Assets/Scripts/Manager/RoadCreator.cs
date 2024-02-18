using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class RoadCreator : MonoBehaviour
    {
        private const int TOTAL_LENGTH = 500;
        private const int ROAD_LENGTH = 10;
    
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private GameObject finishLinePrefab;
        [SerializeField] private GameObject blockPrefab;

        public static int TotalLength => TOTAL_LENGTH;

        public void Initialize()
        {
            CreateRoad();
        }

        private void CreateRoad()
        {
            var roadCount = TotalLength / ROAD_LENGTH + 5;
            for (int i = 0; i < roadCount; i++)
            {
                var road = Instantiate(roadPrefab);
                var block1 = Instantiate(blockPrefab);
                var block2 = Instantiate(blockPrefab);
            
                road.transform.position = Vector3.forward * i * ROAD_LENGTH;
                block1.transform.position = road.transform.position + new Vector3(2.4f, 0f, Random.Range(-5f, 5f));
                block2.transform.position = road.transform.position + new Vector3(-2.4f, 0f, Random.Range(-5f, 5f));
            }

            var finishLine = Instantiate(finishLinePrefab);
            finishLine.transform.position = new Vector3(0f, 0.01f, TotalLength);
        }
    }
}
