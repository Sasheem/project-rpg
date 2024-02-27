using GameDevTV.Utils;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IJsonSaveable, IModifierProvider {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon;

        private void Awake() {
            currentWeapon = new LazyValue<Weapon>(GetDefaultWeapon);
        }

        private Weapon GetDefaultWeapon() {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start() {
            currentWeapon.ForceInit();
        }

        private void Update() {
            // Time.deltaTime = the time the last frame took to render
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;
            
            // get within range
            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                // stop the NavMeshAgent 2m from the center
                // then attack
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() {
            return target;
        }
        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            // throttle the attack speed
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // triggers the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event - Hit
        void Hit() {
            // null check
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            // Debug
            Debug.Log($"Damage = {damage}");

            if (currentWeapon.value.HasProjectile()) {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            } else {
                target.TakeDamage(gameObject, damage);
            }   
        }

        // Animation Event - Shoot
        void Shoot() {
            // needs to be called Shoot to match animation event
            // has same functionality as Hit() though 
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        // Stop attacking
        public void Cancel()
        {
            TriggerStopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void TriggerStopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage) {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage) {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(currentWeapon.value.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            string weaponName = state.ToObject<string>();
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}