using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Entity
{
    public class EntityView : MonoBehaviour
    {
        [HideInPrefabAssets] [Required, AssetSelector]
        public Entity entity;

        
        public TMPro.TextMeshProUGUI entityNameLabel;


        public TMPro.TextMeshProUGUI healthText;
        public Image healthBar;


        public TMPro.TextMeshProUGUI speedText;
        public TMPro.TextMeshProUGUI defenseText;
        
        

        private void OnEnable()
        {
            if (entity != null && entity.IsInitialized())
            {
                entityNameLabel.text = entity.name;
                UpdateView(entity);
            }
        }

        private void OnDisable()
        {
            entityNameLabel.text = "";
        }

        private void Update()
        {
            if (entity.IsInitialized() == false)
            {
                // text.text = "";
                return;
            }

            UpdateView(entity);
            // text.text = entity.ToString();
        }

        void UpdateView(Entity e)
        {
            entityNameLabel.text = entity.name;
            int maxHP = entity.GetStatValue("max_hp");
            int hp = entity.GetStatValue("hp");
            int speed = entity.GetStatValue("spd");
            int defense = entity.GetStatValue("def");
            
            UpdateHP(hp, maxHP);
            UpdateSpeed(speed);
            UpdateDefense(defense);
        }

        void UpdateHP(int current, int max)
        {
            
            if (healthText != null)
            {
                string txt = $"{current}/{max}";
                healthText.text = txt;
            }

            if (healthBar != null)
            {
                float p = current / (float) max;
                healthBar.fillAmount = p;
            }
        }

        void UpdateSpeed(int speed)
        {
            string txt = speed.ToString();
            speedText.text = txt;
        }

        void UpdateDefense(int defense)
        {
            string def = defense.ToString();
            defenseText.text = def;
        }
        public void SetEntity(Entity entity1)
        {
            this.name = $"{entity1.name} View";
            this.entity = entity1;
        }
    }
}