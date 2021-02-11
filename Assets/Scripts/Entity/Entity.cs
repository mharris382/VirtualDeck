using System;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(menuName = "Create New Entity")]
    public class Entity : ScriptableObject
    {
        public string[] shorthands;


        public int this[string abbreviation]
        {
            get => GetStatValue(abbreviation);
            set => SetStatValue(abbreviation, value);
        }
        internal StatsTable Stats
        {
            get;
            set;
        }

        public int GetStatValue(string statAbbreviation)
        {
            return Stats[statAbbreviation];
        }

        public bool HasStat(string statName)
        {
            return StatDatabase.DoesStatExist(statName);
;        }

        public void SetStatValue(string statAbbreviation, int value)
        {
            if(!HasStat(statAbbreviation))throw new FormatException($"Invalid Stat: {statAbbreviation}");
            Stats[statAbbreviation] = value;
        }
    }
}