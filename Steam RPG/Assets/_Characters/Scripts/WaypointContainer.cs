using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WaypointContainer : MonoBehaviour
    {

        private void OnDrawGizmos()
        {
            Vector3 firsPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firsPosition;

            foreach (Transform waypoint in transform)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere(waypoint.position, 0.2f);
                Gizmos.DrawLine(previousPosition, waypoint.position);

                previousPosition = waypoint.position;
            }

            Gizmos.DrawLine(previousPosition, firsPosition);

        }

    }
}