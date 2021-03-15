using System;

namespace Entity
{
    public static class StatUtility
    {
        public static Stat GetStatEnumFromString(string str)
        {
            if (!Enum.TryParse(str, true, out Stat stat))
                throw new StatNotFoundException(str);
            return stat;
        }

        private class StatNotFoundException : Exception {
            public StatNotFoundException(string stat) : base($"There is no stat named {stat}."){}
        }
    }
}