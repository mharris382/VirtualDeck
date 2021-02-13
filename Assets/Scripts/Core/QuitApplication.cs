using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public ConfirmationScreen confirmationScreen;
    public void ExitApp()
    {
      
        confirmationScreen.ShowDialogue(Application.Quit);
    }
}