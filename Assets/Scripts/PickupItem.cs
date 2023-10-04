using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {
    public GameObject maskObject;
    public string objectName;
    void OnTriggerEnter(Collider other) {
        ActiveMaskController activeMaskController = other.gameObject.GetComponent<ActiveMaskController>();
        if (activeMaskController) {
            GameObject newMask = Instantiate(maskObject);
            activeMaskController.Equip(newMask, objectName);
        }
    }
}
