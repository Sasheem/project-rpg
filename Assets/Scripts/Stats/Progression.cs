using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level) {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            // guard against looking for a level that is not in this array
            if (levels.Length < level) return 0;
            return levels[level - 1];
        }

        // build the progression table as a Dictionary
        private void BuildLookup()
        {
            // don't run if a lookup table exists
            if (lookupTable != null) return;
            // begin building lookup table
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses) { 
                // begin building out the nested dictionary
                var statLookupTable = new Dictionary<Stat, float[]>();

                // populate the nested dictionary 
                foreach (ProgressionStat progressionStat in progressionClass.stats) {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                // add the nestet dictonary to the parent dictonary
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            // public float[] health;
        }

        [System.Serializable]
        class ProgressionStat {
            public Stat stat;
            public float[] levels;
        }
    }
}