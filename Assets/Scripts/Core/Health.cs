using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour, IJsonSaveable {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;

        public void Start() {
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

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
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
