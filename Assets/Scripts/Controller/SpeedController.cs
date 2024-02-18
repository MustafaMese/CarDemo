using System.Threading.Tasks;
using Command;
using Manager;
using Model;
using Other;

namespace Controller
{
    public class SpeedController
    {
        private SpeedData<float> _data;
        private bool _active;
        
        private void Cancel()
        {
            if(_data == null) return;
        
            _data.isStopped = true;
        }
    
        public SpeedController()
        {
            _active = true;
            GameManager.Instance.CommandManager.AddCommandListener<UpdateSpeedCommand>(UpdateSpeedCommand);
        }

        private async void UpdateSpeedCommand(UpdateSpeedCommand e)
        {
            Cancel();
            await Task.Yield();

            if (e.SpeedData.state == State.STOP_IMMEDIATELY)
            {
                _active = false;
                e.SpeedData.observerObj.Value = 0f;
            }
            
            if(!_active) return;
            
            _data = e.SpeedData;
            await Update(_data);
        }

        private async Task Update(SpeedData<float> speedData)
        {
            var startTime = Utils.GetTime();
            var speedValue = 0f;

            while (Utils.GetTime() - startTime < speedData.duration && !_data.isStopped)
            {
                var t = (Utils.GetTime() - startTime) / speedData.duration;
                speedValue = Utils.LerpValue(speedData.startValue, speedData.endValue, t);
                speedData.observerObj.Value = Utils.ClampValue(speedValue, 0f, CarModel.MaxSpeed);
                await Task.Yield();
            }
        
            await Task.Yield();
        }
    }
}