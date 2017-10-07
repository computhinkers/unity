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
using System.Collections.Generic;

public enum TowerType {
  Stone, Fire, Ice
}

public class Tower : MonoBehaviour {
  public float attackPower = 3f;
  public float timeBetweenAttacksInSeconds = 1f;
  public float aggroRadius = 15f;
  public int towerLevel = 1;

  public TowerType type;

  //References
  public AudioClip shootSound;
  public Transform towerPieceToAim;

  public Enemy targetEnemy = null;

  private float attackCounter;

  public virtual void Update() {
    attackCounter -= Time.deltaTime;

    if (targetEnemy == null) { // No target
                               // Look at the camera
      if (towerPieceToAim) {
        SmoothlyLookAtTarget(towerPieceToAim.transform.position - new Vector3(0, 0, 1));
      }

      // Attemp to find new target
      if (GetNearestEnemyInRange() != null && Vector3.Distance(transform.position, GetNearestEnemyInRange().transform.position) <= aggroRadius) {
        targetEnemy = GetNearestEnemyInRange();
      }
    }
    else { // Got a target
           // Look at target
      if (towerPieceToAim) {
        SmoothlyLookAtTarget(targetEnemy.transform.position);
      }

      if (attackCounter <= 0f) {
        // Attack
        AttackEnemy();
        // Reset attack counter
        attackCounter = timeBetweenAttacksInSeconds;
      }

      // Target moved out of aggro range
      if (Vector3.Distance(transform.position, targetEnemy.transform.position) > aggroRadius) {
        targetEnemy = null;
      }
    }
  }

  private void SmoothlyLookAtTarget(Vector3 target) {
    towerPieceToAim.localRotation = UtilityMethods.SmoothlyLook(towerPieceToAim, target);
  }

  // Each tower implements its own way of attacking
  protected virtual void AttackEnemy() {
    GetComponent<AudioSource>().PlayOneShot(shootSound, .15f);
  }

  /// <summary>
  /// Get all enemies withing aggro range of this tower
  /// </summary>
  /// <returns></returns>
  public List<Enemy> GetEnemiesInAggroRange() {
    List<Enemy> enemiesInRange = new List<Enemy>();

    foreach (Enemy enemy in EnemyManager.Instance.Enemies) {
      if (Vector3.Distance(transform.position, enemy.transform.position) <= aggroRadius) {
        enemiesInRange.Add(enemy);
      }
    }

    return enemiesInRange;
  }

  public Enemy GetNearestEnemyInRange() {
    Enemy nearestEnemy = null;
    float smallestDistance = float.PositiveInfinity;

    foreach (Enemy enemy in GetEnemiesInAggroRange()) {
      if (Vector3.Distance(transform.position, enemy.transform.position) < smallestDistance) {
        smallestDistance = Vector3.Distance(transform.position, enemy.transform.position);
        nearestEnemy = enemy;
      }
    }

    return nearestEnemy;
  }

  public void LevelUp() {
    towerLevel++;

    //Calculate new stats for this tower
    attackPower *= 2;
    timeBetweenAttacksInSeconds *= 0.7f;
    aggroRadius *= 1.20f;
  }

  public void ShowTowerInfo() {
    UIManager.Instance.ShowTowerInfoWindow(this);
  }
}
