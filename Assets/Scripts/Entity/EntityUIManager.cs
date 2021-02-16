using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityUIManager : MonoBehaviour
{
    [Required, AssetsOnly] public EntityView prefab;


    [Required] public GameEntities gameEntities;


    [Required, ChildGameObjectsOnly] public Transform entityParent;


    Dictionary<Entity.Entity, EntityView> views = new Dictionary<Entity.Entity, EntityView>();

    private void Awake()
    {
        var children = entityParent.GetComponentsInChildren<EntityView>();
        foreach (var child in children)
        {
            if (child.entity == null) Destroy(child.gameObject);
            else views.Add(child.entity, child);
        }

        foreach (var entity in gameEntities.entities)
        {
            if (!views.ContainsKey(entity))
            {
                var view = Instantiate(prefab, entityParent);
                view.SetEntity(entity);
                views.Add(entity, view);
            }
        }
    }
}