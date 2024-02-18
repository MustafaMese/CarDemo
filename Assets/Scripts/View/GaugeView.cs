using Component;
using Controller;
using Model;
using Other;
using TMPro;
using UnityEngine;

namespace View
{
    public class GaugeView : MonoBehaviour
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform speedGauge;
        [SerializeField] private Transform rpmGauge;
        [SerializeField] private TextMeshProUGUI gearPositionText;
        
        public void OnRpmChanged(int oldValue, int newValue)
        {
            if(rpmGauge == null) return;
            
            var percentage = Utils.GetPercentage(RpmController.MaxRpm, newValue);
            var value = Utils.GetValue(270f, percentage);

            var euler = rpmGauge.eulerAngles;
            euler.z = -value;
            rpmGauge.eulerAngles = euler;
        }

        public void OnSpeedChanged(float oldvalue, float newValue)
        {
            if(speedGauge == null) return;
            
            var percentage = Utils.GetPercentage(CarModel.MaxSpeed, newValue);
            var value = Utils.GetValue(270f, percentage);

            var euler = speedGauge.eulerAngles;
            euler.z = -value;
            speedGauge.eulerAngles = euler;
        }


        public void OnGearChanged(int oldValue, int newValue)
        {
            if(gearPositionText == null) return;
            
            gearPositionText.text = newValue.ToString();
        }
    }
}