public class Observer<T>
{
    public delegate void OnValueChangeEventHandler(T oldValue, T newValue);

    private T _currentValue;
    private T _oldValue;

    public Observer(T value)
    {
        _currentValue = value;
    }
    
    public T Value
    {
        get => _currentValue;
        set
        {
            _oldValue = _currentValue;
            _currentValue = value;
            
            OnValueChanged?.Invoke(_oldValue, _currentValue);
        }
    }

    
    public event OnValueChangeEventHandler OnValueChanged;
}