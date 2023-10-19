using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] float weaponRange = 2f;
        Transform target;
        private void Update()
        {
            if (target == null) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
            // stop the NavMeshAgent 2m from the center
                GetComponent<Mover>().Cancel();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        // Stop attacking?
        public void Cancel() {
            target = null;
        }
    }
}