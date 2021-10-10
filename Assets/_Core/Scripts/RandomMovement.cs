using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    // Stats/Definition/Constants
    [SerializeField] protected float speed = 1000.0f;
    [Tooltip("Center point the object roams around.")]
    [SerializeField] protected Vector3 wanderingCenter = Vector3.zero;
    [Tooltip("Max roaming distance from Wandering Center.")]
    [SerializeField] protected float maxRadius = 300f;
    [Tooltip("Min distance before changing to new target.")]
    [SerializeField] protected float minDistance = 10.0f;
    [SerializeField] protected float slowdownDistance = 10.0f;

    // Internal State
    private Vector3 targetPos;
    private bool isStopping = false;
    private float speedMultiplier = 1f;
    private float speedScale = 10f;

    // * Externals

    public void Initialize(Vector3 _wanderingCenter, float _maxRadius) {
      this.wanderingCenter = _wanderingCenter;
      this.maxRadius = _maxRadius;
    }

    public void ChangeCourse() {
      this.isStopping = true;
      Invoke("SetDestiny", 2.0f);
    }

    // * Internals

    void Start() {
      this.wanderingCenter = this.transform.position;
      this.targetPos = this.transform.position;
    }

    void Update() {
        if(this.isStopping) {
          this.speedMultiplier *= 0.1f;
        }

        float dist = Vector3.Distance(this.targetPos, this.transform.position);
        Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

        // Cruising
        if(dist > this.slowdownDistance) {
          velocity = Vector3.Normalize(this.targetPos - this.transform.position) * Time.deltaTime * this.speed;
          Debug.Log(dist);
        }
        // Slowing
        else if(dist <= this.slowdownDistance && dist > this.minDistance) {
          velocity = Vector3.Normalize(this.targetPos - this.transform.position) * Time.deltaTime * this.speed * 0.5f;
        }
        // Get new position
        else {
          velocity = new Vector3(0.0f, 0.0f, 0.0f);
          SetDestiny();
        }

        Debug.Log(velocity);

        // DrawPrimitives.instance.DrawRay(this.transform.position, this.transform.position + velocity * this.speedMultiplier * 1000f - this.transform.position, 0.1f);
        // DrawPrimitives.instance.DrawBall(this.transform.position + velocity * this.speedMultiplier * 1000f, 5f, Color.yellow);
        this.transform.position += velocity / this.speedScale * this.speedMultiplier;
    }

    private void SetDestiny() {
      float angleTheta = Random.Range(0.0f, 180.0f);
      float anglePhi = Random.Range(0.0f, 360.0f);
      float radius = Random.Range(0.0f, this.maxRadius);

      float xPos = this.wanderingCenter.x + radius * Mathf.Cos(anglePhi * Mathf.Deg2Rad) * Mathf.Sin(angleTheta * Mathf.Deg2Rad);
      float yPos = this.wanderingCenter.y + radius * Mathf.Cos(angleTheta * Mathf.Deg2Rad);
      float zPos = this.wanderingCenter.z + radius * Mathf.Sin(anglePhi * Mathf.Deg2Rad) * Mathf.Sin(angleTheta * Mathf.Deg2Rad);
      this.targetPos = new Vector3(xPos, yPos, zPos);
      // DrawPrimitives.instance.DrawBall(this.targetPos, this.slowdownDistance, Color.red);

      this.isStopping = false;
      this.speedMultiplier = 1f;
    }
}
