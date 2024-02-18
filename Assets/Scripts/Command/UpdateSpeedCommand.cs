using Other;

namespace Command
{
    public class UpdateSpeedCommand : ICommand
    {
        public SpeedData<float> SpeedData;

        public UpdateSpeedCommand(SpeedData<float> speedData)
        {
            SpeedData = speedData;
        }
    }
}