namespace Model
{
    public class CarModel
    {
        private const float GEAR1_MAX_SPEED = 52.89f;
        private const float GEAR2_MAX_SPEED = 89.85f;
        private const float GEAR3_MAX_SPEED = 127.20f;
        private const float GEAR4_MAX_SPEED = 174.13f;
        private const float GEAR5_MAX_SPEED = 241.69f;

        private const float GEAR1_SPEED_FACTOR = 0.1f;
        private const float GEAR2_SPEED_FACTOR = 0.125f;
        private const float GEAR3_SPEED_FACTOR = 0.150f;
        private const float GEAR4_SPEED_FACTOR = 0.175f;
        private const float GEAR5_SPEED_FACTOR = 0.2f;

        private const float DECELERATION_SPEED_FACTOR = 0.01f;
        private const float DECELERATION_BRAKE_SPEED_FACTOR = 0.1f;
    
        public Observer<float> Speed;
        public Observer<int> Rpm;
        public Observer<int> GearPosition;
    
        public int MaxGear, MinGear;

        public static float DecelerationSpeedFactor => DECELERATION_SPEED_FACTOR;
        public static float DecelerationBrakeSpeedFactor => DECELERATION_BRAKE_SPEED_FACTOR;
        public static float MaxSpeed => GEAR5_MAX_SPEED;

        public float GetGearMaxSpeed()
        {
            return GearPosition.Value switch
            {
                1 => GEAR1_MAX_SPEED,
                2 => GEAR2_MAX_SPEED,
                3 => GEAR3_MAX_SPEED,
                4 => GEAR4_MAX_SPEED,
                5 => GEAR5_MAX_SPEED,
            };
        }

        public float GetSpeedFactor()
        {
            return GearPosition.Value switch
            {
                1 => GEAR1_SPEED_FACTOR,
                2 => GEAR2_SPEED_FACTOR,
                3 => GEAR3_SPEED_FACTOR,
                4 => GEAR4_SPEED_FACTOR,
                5 => GEAR5_SPEED_FACTOR,
            };
        }
    
        public CarModel()
        {
            MaxGear = 5;
            MinGear = 1;
            
            Speed = new Observer<float>(0f);
            Rpm = new Observer<int>(0);
            GearPosition = new Observer<int>(MinGear);
        }

        public void ResetGearPosition()
        {
            if (Speed.Value < GEAR1_MAX_SPEED)
                GearPosition.Value = 1;
            else if (Speed.Value < GEAR2_MAX_SPEED)
                GearPosition.Value = 2;
            else if (Speed.Value < GEAR3_MAX_SPEED)
                GearPosition.Value = 3;
            else if (Speed.Value < GEAR4_MAX_SPEED)
                GearPosition.Value = 4;
            else if (Speed.Value < GEAR5_MAX_SPEED)
                GearPosition.Value = 5;
        }
    }
}