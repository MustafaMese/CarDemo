using Other;

namespace Command
{
    public class UpdateRpmCommand : ICommand
    {
        public SpeedData<int> SpeedData;
        public readonly bool IsGearPositionAtMax;

        public UpdateRpmCommand(SpeedData<int> speedData, bool isGearPositionAtMax)
        {
            SpeedData = speedData;
            IsGearPositionAtMax = isGearPositionAtMax;
        }
    }
}