using Sirenix.OdinInspector;
using UnityEngine;

namespace Entity
{
    public class EntityView : MonoBehaviour
    {
        [Required,AssetSelector]
        public Entity entity;
        
        [ChildGameObjectsOnly,Required]
        public TMPro.TextMeshProUGUI text;


        private void OnEnable()
        {
            if (entity.IsInitialized())
            {
                text.text = entity.ToString();
            }
        }

        private void OnDisable()
        {
            text.text = "";
        }

        private void Update()
        {
            if (entity.IsInitialized() == false)
            {
                text.text = "";
                return;
            }

            text.text = entity.ToString();
        }
    }
}