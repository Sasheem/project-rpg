using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        
        [Range(1,99)]
        [SerializeField] int startingLlevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, startingLlevel);
        }
    }
}
