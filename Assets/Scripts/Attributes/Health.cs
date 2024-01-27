using System;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, IJsonSaveable {

        [SerializeField] float regenerationPercentage = 70f;
        float healthPoints = -1f;
        bool isDead = false;

        public void Start() {
            // subscribe RegenerateHealth to onLevelUp event
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;

            // doing this here causes an issue here, will fix later
            if (healthPoints < 0) {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
        }

        public bool IsDead() { return isDead; }

        public void TakeDamage(GameObject instigator, float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage() {
            return 100 * (healthPoints/ GetComponent<BaseStats>().GetStat(Stat.Health));
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

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return; 
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(healthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints = state.ToObject<float>();
            if (healthPoints <= 0) {
                Die();
            }
            // UpdateState();
        }

    }
}
