using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {   
            // can't attack then continue, don't try anything with it
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0)) {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }

            return true;
        }
    }
}