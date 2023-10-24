using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 20f;
        Health target;
        float timeSinceLastAttack = 0;

        
        private void Update() {
            // Time.deltaTime = the time the last frame took to render
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;
            
            // get within range
            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                // stop the NavMeshAgent 2m from the center
                // then attack
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            // throttle the attack speed
            if (timeSinceLastAttack > timeBetweenAttacks) {
                // triggers the Hit() event.
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
            
        }

        // Animation Event - Hit
        void Hit() {
            // need to add null check
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        // Stop attacking
        public void Cancel() {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
    }
}