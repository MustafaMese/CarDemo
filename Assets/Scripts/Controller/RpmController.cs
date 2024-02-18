using System;
using System.Threading.Tasks;
using Command;
using Manager;
using Other;

namespace Controller
{
    public class RpmController
    {
        private const int MAX_RPM = 10000;
        private const int MIN_RPM = 0;
        private const int SHIFT_RPM = 8500;
        private const int RPM_DROP_VALUE = 2000;
        
        private bool _isGearPositionAtMax;
        private bool _active;
        private SpeedData<int> _data;

        public Action<State, float> OnShift;
    
        public RpmController()
        {
            _active = true;
            
            GameManager.Instance.CommandManager.AddCommandListener<UpdateRpmCommand>(UpdateRpmCommand);
        }

        public static int MaxRpm => MAX_RPM;
        public static int MinRpm => MIN_RPM;
        public static int ShiftRpm => SHIFT_RPM;
        public static int RpmDropValue => RPM_DROP_VALUE;

        private async void UpdateRpmCommand(UpdateRpmCommand e)
        {
            Cancel();
            await Task.Yield();

            if (e.SpeedData.state == State.STOP_IMMEDIATELY)
            {
                _active = false;
                e.SpeedData.observerObj.Value = 0;
            }
            
            if(!_active) return; 
            
            _isGearPositionAtMax = e.IsGearPositionAtMax;
            _data = e.SpeedData;
            
            float endValue;
            if (e.SpeedData.state == State.FULL_THROTTLE || e.SpeedData.state == State.SHIFT_GEAR)
                endValue = _isGearPositionAtMax ? MaxRpm : ShiftRpm;
            else
                endValue = MinRpm;
            
            await Update(e.SpeedData, endValue);
            
        }

        private void Cancel()
        {
            if(_data == null) return;
        
            _data.isStopped = true;
        }
    
        private async Task Update(SpeedData<int> speedData, float endValue)
        {
            var startTime = Utils.GetTime();
            float startValue = speedData.observerObj.Value;
            
            while (Utils.GetTime() - startTime < speedData.duration && !_data.isStopped)
            {
                var t = (Utils.GetTime() - startTime) / speedData.duration;
                speedData.observerObj.Value = (int)Utils.LerpValue(startValue, endValue, t);

                await Task.Yield();
            }

            if (!_data.isStopped && (speedData.state == State.FULL_THROTTLE || speedData.state == State.SHIFT_GEAR) && (speedData.observerObj.Value >= MaxRpm - 200 || speedData.observerObj.Value >= ShiftRpm - 200))
            {
                OnShift?.Invoke(State.SHIFT_GEAR, Utils.GetFrameDuration());
            }
        }

        public int DecreaseRpm(int rpmValue, int gear)
        {
            return rpmValue - RpmDropValue + gear * 100;
        }
    }
}