using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMovement : MonoBehaviour
{
    public Transform parentTransform = null;

    void Start() {

    }

    void Update() {
        this.transform.RotateAround(parentTransform.position, Vector3.up, 10f * Time.deltaTime);
    }
}
