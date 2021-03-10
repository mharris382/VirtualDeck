using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entity
{
    public interface IEntity
    {
        int GetStatValue(string statAbbreviation);
    }

    [CreateAssetMenu(menuName = "Create New Entity")]
    public class Entity : ScriptableObject, IEntity
    {
        [SerializeField]
        private string[] abbreviations;

        public string[] Abbreviations => abbreviations.ToArray();

        public void SetAbbreviations(string[] abbrevs)
        {
            this.abbreviations = abbrevs;
        }

        public int this[string abbreviation]
        {
            get => GetStatValue(abbreviation);
            set => SetStatValue(abbreviation, value);
        }
        public StatsTable Stats
        {
            get;
            internal set;
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

        public override string ToString()
        {
            return $"{name}\n{Stats.GetStatsString()}";
        }


        public bool IsInitialized()
        {
            return Stats != null;
        }
    }
}