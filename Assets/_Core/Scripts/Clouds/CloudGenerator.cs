using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject cloudPrefab = null;

    public float maxRadius = 10000f;
    public float minScale = 10f;
    public float maxScale = 10f;
    public int numberClouds = 100;

    void Start() {
      CreateClouds();
    }

    void Update() {
        
    }

    void CreateClouds() {
      for(int i = 1; i <= this.numberClouds; i++) {
        float angleTheta = Random.Range(0.0f, 180.0f);
        float anglePhi = Random.Range(0.0f, 360.0f);
        float radius = Random.Range(0.0f, this.maxRadius);
        float scale = Random.Range(this.minScale, this.maxScale);

        float xPos = this.transform.position.x + radius * Mathf.Cos(anglePhi * Mathf.Deg2Rad) * Mathf.Sin(angleTheta * Mathf.Deg2Rad);
        float yPos = this.transform.position.y + radius * Mathf.Cos(angleTheta * Mathf.Deg2Rad);
        float zPos = this.transform.position.z + radius * Mathf.Sin(anglePhi * Mathf.Deg2Rad) * Mathf.Sin(angleTheta * Mathf.Deg2Rad);

        GameObject cloudInstance = Instantiate(this.cloudPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        cloudInstance.transform.localScale = new Vector3(scale, scale, scale);
        cloudInstance.transform.localEulerAngles = Random.insideUnitSphere * 360.0f;
        cloudInstance.transform.SetParent(this.transform);
      }
    }
}
