/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

public class Enemy : MonoBehaviour {
  public float maxHealth = 100f;
  public float health = 100f;
  public float moveSpeed = 3f;
  public int goldDrop = 10;
  public float timeEnemyStaysFrozenInSeconds = 2f;

  public bool frozen;

  public int pathIndex = 0; // The index of the path this enemy is moving along

  private int wayPointIndex = 0; // Node this enemy is moving to
  private float freezeTimer;

  void Start() {
    EnemyManager.Instance.RegisterEnemy(this);
  }

  void Update() {
    // Only allow movement if this enemy hasn't arrived at the last waypoint yet
    if (wayPointIndex < WayPointManager.Instance.Paths[pathIndex].WayPoints.Count) {
      UpdateMovement();
    }
    else {
      OnGotToLastWayPoint();
    }

    if (frozen) {
      freezeTimer += Time.deltaTime;
      if (freezeTimer >= timeEnemyStaysFrozenInSeconds) {
        Defrost();
      }
    }
  }

  private void UpdateMovement() {
    Vector3 targetPosition = WayPointManager.Instance.Paths[pathIndex].WayPoints[wayPointIndex].position;

    // Move to waypoint
    transform.position = Vector3.MoveTowards(transform.position, targetPosition
        , moveSpeed * Time.deltaTime);
    // Look at next waypoint
    transform.localRotation = UtilityMethods.SmoothlyLook(transform, targetPosition);

    // If arrived at waypoint, increase wayPointIndex
    if (Vector3.Distance(transform.position, targetPosition) < .1f) {
      wayPointIndex++;
    }
  }

  void OnGotToLastWayPoint() {
    // This enemy has managed to escape
    GameManager.Instance.OnEnemyEscape();

    // Die
    Die();
  }

  public void TakeDamage(float amountOfDamage) {
    health -= amountOfDamage;

    if (health <= 0) {
      DropGold();
      Die();
    }
  }

  void DropGold() {
    GameManager.Instance.gold += goldDrop;
  }

  public void Freeze() {
    if (!frozen) {
      frozen = true;
      moveSpeed /= 2;
    }
  }

  void Defrost() {
    freezeTimer = 0f;
    frozen = false;
    moveSpeed *= 2;
  }

  void Die() {
    // Check if gameobject still exists for the rare occasion to attacks overlap and both cause death
    if (gameObject != null) {
      EnemyManager.Instance.UnRegister(this);
      gameObject.AddComponent<AutoScaler>().scaleSpeed = -2;
      enabled = false;
      Destroy(gameObject, 0.3f);
    }
  }
}
