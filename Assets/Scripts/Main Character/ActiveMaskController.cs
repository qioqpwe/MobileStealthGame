using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMaskController : MonoBehaviour {
    GameObject currentMask;

    public Transform instantiateMaskAtTransform;
    public Animator rigController;

    public void Equip(GameObject newMask, string maskName) {
        if (currentMask) {
            Destroy(currentMask.gameObject);
        }

        currentMask = newMask;
        currentMask.transform.SetParent(instantiateMaskAtTransform, false);
        StartCoroutine(MaskAnimation(maskName));
    }

    IEnumerator MaskAnimation(string maskName) {
        rigController.Play("equip_" + maskName);
        do {
            yield return new WaitForEndOfFrame();
        } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
    }
}
