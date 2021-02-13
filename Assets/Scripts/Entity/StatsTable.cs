using System;
using System.Text;
using UnityEngine;

namespace Entity
{
    public class StatsTable : MonoBehaviour
    {
        private int[] stats;

        public int this[string abbreviation]
        {
            get => GetStat(abbreviation);
            set => SetValue(abbreviation, value);
        }
        
        public void Construct(int[] stats)
        {
            this.stats = stats;
        }

        public int GetStat(string statAbbreviation)
        {
            int index = StatDatabase.GetStatID(statAbbreviation);
            return stats[index];
        }

        public void SetValue(string statAbbreviation, int value)
        {
            int index = StatDatabase.GetStatID(statAbbreviation);
            stats[index] = value;
        }
        
        
        public string GetStatsString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < stats.Length; i++)
            {
                sb.AppendLine($"{StatDatabase.GetStatName(i)}: {stats[i]}");
            }
            return sb.ToString();
        }
    }


    public class ParseException : Exception
    {
        public ParseException(string parseError, string parsePhase) : base($"Error Parsing During Phase:{parsePhase} \nError Phrase: <i>{parseError}</i> ")
        {
        }
    }
}