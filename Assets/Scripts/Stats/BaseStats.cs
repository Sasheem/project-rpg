using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        
        [Range(1,99)]
        [SerializeField] int startingLlevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private void Update() {
            if (gameObject.tag == "Player") {
                print(GetLevel());
            }
        }

        public float GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        // return level for character this script is attached to
        public int GetLevel() {
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
