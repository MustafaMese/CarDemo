namespace Other
{
    public class SpeedData<T>
    {
        public bool isStopped;
        public Observer<T> observerObj;
        public T startValue;
        public T endValue;
        public float duration;
        public State state;
    }
}