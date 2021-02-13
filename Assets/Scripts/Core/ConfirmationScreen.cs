using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationScreen : MonoBehaviour
{
    
    
    [Required,ChildGameObjectsOnly] public Button yesButton;
    [Required,ChildGameObjectsOnly] public Button noButton;
    public bool toggleGameObjectOnDialogue = true;

    private IObservable<bool> userChoice;
    private CompositeDisposable lastDialogue;
    private IDisposable disposable;
    private void Awake()
    {
        userChoice = yesButton.OnClickAsObservable().Select(t => true).Merge(noButton.OnClickAsObservable().Select(t => false)).Where(t => enabled);
        }


    public virtual void DisplayPrompt(string prompt)
    {
        Debug.Log(prompt);
    }

    public void ShowDialogue(Action confirmAction, Action declineAction = null, string prompt = null)
    {
        if (string.IsNullOrEmpty(prompt))
            DisplayPrompt(prompt);
        
        lastDialogue?.Dispose();
        lastDialogue = new CompositeDisposable();
       
        if (toggleGameObjectOnDialogue)
        {
            gameObject.SetActive(true);
            userChoice.Take(1).Where(t => !t).Subscribe(_ => gameObject.SetActive(false)).AddTo(lastDialogue);
        }

        userChoice.Take(1).Subscribe(t =>
        {
            if (t) confirmAction?.Invoke();
            else declineAction?.Invoke();
        }).AddTo(lastDialogue);
    }
}