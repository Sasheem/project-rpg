using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        
        void Update()
        {
            // when the player dies, this will disable navMeshAgent
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        // To obey interface contract
        // Stop moving?
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }


        private void UpdateAnimator() {
            // get global velocity of nav mesh agent
            Vector3 velocity = navMeshAgent.velocity;
            // make local so it is meaningful for character
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
