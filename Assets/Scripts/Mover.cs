using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        
        if (hasHit) {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }

    private void UpdateAnimator() {
        // get global velocity of nav mesh agent
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        // make local so it is meaningful for character
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
}
