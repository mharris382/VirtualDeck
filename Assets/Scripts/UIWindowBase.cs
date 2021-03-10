using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;


//TODO: move this to core
public class UIWindowBase : MonoBehaviour
{
    [SerializeField] private UnityEvent onOpened;
    [SerializeField] private UnityEvent onClosed;

    private bool _isOpen;


    private IObservable<bool> _isOpenChanged;
    public IObservable<bool> IsOpenChanged
    {
        get
        {
            _isOpenChanged ??= _isOpenChanged.Merge(
                    onClosed.AsObservable().Select(_ => false),
                    onOpened.AsObservable().Select(_ => true));
            
            return _isOpenChanged;
        }
    }
    
    public void ToggleConsole()
    {
        if (_isOpen)
        {
            OnClose();
        }
        else
        {
            OnOpen();
        }
    }

    public void Close()
    {
        if (_isOpen)
        {
            OnClose();   
        }
    }

    public void Open()
    {
        if (!_isOpen)
        {
            OnOpen();
        }
    }

    private void OnOpen()
    {
        _isOpen = true;
        onOpened?.Invoke();
    }

    private void OnClose()
    {
        _isOpen = false;
        onClosed?.Invoke();
    }
}