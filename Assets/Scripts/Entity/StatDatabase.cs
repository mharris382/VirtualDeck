using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Entity
{
    [GlobalConfig("Resources")]
    public class StatDatabase : GlobalConfig<StatDatabase>
    {
        [System.Serializable]
        public class StatID
        {
            [HideInInspector] public int id;
            public string name;
            public string abbreviation;
            public int defaultValue = 10;
        }


        [OnValueChanged("Reset")] [SerializeField]
        public StatID[] statIds =
        {
                new StatID() {id = 0, name = "Health", abbreviation = "hp"},
                new StatID() {id = 1, name = "Speed", abbreviation = "spd"},
                new StatID() {id = 2, name = "Defense", abbreviation = "def"}
        };


        private Dictionary<string, StatID> _abrevLookup ;


        void Reset() => _abrevLookup = null;


        private int INTERNAL_GetStatID(string abbreviation)
        {
            abbreviation = abbreviation.ToLower();
            try
            {
                if (_abrevLookup.ContainsKey(abbreviation) == false) throw new KeyNotFoundException($"No Stat Found with abbreviation {abbreviation}");
            }
            catch (NullReferenceException e)
            {
                InitializeDatabase();
            }
            return _abrevLookup[abbreviation].id;
        }


        public static int[] CreateNewStatsArray() => Instance.INTERNAL_CreateNewStatsArray();
        
        
        private int[] INTERNAL_CreateNewStatsArray()
        {
            int[] arr = new int[statIds.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = statIds[i].defaultValue;
            }
            return arr;
        }

        public void InitializeDatabase()
        {
            _abrevLookup = new Dictionary<string, StatID>();
            for (int i = 0; i < statIds.Length; i++)
            {
                statIds[i].id = i;
                _abrevLookup.Add(statIds[i].abbreviation.ToLower(), statIds[i]);
            }
        }

        private bool INTERNAL_DoesStatExist(string abbreviation)
        {
            abbreviation = abbreviation.ToLower();
            if(_abrevLookup ==null)
                InitializeDatabase();
            return _abrevLookup.ContainsKey(abbreviation);
        }


        public static int GetStatID(string abrev) => Instance.INTERNAL_GetStatID(abrev);
        public static string GetStatName(int id) => Instance.statIds[id].name;
        public static bool DoesStatExist(string abbreviation) => Instance.INTERNAL_DoesStatExist(abbreviation);
    }
}