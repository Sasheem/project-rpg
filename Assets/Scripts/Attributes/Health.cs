using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, IJsonSaveable {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;

        public void Start() {
            // doing this here causes an issue here, will fix later
            healthPoints = GetComponent<BaseStats>().GetHealth();
            if (healthPoints == 0) {
                Die();
            }
        }

        public bool IsDead() { return isDead; }

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0)
            {
                Die();
            }
        }

        public float GetPercentage() {
            return 100 * (healthPoints/ GetComponent<BaseStats>().GetHealth());
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<CapsuleCollider>().enabled = false;
            // anything that was running now knows it should stop running
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(healthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints = state.ToObject<float>();
            // UpdateState();
        }

    }
}
