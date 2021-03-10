using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entity
{
    public class GameEntities : MonoBehaviour
    {
        public Entity[] entities;
        
        private Dictionary<string, Entity> entityLookup = new Dictionary<string, Entity>();

        //TODO: EVERYTHING EXCEPT THE TryParseOperation METHOD SHOULD BE MOVED INTO A DEDICATED CONSTRUCTION CLASS SUCH AS DI INSTALLER 
        private void Awake()
        {
            Initialize();
        }

        //TODO: move to installer
        public void Initialize()
        {
            void LogErroneousAbbreviation(string shorthand, Object entity)
            {
                Debug.LogError($"Shorthand used by entity: {entityLookup[shorthand].name}", entityLookup[shorthand]);
                Debug.LogError($"Shorthand used by entity: {entity.name}", entity);
                throw new DuplicateNameException($"Shorthand {shorthand} is used multiple times!");
            }

            void RecordEntityAbbreviations(Entity entity)
            {
                foreach (var shorthand in entity.Abbreviations)
                {
                    if (entityLookup.ContainsKey(shorthand))
                        LogErroneousAbbreviation(shorthand, entity);
                    else
                        entityLookup.Add(shorthand, entity);
                }
            }

            foreach (var entity in entities)
            {
                entityLookup.Add(entity.name.ToLower(), entity);
                RecordEntityAbbreviations(entity);
                entity.Stats = CreateNewStatsTableInstance(entity.name);
            }
        }

        
        


        private StatsTable CreateNewStatsTableInstance(string entityName)
        {
            var go = new GameObject($"{entityName} Stats", typeof(StatsTable));
            go.transform.parent = transform;
            var table = go.GetComponent<StatsTable>();
            table.Construct(StatDatabase.CreateNewStatsArray());
            return table;
        }


        
        //TODO: ewwww this is a god method...
        public bool TryParseOperation(string operation)
        {
            
            var lexemes = operation.Split( '.', '+', '-', '*', '=', '/', ' ').ToList();
            string entityName = lexemes[0];
            
            if (entityLookup.ContainsKey(entityName) == false) {
                Debug.LogError($"Entity Named {entityName} Not Found!");
                return false;
            }

            Entity entity = entityLookup[entityName];
            string statName = lexemes[1];

            
            
            if (!entity.HasStat(statName)) {
                Debug.LogError($"Entity does not have stat {statName} or Stat abbreviation does not exist ");
                return false;
            }

            
            var mathOp =operation.Substring(entityName.Length + statName.Length + 1).Replace(" ", "");
            if (!Int32.TryParse(mathOp.Substring(1), out int number))
            {
                Debug.LogError($"Couldn't parse the string <b>{mathOp.Substring(1)}</b> to an integer");
                return false;
            }
            
            if (mathOp[0] == '=')
            {
                entity.SetStatValue(statName, number);
            }
            else if (mathOp[0] == '/')
            {
                int currentValue = entity.GetStatValue(statName);
                entity.SetStatValue(statName, currentValue/number);
            }
            else
            {
                int currentValue = entity.GetStatValue(statName);
                var exp = $"{currentValue}{mathOp[0]}{number}";
                var res = (int)new DataTable().Compute(exp, null);
                
                Debug.Log($"Expression:{exp}");
                Debug.Log($"Result:{res}");
                
                entity.SetStatValue(statName, res);
            }
            
            
            return true;
        }
    }
}

