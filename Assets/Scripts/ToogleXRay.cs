using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code showcasing how to toggle the xray 

public class ToogleXRay : MonoBehaviour {
    public LayerMask defaultLayer, xRayLayer;

    bool xRayActive;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (xRayActive) {
                xRayActive = !xRayActive;
                int layerNum = (int) Mathf.Log(defaultLayer.value, 2); // defaultLayer.value returns 2^(layerIndex)
                gameObject.layer = layerNum;

                if (transform.childCount > 0) {
                    SetLayerAllChildren(transform, layerNum); 
                }
            } else {
                xRayActive = !xRayActive;
                int layerNum = (int) Mathf.Log(xRayLayer.value, 2); // defaultLayer.value returns 2^(layerIndex)
                gameObject.layer = layerNum;

                if (transform.childCount > 0) {
                    SetLayerAllChildren(transform, layerNum); 
                }
            }
        }
    }

    void SetLayerAllChildren(Transform root, int layer) {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children) {
            child.gameObject.layer = layer;
        }
    }
}
