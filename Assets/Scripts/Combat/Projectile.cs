using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = true;
    Health target = null;
    float damage = 0;

    private void Start() {
        transform.LookAt(GetAimLocation());
    } 

    // Update is called once per frame
    void Update()
    {
        // protect against null
        if (target == null) return;
        // look at the target
        if (isHoming && !target.IsDead()) { 
            transform.LookAt(GetAimLocation()); 
        }
        
        // move to the target
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage) {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule =  target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    // Projectile deals damage and destory itself if collided with target
    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Health>() != target) return;
        if (target.IsDead()) return;
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
