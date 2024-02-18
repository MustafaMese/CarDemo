using System.Collections.Generic;
using Command;

public class CommandManager
{
    public delegate void InvokeCommandDelegate<TCommand>(TCommand e) where TCommand : ICommand;
    private delegate void InvokeCommandDelegate(ICommand e);

    public delegate ICommand InvokeReturnCommandDelegate<UCommand>(UCommand e) where UCommand : ICommand;
    private delegate ICommand InvokeReturnCommandDelegate(ICommand e);
    
    private readonly Dictionary<System.Type, InvokeCommandDelegate> _invokeCommandDelegates = new();
    private readonly Dictionary<System.Type, InvokeReturnCommandDelegate> _invokeReturnCommandDelegates = new();

    public ICommand InvokeReturnCommand(ICommand command)
    {
        if (_invokeReturnCommandDelegates.TryGetValue(command.GetType(), out var del))
            return del.Invoke(command);

        return null;
    }
    
    public void AddCommandListener<TCommand>(InvokeReturnCommandDelegate<TCommand> invokeDelegate) where TCommand : ICommand
    {
        AddReturnCommandListenerImpl(invokeDelegate);
    }
    
    private void AddReturnCommandListenerImpl<TCommand>(InvokeReturnCommandDelegate<TCommand> del) where TCommand : ICommand
    {
        InvokeReturnCommandDelegate internalDelegate = (e) => del((TCommand)e);

        if (_invokeReturnCommandDelegates.TryGetValue(typeof(TCommand), out var tempDel))
            _invokeReturnCommandDelegates[typeof(TCommand)] = tempDel + internalDelegate;
        else
            _invokeReturnCommandDelegates[typeof(TCommand)] = internalDelegate;
    }
    
    public void InvokeCommand(ICommand command)
    {
        if (_invokeCommandDelegates.TryGetValue(command.GetType(), out var del))
            del.Invoke(command);
    }
        
    public void AddCommandListener<TCommand>(InvokeCommandDelegate<TCommand> invokeDelegate) where TCommand : ICommand
    {
        AddCommandListenerImpl(invokeDelegate);
    }
        
    private void AddCommandListenerImpl<TCommand>(InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
    {
        InvokeCommandDelegate internalDelegate = (e) => del((TCommand)e);

        if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out var tempDel))
            _invokeCommandDelegates[typeof(TCommand)] = tempDel + internalDelegate;
        else
            _invokeCommandDelegates[typeof(TCommand)] = internalDelegate;
    }
}