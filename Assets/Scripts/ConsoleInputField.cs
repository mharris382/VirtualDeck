using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
 
//TODO: move this to assembly not referencing entity
public class ConsoleInputField : MonoBehaviour
{
  
    [Required, ChildGameObjectsOnly] public TMPro.TMP_InputField inputField;
    [Required] public MessageLog logger;
    public bool retainFocusOnSubmit;
    private LinkedList<string> consoleHistory = new LinkedList<string>();
    private LinkedListNode<string> counter;

    [FoldoutGroup("Events")] public UnityEvent onOpenedConsole;
    [FoldoutGroup("Events")] public UnityEvent onClosedConsole;


    private ILogger _logger;
    private IDisposable _disposable;
    private bool _allowInput;
    private IConsoleCommand _consoleCommand;
    private InputField _inputField;
    private ConsoleKeyboard keyboard = new ConsoleKeyboard();
    
    private void Awake()
    {
        _inputField = new InputField(inputField);
        _disposable = SubscribeToInputFieldEvents();
    }

    private IDisposable SubscribeToInputFieldEvents()
    { 
        void OnConsoleChanged(string arg0)
        {     
            Debug.Log($"Console Became: {arg0}");
        }
        
        void OnSubmitPressed(string arg0)
        {
            if (_consoleCommand.TryCommand(ref arg0))
            {
                consoleHistory.AddFirst(arg0);
            }
            logger.LogMessage(arg0);
        }
        
        var cp = new CompositeDisposable();
        inputField.onValueChanged.AsObservable().Subscribe(OnConsoleChanged).AddTo(cp);
        inputField.onSubmit.AsObservable().Subscribe(OnSubmitPressed).AddTo(cp);
        inputField.onSubmit.AsObservable().Where(_ => retainFocusOnSubmit).Subscribe(_ => ResetConsole()).AddTo(cp);
        return cp;
    }
    
    
    private void ResetConsole()
    {
        ResetHistoryCounter(); 
        _inputField.FocusOnField();
    }
    

    private void Start()
    {
        this.GetComponentInChildrenAndAssertExists(ref _logger);
        this.GetComponentAndAssertExists(ref _consoleCommand);
        _allowInput = true;
    }

    private void Update()
    {
        if (!_allowInput)
            return;

        if (keyboard.UpPressed())
            MoveHistoryCounterUp();
        else if (keyboard.DownPressed()) MoveHistoryCounterDown();
    }

    //TODO: refactor this into UIWindowBase and remove from this class
    public void ToggleConsole()
    {
        if (_allowInput)
        {
            _allowInput = false;
            onClosedConsole?.Invoke();
        }
        else
        {
            _allowInput = true;
            onOpenedConsole?.Invoke();
        }
    }
    
    
    //TODO: refactor history counter into separate class
    private void MoveHistoryCounterUp()
    {
        if (consoleHistory.Count == 0) return;

        if (counter == null)
            counter = consoleHistory.First;
        else if (counter.Next != null)
            counter = counter.Next;
        else
            return;
        inputField.SetTextWithoutNotify(counter.Value);
        _inputField.ResetFieldForUse();
    }

    private void ResetHistoryCounter()
    {
        counter = null;
        inputField.text = "";
    }

    private void MoveHistoryCounterDown()
    {
        if (consoleHistory.Count == 0 || counter == null) return;
        if (counter.Next == null)
        {
            ResetHistoryCounter();
        }
        else
        {
            counter = counter.Next;
        }
        
        _inputField.ResetFieldForUse();
    }


    
    private void OnApplicationQuit()
    {
        this._disposable?.Dispose();
    }
    
    private class ConsoleKeyboard
    {
        private Keyboard keyboard;
        private Keyboard GetKeyboard()
        {
            keyboard ??= InputSystem.GetDevice<Keyboard>();
            return keyboard;
        }

        public bool UpPressed()
        {
            return GetKeyboard().upArrowKey.wasPressedThisFrame;
        }

        public bool DownPressed()
        {
            return GetKeyboard().downArrowKey.wasPressedThisFrame;
        }
    }
    
    private class InputField
    {
        private TMP_InputField _inputField;

        public InputField(TMP_InputField inputField)
        {
            _inputField = inputField;
        }

        public void FocusOnField()
        {
            EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
            _inputField.ActivateInputField();
            ResetFieldForUse();
        }
    
        public void ResetFieldForUse()
        {
            _inputField.SetTextWithoutNotify("");
            _inputField.ActivateInputField();
            ResetCaret();
        }

        private void ResetCaret()
        {
            _inputField.caretPosition = 0;
        }
    
    
    }
    
    
    
   
}


