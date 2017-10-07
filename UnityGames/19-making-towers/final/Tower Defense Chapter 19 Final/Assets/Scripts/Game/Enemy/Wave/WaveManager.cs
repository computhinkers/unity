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

public class WaveManager : MonoBehaviour {
  public static WaveManager Instance;

  public List<EnemyWave> enemyWaves = new List<EnemyWave>();

  private float elapsedTime = 0f; // Total elapsed time spent on this level
  private EnemyWave activeWave;
  private float spawnCounter = 0f;
  private List<EnemyWave> activatedWaves = new List<EnemyWave>();

  void Awake() {
    Instance = this;
  }

  void Update() {
    elapsedTime += Time.deltaTime;

    SearchForWave();
    UpdateActiveWave();
  }

  private void SearchForWave() {
    foreach (EnemyWave enemyWave in enemyWaves) {
      // Only get the wave if it wasn't activated already and the time is past or equal to its start time
      if (!activatedWaves.Contains(enemyWave) && enemyWave.startSpawnTimeInSeconds <= elapsedTime) {
        // Activate next wave
        activeWave = enemyWave;
        activatedWaves.Add(enemyWave);
        spawnCounter = 0f;
        GameManager.Instance.waveNumber++;
        UIManager.Instance.ShowCenterWindow("Wave " + GameManager.Instance.waveNumber);
        break;
      }
    }
  }

  private void UpdateActiveWave() {
    if (activeWave != null) {
      spawnCounter += Time.deltaTime;

      // Need to spawn a new enemy
      if (spawnCounter >= activeWave.timeBetweenSpawnsInSeconds) {
        spawnCounter = 0f;

        // Check if there's still enemies in current wave  
        if (activeWave.listOfEnemies.Count != 0) {
          // Spawn the first enemy in list at its spawn position with default rotation
          GameObject enemy = (GameObject)Instantiate(activeWave.listOfEnemies[0], WayPointManager.Instance.GetSpawnPosition(activeWave.pathIndex), Quaternion.identity);
          enemy.GetComponent<Enemy>().pathIndex = activeWave.pathIndex;
          activeWave.listOfEnemies.RemoveAt(0);
        }
        else {
          // No more enemies in current wave
          activeWave = null;

          if (activatedWaves.Count == enemyWaves.Count) {
            // All waves are over
            GameManager.Instance.enemySpawningOver = true;
          }
        }
      }
    }
  }

  /// <summary>
  /// Stops spawning of enemies and resets the wave manager variables
  /// </summary>
  public void StopSpawning() {
    elapsedTime = 0;
    spawnCounter = 0;
    activeWave = null;
    activatedWaves.Clear();

    enabled = false;
  }
}
