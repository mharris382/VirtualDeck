using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[GlobalConfig(@"Assets\Scripts\Tests\EditModeTests")]
public class EditModeTestingResources : GlobalConfig<EditModeTestingResources>
{
    [Sirenix.OdinInspector.FilePath(Extensions = "xlsx")]
    [SerializeField] private string testDatabasePath;
    
    
}