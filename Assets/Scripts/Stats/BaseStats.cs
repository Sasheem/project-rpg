using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        
        [Range(1,99)]
        [SerializeField] int startingLlevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetHealth() {
            return progression.GetHealth(characterClass, startingLlevel);
        }

        // temporarily give 10 xp points by default
        public float GetExperienceReward() {
            return 10;
        }
    }
}
