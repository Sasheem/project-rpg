using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        private void OnDrawGizmos() {
            // cycle through all the waypoints
            for (int i = 0; i < transform.childCount; i++)
            {
                // get the child transform
                // draw a gizmo for each
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount) return 0;
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
