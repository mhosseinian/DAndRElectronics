﻿using System.Collections.Generic;
using System.Linq;

namespace DAndRElectronics.Helpers
{
    public static class Constants
    {
        public const string MOMENTARY = "MOMENTARY";
        public const string TOGGLE = "TOGGLE";
        public const string SEQUENTIAL = "SEQUENTIAL";
        public const string DELAY = "DELAY";
        public const string NOTUSE = "NOT USE";

        public const string KeyBaseName = "KEY";
        public const string InputBaseName = "INPUT";
        public const string EventBaseName = "CAN EVENT";
        public const string SlideBaseName = "SLIDE KEY";
        public const string AnalogBaseName = "ANALOG IN";
        public const string TimerBaseName = "TIMER";
        public const string TemperatureBaseName = "TEMPERATURE";
        public const string SensorBaseName = "G SENSOR";

        public const string ON = "On";
        public const string OFF = "Off";
        public const string NOTUSED = "Not used";

        public static List<string> PossibleTypes { get; } = new List<string>
        {
            MOMENTARY,
            TOGGLE,
            SEQUENTIAL,
            DELAY,
            NOTUSE,
        };
        public static List<string> PossibleTypesSlider { get; } = new List<string>
        {
            MOMENTARY,
            DELAY,
            NOTUSE,
        };

        public static Dictionary<string, int> OnOffNotUseMappings = new Dictionary<string, int>
        {
            {OFF, 0},
            {ON, 1},
            {NOTUSED, 2},
        };

        public static Dictionary<int, string> OnOffNotUseMappingsReversed = new Dictionary<int, string>
        {
            {0, OFF},
            {1, ON},
            {2, NOTUSED},
        };


        public static IEnumerable<int> RangedEnumeration(int min, int max, int step)
        {
            return Enumerable.Range(min, max - min + 1).Where(i => (i - min) % step == 0);
        }

        public const string JsonButtonName = "buttonName";
        public const string JsonName = "name";
        public const string JsonEquipmentType = "equipmentType";
        public const string JsonPriority = "priority";
        public const string JsonOffBackgroundColor = "offBackgroundColor";
        public const string JsonOnBackgroundColor = "onBackgroundColor";
        public const string JsonPattern = "pattern";
        public const string JsonOuts = "outs";
        public const string JsonOutsPercent = "outPercent";
        public const string JsonOutsKeys = "outsKeys";
        public const string JsonDelayTime = "delayTime";
        public const string JsonTone = "tone";
        public const string JsonLed1 = "led1";
        public const string JsonLed2 = "led2";
        public const string JsonLed3 = "led3";
        public const string JsonVoltage = "voltage";
        public const string JsonVoltageGt = "voltageGT";
        public const string JsonTimer = "timer";
        public const string JsonTemperature = "temperature";
        public const string JsonEvent = "event";
        public const string JsonG = "g";
        public const string JsonSequence = "sequence";
        public const string JsonNumSequence = "nSequences";
        public const string JsonIgnition = "ignition";
        public const string JsonSync = "sync";
    }
}