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

        public StatsTable Stats
        {
            get;
            internal set;
        }


        public void SetAbbreviations(string[] abbrevs) => this.abbreviations = abbrevs;

        public int GetStatValue(string statAbbreviation) => Stats[statAbbreviation];

        public bool IsStatNameValid(string statName) => StatDatabase.DoesStatExist(statName);

        public void SetStatValue(string statAbbreviation, int value)
        {
            if(!IsStatNameValid(statAbbreviation))throw new FormatException($"Invalid Stat: {statAbbreviation}");
            Stats[statAbbreviation] = value;
        }

        public override string ToString() => $"{name}\n{Stats.GetStatsString()}";


        // ReSharper disable Unity.PerformanceAnalysis
        public bool IsInitialized() => Stats != null;
    }
}