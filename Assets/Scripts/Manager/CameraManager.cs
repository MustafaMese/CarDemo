using Command;
using UnityEngine;
using View;

namespace Manager
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float cameraSpeed;
        
        private bool _active;
        
        public void Initialize()
        {
            cameraPivot.SetParent(FindObjectOfType<CarView>().transform);
            cameraPivot.transform.localPosition = Vector3.up * 12f; 
            
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

        private void LateUpdate()
        {
            if(!_active) return;

            var mouseX = Input.GetAxis("Mouse X") * cameraSpeed;
            cameraPivot.eulerAngles += Vector3.up * mouseX;

        }
    }
}