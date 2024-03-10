using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, IJsonSaveable {

        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] UnityEvent<float> takeDamage;
        LazyValue<float> healthPoints;
        bool isDead = false;

        private void Awake() {
            // not including () on function makes it a delegate?
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth() {
            return GetMaxHealthPoints();
        }

        private void Start() {
            // backup to making sure health is init'd if it hasn't by this point
            healthPoints.ForceInit();
        }

        private void OnEnable() {
            // subscribe RegenerateHealth to onLevelUp event
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            // subscribe RegenerateHealth to onLevelUp event
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead() { return isDead; }

        public void TakeDamage(GameObject instigator, float damage) {
            print(gameObject.name + " took damage: " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            
            if(healthPoints.value == 0) {
                Die();
                AwardExperience(instigator);
            } else {
                print("Invoking takeDamage event");
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints() {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage() {
            return 100 * GetFraction();
        }

        public float GetFraction() {
            return healthPoints.value / GetMaxHealthPoints();
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
            float regenHealthPoints = GetMaxHealthPoints() * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(healthPoints.value);
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints.value = state.ToObject<float>();
            if (healthPoints.value <= 0) {
                Die();
            }
        }

    }
}
