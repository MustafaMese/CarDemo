using Controller;
using Other;
using UnityEngine;

namespace Component
{
    public class SoundComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        private float _pitch;
    
        private void Start()
        {
            _pitch = minPitch;
            audioSource.pitch = _pitch;
        }

        public void OnRpmChanged(int oldValue, int newValue)
        {
            if(audioSource == null) return;
        
            var percentage = Utils.GetPercentage(RpmController.MaxRpm, newValue);
            var value = Utils.GetValue(1.85f, percentage);

            audioSource.pitch = Mathf.Clamp(value, minPitch, maxPitch);
        }
    }
}
