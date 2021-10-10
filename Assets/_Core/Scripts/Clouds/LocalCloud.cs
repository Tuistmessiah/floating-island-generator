using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCloud : MonoBehaviour
{
    public Transform parentTransform = null;

    void Start() {
        if(parentTransform) return;
        this.parentTransform = this.gameObject.GetComponentInParent<Transform>();
    }

    void Update() {
        this.transform.RotateAround(parentTransform.position, Vector3.up, 2f * Time.deltaTime);
    }
}
