using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        
        [Range(1,99)]
        [SerializeField] int startingLlevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();
        }

        private void Update() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
            }
        }

        public float GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            return currentLevel;
        }

        // return level for character this script is attached to
        public int CalculateLevel() {
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
