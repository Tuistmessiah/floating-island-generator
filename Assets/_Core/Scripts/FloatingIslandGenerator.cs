using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIslandGenerator : MonoBehaviour
{
    public List<GameObject> floatingStructure = null;
    public List<GameObject> floatingBiggerStructure = null;

    public float spawnDistance = 250.0f;
    public float repelDistance = 250.0f;
    public int numberOfIslands = 20;

    public float angleStep = 20f; 
    public float startingRadius = 0f;

    private float angle = 0f;
    public float interval = 5.0f;

    // TODO: Use in the future a quadrant system for performance
    private List<GameObject> islandRefs = new List<GameObject>();

    void Start() {
      for(int i = 1; i <= this.numberOfIslands; i++) CreateIsland();
      for(int i = 1; i <= this.numberOfIslands; i++) CreateBiggerIsland();

      // InvokeRepeating("CheckProximity", 1f, 5f);
      DrawPrimitives.instance.DrawBall(new Vector3(0,0,0), 2, Color.blue);
    }

    void Update() {
    }

    void CreateIsland() {
      float x = this.startingRadius * Mathf.Cos(this.angle);
      float y = Random.Range(-this.spawnDistance, this.spawnDistance);
      float z = this.startingRadius * Mathf.Sin(this.angle);
      Vector3 newPosition = new Vector3(x, y, z);

      GameObject randomPrefab = this.floatingStructure[Random.Range(0, this.floatingStructure.Count)];
      GameObject cloudInstance = Instantiate(randomPrefab, newPosition, Quaternion.identity);
      cloudInstance.transform.SetParent(this.transform);
      islandRefs.Add(cloudInstance);

      this.startingRadius += this.spawnDistance;
      this.angle += this.angleStep;
    }

    void CreateBiggerIsland() {
      float x = this.startingRadius * Mathf.Cos(this.angle);
      float y = Random.Range(-this.spawnDistance, this.spawnDistance)- 2000f;
      float z = this.startingRadius * Mathf.Sin(this.angle);
      Vector3 newPosition = new Vector3(x, y, z);

      GameObject randomPrefab = this.floatingBiggerStructure[Random.Range(0, this.floatingBiggerStructure.Count)];
      GameObject cloudInstance = Instantiate(randomPrefab, newPosition, Quaternion.identity);
      cloudInstance.transform.SetParent(this.transform);
      islandRefs.Add(cloudInstance);

      this.startingRadius += this.spawnDistance;
      this.angle += this.angleStep;     
    }

    void CheckProximity() {
      // TODO: Improve performance
      foreach(GameObject islandA in islandRefs) {
        foreach(GameObject islandB in islandRefs) {
          if(islandA == islandB) continue;
          // TODO: Check if too close
          float dist = Vector3.Distance(islandA.transform.position, islandB.transform.position);
          if(dist < this.repelDistance){
            islandA.GetComponent<RandomMovement>().ChangeCourse();
            islandB.GetComponent<RandomMovement>().ChangeCourse();
          }
          // TODO: Check if direction is contrary
          // island.GetComponent<RandomMovement>().color = new Color(1f, 1f, 1f, alphaLevel);
        }
      }
    }

    void RandomizePosition(Vector3 _center) {
    }

}

// TODO: Make this into an utility
//  public static class Utils {
 
 
//      public static Vector3 ChangeX(Vector3 v, float x)
//      {
//          return new Vector3(x, v.y, v.z);
//      }
 
//      public static Vector3 ChangeY(Vector3 v, float y)
//      {
//          return new Vector3(v.x, y, v.z);
//      }
 
//      public static Vector3 ChangeZ(Vector3 v, float z)
//      {
//          return new Vector3(v.x, v.y, z);
//      }
 
//  }