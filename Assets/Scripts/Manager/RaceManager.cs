using System.Collections;
using Command;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class RaceManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private TextMeshProUGUI timerText;
    
        private bool _timerActive;
        private float _timer;
        private float _countdown;
    
        public void Initialize()
        {
            GameManager.Instance.CommandManager.AddCommandListener<GameEndCommand>(GameEndCommand);
            StartCoroutine(StartCountdown());
        }
        
        private void GameEndCommand(GameEndCommand e)
        {
            _timerActive = false;
        }

        private IEnumerator StartCountdown()
        {
            countdownText.gameObject.SetActive(true);
            _countdown = 5f;
        
            while (_countdown > 0f)
            {
                _countdown = Mathf.Clamp(_countdown - Time.deltaTime, 0f, 5f);
                countdownText.text = _countdown.ToString("F1");
                yield return null;
            }
        
            GameManager.Instance.CommandManager.InvokeCommand(new StartGameCommand());
            StartCoroutine(StartTimer());
        }
    
    
        private IEnumerator StartTimer()
        {
            countdownText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);
            _timerActive = true;
        
            while (_timerActive)
            {
                _timer += Time.deltaTime;
                timerText.text = _timer.ToString("F1");
                yield return null;
            }   
        }

        
    }
}