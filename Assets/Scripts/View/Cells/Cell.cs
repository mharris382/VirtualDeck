using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public IntReactiveProperty cellScore;

    [HideInInspector]
    public IntReactiveProperty index = new IntReactiveProperty();
    
    public int CellScore => cellScore.Value;


    [ButtonGroup()]
    void Increase()
    {
        cellScore.Value += 1;
    }

    [ButtonGroup()]
    void Decrease()
    {
        cellScore.Value -= 1;
    }
}