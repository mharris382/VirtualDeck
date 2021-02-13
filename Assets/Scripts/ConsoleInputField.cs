using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Entity;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ConsoleInputField : MonoBehaviour
{
    [Required] public GameEntities gameEntities;
    [Required, ChildGameObjectsOnly] public TMPro.TMP_InputField inputField;
    [Required] public MessageLog logger;
    public bool retainFocusOnSubmit;
    private LinkedList<string> consoleHistory = new LinkedList<string>();
    private LinkedListNode<string> counter;

    [FoldoutGroup("Events")] public UnityEvent onOpenedConsole;
    [FoldoutGroup("Events")] public UnityEvent onClosedConsole;

    private IDisposable disposable;
    private bool _allowInput;

    private void Awake()
    {
       var cp = new CompositeDisposable(); 
        inputField.onValueChanged.AddListener(OnConsoleChanged);
        inputField.onSubmit.AddListener(OnSubmitPressed);
    }
    
    
     private void OnApplicationQuit()
    {
        this.disposable?.Dispose();
        inputField.onValueChanged.RemoveListener(OnConsoleChanged);
        inputField.onSubmit.AddListener(OnSubmitPressed);
    }

    private void Start()
    {
        string entities = gameEntities.entities.SelectMany(t => t.Abbreviations).AsRegExPickOne();
        string stats = StatDatabase.Instance.statIds.Select(t => t.abbreviation).AsRegExPickOne();
        inputField.characterValidation = TMP_InputField.CharacterValidation.Regex;
        _allowInput = true;
        // var regex = inputField.GetType().GetField("m_RegexValue").GetMemberValue(inputField) as string;
        // regex = regex.Replace("ENTITIES", entities);
        // regex = regex.Replace("STATS", stats);
        // string regex = String.Format(@"^{0}.{1}(\s?)[\-+*\/=](\s?)\d+", entities, stats);
        // inputField.GetType().GetField("m_RegexValue", BindingFlags.NonPublic|BindingFlags.Instance).SetMemberValue(inputField, regex);

    }


    private void Update()
    {
        
        if (!GetKeyboard(out var keyboard))
            return;
        
        if (!_allowInput)
            return;

        if (keyboard.upArrowKey.wasReleasedThisFrame)
        {
            MoveHistoryCounterUp();
        }
        else if (keyboard.downArrowKey.wasReleasedThisFrame)
        {
            MoveHistoryCounterDown();
        }
    }

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

    private static bool GetKeyboard(out Keyboard keyboard)
    {
        keyboard = InputSystem.GetDevice<Keyboard>();
        if (keyboard == null)
        {
            Debug.LogError("No Keyboard");
            return false;
        }

        return false;
    }

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
        ResetCaret(); 
    }

    private void ResetCaret()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        inputField.ActivateInputField();
        inputField.caretPosition = 0;
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
        ResetCaret();
    }

    private void OnConsoleChanged(string arg0)
    {     
    }

    private void OnSubmitPressed(string arg0)
    {
        if (gameEntities.TryParseOperation(arg0))
        {
            consoleHistory.AddFirst(arg0);
           
            logger.LogMessage(arg0);
            ResetConsole();
        }
        else
        {
            logger.LogMessage("Invalid Format");
        }

        if (retainFocusOnSubmit)
        {
            ResetConsole();
        }
    }

    private void ResetConsole()
    {
        ResetHistoryCounter(); 
        inputField.SetTextWithoutNotify("");
        inputField.ActivateInputField();
        ResetCaret();
    }
}