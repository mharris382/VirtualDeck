using System;
using UnityEngine;

namespace Entity
{
    public class EntityTestHelper : MonoBehaviour
    {
        public EntityView entityView;
        public GameEntities gameEntities;
        private void Awake()
        {
            entityView = GetComponentInParent<EntityView>();
            if (gameEntities == null)
                gameEntities = GameObject.FindObjectOfType<GameEntities>();
        }

        /// <summary>
        /// msg should be in the form:STAT OP VALUE <example>hp+10</example>
        /// </summary>
        /// <param name="msg"></param>
        public void ExecuteOperationOnEntity(string msg)
        {
            msg = $"{entityView.entity.Abbreviations[0]}.{msg}";
            gameEntities.TryParseOperation(msg);
        }
        
    }
}