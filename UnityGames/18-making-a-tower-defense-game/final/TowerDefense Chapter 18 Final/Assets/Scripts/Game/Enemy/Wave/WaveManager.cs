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
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
  //1
  public static WaveManager Instance;
  //2
  public List<EnemyWave> enemyWaves = new List<EnemyWave>();
  //3
  private float elapsedTime = 0f;
  //4
  private EnemyWave activeWave;
  //5
  private float spawnCounter = 0f;
  //6
  private List<EnemyWave> activatedWaves = new List<EnemyWave>();

  //1
  void Awake() {
    Instance = this;
  }
  //2
  void Update() {
    elapsedTime += Time.deltaTime;

    SearchForWave();
    UpdateActiveWave();
  }
  private void SearchForWave() {
    //3
    foreach (EnemyWave enemyWave in enemyWaves) {
      //4
      if (!activatedWaves.Contains(enemyWave) && enemyWave.startSpawnTimeInSeconds <= elapsedTime) {
        // Activate next wave
        //5
        activeWave = enemyWave;
        activatedWaves.Add(enemyWave);
        spawnCounter = 0f;
        GameManager.Instance.waveNumber++;
        //6
        break;
      }
    }
  }
  //7
  private void UpdateActiveWave() {
    //1
    if (activeWave != null) {
      spawnCounter += Time.deltaTime;

      //2
      if (spawnCounter >= activeWave.timeBetweenSpawnsInSeconds) {
        spawnCounter = 0f;

        //3
        if (activeWave.listOfEnemies.Count != 0) {
          //4
          GameObject enemy = (GameObject)Instantiate(activeWave.listOfEnemies[0], WayPointManager.Instance.GetSpawnPosition(activeWave.pathIndex), Quaternion.identity);
          //5
          enemy.GetComponent<Enemy>().pathIndex = activeWave.pathIndex;
          //6
          activeWave.listOfEnemies.RemoveAt(0);
        }
        else {
          //7
          activeWave = null;
          //8
          if (activatedWaves.Count == enemyWaves.Count) {
            GameManager.Instance.enemySpawningOver = true;
            // All waves are over
          }
        }
      }
    }
  }

  public void StopSpawning() {
    elapsedTime = 0;
    spawnCounter = 0;
    activeWave = null;
    activatedWaves.Clear();

    enabled = false;
  }
}
