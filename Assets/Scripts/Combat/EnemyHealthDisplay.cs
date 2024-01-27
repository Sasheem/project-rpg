using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour {
        Fighter fighter;

        private void Awake() {
            // Store the player's Fighter
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();            
        }

        private void Update() {
            if (fighter.GetTarget() == null) {
                GetComponent<Text>().text = "N/A";
                return;
            }
            Health health = fighter.GetTarget();
            GetComponent<Text>().text = string.Format(
                "{0:0}/{1:0}", 
                health.GetHealthPoints(), 
                health.GetMaxHealthPoints()
            );
        }
    }
}