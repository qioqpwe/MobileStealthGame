using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour {
    public Color spheresColor = Color.blue, lineColors = Color.red;
    public float spheresRadius = 1f;
    public bool isClosedLoop, isSingleWaypoint;
    public float[] awaitTimeAtWaypoint;

    bool openLoopReturnal;

    void OnDrawGizmos() {
        Gizmos.color = spheresColor;
        foreach (Transform t in transform) {
            Gizmos.DrawWireSphere(t.position, spheresRadius);
        }

        Gizmos.color = lineColors;
        int i;
        for (i = 0; i < transform.childCount - 1; i++) {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        if (isClosedLoop) {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(0).position);
        }
    }

    public float GetCurrentAwaitTime(int currentIndex) {
        return awaitTimeAtWaypoint[currentIndex];
    }

    public Transform GetCurrentWaypoint(int currentIndex) {
        return transform.GetChild(currentIndex).transform;
    }

    public Transform GetNextWaypoint(ref int currentIndex) {
        if (isClosedLoop) {
            if (currentIndex == transform.childCount - 1) {
                currentIndex = 0;
                return transform.GetChild(currentIndex);
            }
            currentIndex += 1;
            return transform.GetChild(currentIndex);
        } else {
            if (currentIndex == transform.childCount - 1) {
                openLoopReturnal = true;
            } else if (currentIndex == 0) {
                openLoopReturnal = false;
            }

            if (openLoopReturnal) {
                currentIndex -= 1;
                return transform.GetChild(currentIndex);
            } else {
                currentIndex += 1;
                return transform.GetChild(currentIndex);
            }
        }
    }
}
