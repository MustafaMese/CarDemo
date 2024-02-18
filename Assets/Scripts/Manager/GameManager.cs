using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        public CommandManager CommandManager;

        [SerializeField] private InputController inputControllerPrefab;
        [SerializeField] private RaceManager raceManagerPrefab;
        [SerializeField] private CameraManager cameraManagerPrefab;
        [SerializeField] private RoadCreator roadCreatorPrefab;
        
        private void Awake()
        {
            _instance = this;
            CommandManager = new CommandManager();

            Instantiate(roadCreatorPrefab).Initialize();
            Instantiate(inputControllerPrefab).Initialize();
            Instantiate(raceManagerPrefab).Initialize();
            Instantiate(cameraManagerPrefab).Initialize();
        }

    }
}
