using Command;
using UnityEngine;
using View;

namespace Manager
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private CarView carViewPrefab;

        private bool _active;
        private CarView _car;
        private int _input;

        public void Initialize()
        {
            _car = Instantiate(carViewPrefab);
            _car.transform.position = Vector3.zero;
            _car.Setup();
            
            GameManager.Instance.CommandManager.AddCommandListener<StartGameCommand>(StartGameCommand);
            GameManager.Instance.CommandManager.AddCommandListener<GameEndCommand>(GameEndCommand);
        }

        private void GameEndCommand(GameEndCommand e)
        {
            _active = false;
        }

        private void StartGameCommand(StartGameCommand e)
        {
            _active = true;
        }

        private void Update()
        {
            if(!_active) return;
        
            if (Input.GetKey(KeyCode.W))
                _input = 1;
            else if (Input.GetKey(KeyCode.S))
                _input = -1;
            else
                _input = 0;
        
            _car.Move(_input);
        }
    }
}
