using Component;
using Controller;
using Manager;
using Other;
using UnityEngine;

namespace View
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private GaugeView gaugeViewPrefab;
        [SerializeField] private SoundComponent soundComponent;
        [SerializeField] private float wheelRadius = 0.00372f;
        [SerializeField] private Transform[] wheels;

        private CarController _carController;
        private bool _active = true;
    
        public void Setup()
        {
            var wheelController = new WheelComponent(wheelRadius, wheels);
            _carController = new CarController(wheelController);

            var gauge = Instantiate(gaugeViewPrefab);
            
            _carController.AddSubscriberToGearPosition(gauge.OnGearChanged);
            _carController.AddSubscriberToSpeedValue(gauge.OnSpeedChanged);
            _carController.AddSubscriberToRpmValue(gauge.OnRpmChanged);
            _carController.AddSubscriberToRpmValue(soundComponent.OnRpmChanged);
        }

        public void Move(int input)
        {
            if(!_active) return; 
            
            var speed = _carController.Update((State)input, Time.deltaTime);
            transform.position += transform.forward * speed;

            if (transform.position.z > RoadCreator.TotalLength)
            {
                _active = false;
                GameManager.Instance.CommandManager.InvokeCommand(new GameEndCommand());
            }
        }
    }
}