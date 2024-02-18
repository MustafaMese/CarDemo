using Command;
using Component;
using Manager;
using Model;
using Other;

namespace Controller
{
    public class CarController
    {
        private readonly CarModel _model;
        private readonly WheelComponent _wheelComponent;
        private readonly RpmController _rpmController;
        private readonly SpeedController _speedController;

        private State _oldState;
        private State _currentState;
    
        public CarController(WheelComponent wheelComponent)
        {
            _model = new CarModel();
            _speedController = new SpeedController();
            _rpmController = new RpmController();

            _rpmController.OnShift += HandleShiftGears;
        
            _wheelComponent = wheelComponent;
            _oldState = State.NONE;
            
            GameManager.Instance.CommandManager.AddCommandListener<GameEndCommand>(GameEndCommand);
        }

        private void GameEndCommand(GameEndCommand e)
        {
            UpdateSpeed(State.STOP_IMMEDIATELY, 0, 0, 0);
        }

        private void HandleShiftGears(State state, float deltaTime)
        {
            if (_model.GearPosition.Value < _model.MaxGear)
            {
                _model.GearPosition.Value++;
                CheckState(state, deltaTime);
            }
        }

        public float Update(State state, float deltaTime)
        {
            var speed = _model.Speed.Value * deltaTime;
            _wheelComponent.Rotate(speed);
            CheckState(state, deltaTime);

            return speed;
        }

        private void CheckState(State state, float deltaTime)
        {
            _currentState = state;
        
            if (_oldState != _currentState)
            {
                var currSpeed = _model.Speed.Value;
                float endSpeed, totalDuration;

                if ((_currentState == State.FULL_THROTTLE || _currentState == State.SHIFT_GEAR) &&
                    (_oldState == State.OFF_THE_GAS || _oldState == State.ON_THE_BRAKE))
                    _model.ResetGearPosition();
                
                switch (_currentState)
                {
                    case State.FULL_THROTTLE:
                        endSpeed = _model.GetGearMaxSpeed();
                        totalDuration = (endSpeed - currSpeed) * deltaTime / _model.GetSpeedFactor();
                        UpdateSpeed(State.FULL_THROTTLE, totalDuration, endSpeed, currSpeed);
                        break;
                    case State.SHIFT_GEAR:
                        endSpeed = _model.GetGearMaxSpeed();
                        totalDuration = (endSpeed - currSpeed) * deltaTime / _model.GetSpeedFactor();
                        _model.Rpm.Value = _rpmController.DecreaseRpm(_model.Rpm.Value, _model.GearPosition.Value);
                        UpdateSpeed(State.SHIFT_GEAR, totalDuration, endSpeed, currSpeed);
                        break;
                    case State.OFF_THE_GAS:
                        endSpeed = 0f;
                        totalDuration = currSpeed * deltaTime / CarModel.DecelerationSpeedFactor;
                        UpdateSpeed(State.OFF_THE_GAS, totalDuration, endSpeed, currSpeed);
                        break;
                    case State.ON_THE_BRAKE:
                        endSpeed = 0f;
                        totalDuration = currSpeed * deltaTime / CarModel.DecelerationBrakeSpeedFactor;
                        UpdateSpeed(State.ON_THE_BRAKE, totalDuration, endSpeed, currSpeed);
                        break;
                }
            
                _oldState = _currentState;
            }
        }

        private void UpdateSpeed(State state, float totalDuration, float endSpeed, float currSpeed)
        {
            GameManager.Instance.CommandManager.InvokeCommand(new UpdateSpeedCommand(
                new SpeedData<float>
                {
                    state = state,
                    duration = totalDuration,
                    endValue = endSpeed,
                    isStopped = false,
                    observerObj = _model.Speed,
                    startValue = currSpeed
                }));

            GameManager.Instance.CommandManager.InvokeCommand(new UpdateRpmCommand(
                new SpeedData<int>
                {
                    state = state,
                    duration = totalDuration,
                    endValue = -1,
                    isStopped = false,
                    observerObj = _model.Rpm,
                    startValue = -1
                }, _model.GearPosition.Value >= _model.MaxGear));
        }

        public void AddSubscriberToSpeedValue(Observer<float>.OnValueChangeEventHandler func)
        {
            _model.Speed.OnValueChanged += func;
        }

        public void AddSubscriberToRpmValue(Observer<int>.OnValueChangeEventHandler func)
        {
            _model.Rpm.OnValueChanged += func;
        }

        public void AddSubscriberToGearPosition(Observer<int>.OnValueChangeEventHandler func)
        {
            _model.GearPosition.OnValueChanged += func;
        }
    }
}