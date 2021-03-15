using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using Sirenix.OdinInspector;
using UnityEngine;
//TODO: move this to assembly referencing entity 
public class EntityConsoleInput : MonoBehaviour, IConsoleCommand
{
    //[Required] public GameEntities gameEntities;
    public bool TryCommand(ref string command)
    {
        throw new NotImplementedException();
        //if (gameEntities.TryParseOperation(command))
        //{
        //    return true;
        //}

        command = "Invalid Format";
        return false;
    }
    
}