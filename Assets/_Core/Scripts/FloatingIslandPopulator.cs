using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIslandPopulator : MonoBehaviour
{
  public int numberOfTries = 5;
  public int numberOfElements = 1;

  public int structureNumber = 1;
  public int structureVariation = 0;

  public Vector3 positionOffset = new Vector3(0.0f, 0.0f, 0.0f);
  public Vector3 positionVariation = new Vector3(0.0f, 0.0f, 0.0f);

  public Vector3 rotationOffset = new Vector3(0.0f, 0.0f, 0.0f);
  public Vector3 rotationVariation = new Vector3(0.0f, 0.0f, 0.0f);

  public Vector3 scaleOffset = new Vector3(0.0f, 0.0f, 0.0f);
  public Vector3 scaleVariation = new Vector3(0.0f, 0.0f, 0.0f);
  public bool isXZSymmetrical = true;

  public Collider baseCollider = null;
  
  public GameObject structureTerrain;
  public List<GameObject> structureModels;

  private float surfaceRadius = 0.0f; 

  private bool hasCalled = false;

  void InitiateRandomTransform(Vector3? _scaleOffset_, Vector3 _scaleVariation,
    Vector3? _rotationOffset_, Vector3 _rotationVariation
    ) {
    if(_scaleOffset_ is Vector3 _scaleOffset) {
      this.scaleOffset = _scaleOffset;
      this.scaleVariation = _scaleVariation;
    }
    if(_rotationOffset_ is Vector3 _rotationOffset) {
      this.rotationOffset = _rotationOffset;
      this.rotationVariation = _rotationVariation;
    }
  }

  void Start() {
      RandomizeTransform();

      Vector3 size = this.baseCollider.bounds.size;
      this.surfaceRadius = this.transform.localScale.x * size.x / 2;

      // Invoke("CreateGround", 0.5f);
      Invoke("PopulateGround", 1.0f);
      Invoke("CreateGround", 1.0f);
  }

  void CreateGround() {
    GameObject terrainInstance = Instantiate(this.structureTerrain, this.transform.position, Quaternion.identity);
    // meshHolder.transform.parent = transform;
    // public GameObject parentObject;            
    terrainInstance.transform.SetParent(this.transform);
    terrainInstance.GetComponent<ProceduralGenerator>().Initiate(this.surfaceRadius, 10f, 0.6f, new Vector3(.0f, .0f, .0f));
  }

  void PopulateGround() {
    int instancedElements = 0;
    // GameObject terrainInstance = Instantiate(this.structureTerrain, this.transform.position, Quaternion.identity);
    // terrainInstance.GetComponent<ProceduralGenerator>().Initiate(this.surfaceRadius, 10f, 0.6f, new Vector3(.0f, .0f, .0f));

    for (int i = 0; i < this.numberOfTries && instancedElements < this.numberOfElements; i++) {
      float randX = Random.Range(this.transform.position.x - this.surfaceRadius, this.transform.position.x + this.surfaceRadius);
      float randZ = Random.Range(this.transform.position.z - this.surfaceRadius, this.transform.position.z + this.surfaceRadius);

      Vector3? tryGroundHit = GetGroundPosition(new Vector3(randX, this.transform.position.y, randZ));
      // DrawPrimitives.instance.DrawVerticalRay(new Vector3(randX, this.transform.position.y, randZ), 50.0f);

      if(tryGroundHit is Vector3 groundHit){
          // DrawPrimitives.instance.DrawBall(groundHit, 10.0f);

          // int layerMask = LayerMask.GetMask("Structure");
          // int neighbours = Physics.OverlapSphere(groundHit, 5f, layerMask).Length;

          // if (neighbours == 0) return;

          GameObject randomPrefab = this.structureModels[Random.Range(0, structureModels.Count)];
          GameObject structureInstance = Instantiate(randomPrefab, groundHit, Quaternion.identity);
          structureInstance.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
          structureInstance.transform.SetParent(this.transform);

          if(IsOffEdge(structureInstance)) {
            Destroy(structureInstance);
            continue;
          } else {
            instancedElements++;
          }

      }
    }
  }

  // TODO: Make Util method to change only ONE coordinate in a fucking vector!

  bool IsOffEdge(GameObject _object) {
    Collider collider = _object.GetComponent<Collider>();
    float radius = collider.bounds.size.x * _object.transform.localScale.x / 2;

    // DrawPrimitives.instance.DrawBall(_object.transform.position, radius, Color.yellow);

    Vector3 projectionOrigin = new Vector3(_object.transform.position.x, _object.transform.position.y + 10.0f, _object.transform.position.z);
    bool allCollided = PhysicsCircumferenceCast(projectionOrigin, radius, -Vector3.up * 20.0f, 12);
    return !allCollided;
  }

  void RandomizeTransform() {
    // Position
    // TODO: Make the same for position
    float positionOffsetX = Random.Range(this.positionOffset.x - this.positionVariation.x, this.positionOffset.x + this.positionVariation.x);
    float positionOffsetY = Random.Range(this.positionOffset.y - this.positionVariation.y, this.positionOffset.y + this.positionVariation.y);
    float positionOffsetZ = Random.Range(this.positionOffset.z - this.positionVariation.z, this.positionOffset.z + this.positionVariation.z);
    this.transform.position += new Vector3(positionOffsetX, positionOffsetY, positionOffsetZ);

    // Rotation
    float rotationOffsetX = Random.Range(this.rotationOffset.x - this.rotationVariation.x, this.rotationOffset.x + this.rotationVariation.x);
    float rotationOffsetY = Random.Range(this.rotationOffset.y - this.rotationVariation.y, this.rotationOffset.y + this.rotationVariation.y);
    float rotationOffsetZ = Random.Range(this.rotationOffset.z - this.rotationVariation.z, this.rotationOffset.z + this.rotationVariation.z);
    this.transform.Rotate(rotationOffsetX, rotationOffsetY, rotationOffsetZ, Space.World);

    // Scale
    float scaleOffsetX = Random.Range(this.scaleOffset.x - this.scaleVariation.x, this.scaleOffset.x + this.scaleVariation.x);
    float scaleOffsetY = Random.Range(this.scaleOffset.y - this.scaleVariation.y, this.scaleOffset.y + this.scaleVariation.y);
    float scaleOffsetZ = Random.Range(this.scaleOffset.z - this.scaleVariation.z, this.scaleOffset.z + this.scaleVariation.z);
    if(this.isXZSymmetrical) scaleOffsetZ = scaleOffsetX;

    this.transform.localScale = new Vector3(scaleOffsetX, scaleOffsetY, scaleOffsetZ);
  }

  // TODO: Make more dynamic with custom layer mask and 2 modes: detect any collision OR detect any non-collision
  private bool PhysicsCircumferenceCast(Vector3 _center, float _radius, Vector3 _direction, float? _rayNumberOpt ) {
    bool allCollided = true;
    float rayNumber = 4;
    int layerMask = 1 << 3;
    if(_rayNumberOpt is float rayNumberVal) rayNumber = rayNumberVal;

    // Get any projection coordinates
    Vector3 right = Vector3.Cross(_direction, Vector3.right).normalized;
    if(right.Equals(Vector3.zero)) right = Vector3.Cross(_direction, Vector3.up).normalized;
    Vector3 up = Vector3.Cross(_direction, right).normalized;

    for(float a = 0; a < 360; a += 360 / rayNumber) {
      Vector3 newPos = _center + (right * Mathf.Cos(a * Mathf.Deg2Rad) + up * Mathf.Sin(a * Mathf.Deg2Rad)) * _radius;

      // TODO: Place DrawRays with the utils too
      // DrawPrimitives.instance.DrawRay(newPos, _direction * 100.0f);
      RaycastHit hit;
      if (Physics.Raycast (newPos, _direction, out hit, Mathf.Infinity)) {
        int layerInt = hit.transform.gameObject.layer;
      } else {
        return false;
      }
    }

    return allCollided;
  } 

  private Vector3? GetGroundPosition(Vector3 newPos) {
    RaycastHit hit;
    Vector3 rayDirection = new Vector3(0, -100, 0);
    Vector3 rayOrigin = new Vector3(newPos.x, newPos.y + 50, newPos.z);

    // DrawPrimitives.instance.DrawBall(rayOrigin, 3.0f, Color.red);

    // DrawPrimitives.instance.DrawRay(rayOrigin, rayDirection, 100f, Color.red);
    // DrawPrimitives.instance.DrawVerticalRay(newPos, 50.0f);
    // DrawPrimitives.instance.DrawRay(rayOrigin, rayDirection);
    // TODO: Place this into an Utils class
    // int layerMask = 1 << 3;
    if (Physics.Raycast (rayOrigin, rayDirection, out hit, Mathf.Infinity)) {
        int layerInt = hit.transform.gameObject.layer;
        // DrawPrimitives.instance.DrawBall(hit.transform.position, 20.0f, Color.yellow);
        // Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red, 5.0f);
        // if(layerInt != 3) return null;
        return hit.point;
    } else {
    }

    return null;
  }
}

// TODO: Make a project with most impolementations of code, examples, loops, etc