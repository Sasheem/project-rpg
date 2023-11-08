using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] float chaseDistance = 5f;
        Fighter fighter;
        Health health;
        GameObject player;
        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
        }

        // Attack player if within range, stop attacking if not
        private void Update() {
            // cheeck if player is dead
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
                fighter.Attack(player);
            } else {
                fighter.Cancel();
            }
        }

        // calculate the distance to the player
        private bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // called by unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
