using System;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        
        [Range(1,99)]
        [SerializeField] int startingLlevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        public event Action onLevelUp;

        int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null) {
                // adds UpdateLevel() to the list to be called when 
                // onExperiencedGained is called in Experience.cs
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) {
            return GetBaseStat(stat) + GetAdditiveModifier(stat);
        }

        public float GetBaseStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            // ensure currentLevel is always set
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) {
                foreach (float modifier in provider.GetAdditiveModifier(stat)) {
                    total += modifier;
                }
            }
            return total;
        }

        // return level for character this script is attached to
        private int CalculateLevel() {
            Experience experience = GetComponent<Experience>();
            // enemies with no experience need a level too
            if (experience == null) return startingLlevel;

            // no return above, move on to getting current XP
            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++) {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP) {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
