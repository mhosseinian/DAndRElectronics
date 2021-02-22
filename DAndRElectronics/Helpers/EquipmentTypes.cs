using System.Collections.Generic;

namespace DAndRElectronics.Helpers
{
    public static class EquipmentTypes
    {
        public const string MOMENTARY = "MOMENTARY";
        public const string TOGGLE = "TOGGLE";
        public const string SEQUENTIAL = "SEQUENTIAL";
        public const string DELAY = "DELAY";
        public const string NOTUSE = "NOT USE";

        public static List<string> PossibleTypes { get; } = new List<string>
        {
            MOMENTARY,
            TOGGLE,
            SEQUENTIAL,
            DELAY,
            NOTUSE,
        };
    }
}