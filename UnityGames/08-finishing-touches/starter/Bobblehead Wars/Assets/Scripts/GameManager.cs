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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  public GameObject player;
  public GameObject[] spawnPoints;
  public GameObject alien;
  public int maxAliensOnScreen;
  public int totalAliens;
  public float minSpawnTime;
  public float maxSpawnTime;
  public int aliensPerSpawn;
  public GameObject upgradePrefab;
  public Gun gun;
  public float upgradeMaxTimeSpawn = 7.5f;

  private bool spawnedUpgrade = false;
  private float actualUpgradeTime = 0;
  private float currentUpgradeTime = 0;

  private int aliensOnScreen = 0;
  private float generatedSpawnTime = 0;
  private float currentSpawnTime = 0;

  // Use this for initialization
  void Start () {
    actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
    actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
  }
	
	// Update is called once per frame
	void Update () {
    currentUpgradeTime += Time.deltaTime;
    currentSpawnTime += Time.deltaTime;
    if (currentUpgradeTime > actualUpgradeTime) {
      if (!spawnedUpgrade) { // 1
        // 2
        int randomNumber = Random.Range(0, spawnPoints.Length - 1);
        GameObject spawnLocation = spawnPoints[randomNumber];
        // 3
        GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
        Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
        upgradeScript.gun = gun;
        upgrade.transform.position = spawnLocation.transform.position;
        // 4
        spawnedUpgrade = true;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
      }
    }
    if (currentSpawnTime > generatedSpawnTime) {
      currentSpawnTime = 0;
      generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
      if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens) {
        List<int> previousSpawnLocations = new List<int>();
        if (aliensPerSpawn > spawnPoints.Length) {
          aliensPerSpawn = spawnPoints.Length - 1;
        }
        aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;
        for (int i = 0; i < aliensPerSpawn; i++) {
          if (aliensOnScreen < maxAliensOnScreen) {
            aliensOnScreen += 1;
            // 1
            int spawnPoint = -1;
            // 2
            while (spawnPoint == -1) {
              // 3
              int randomNumber = Random.Range(0, spawnPoints.Length - 1);
              // 4
              if (!previousSpawnLocations.Contains(randomNumber)) {
                previousSpawnLocations.Add(randomNumber);
                spawnPoint = randomNumber;
              }
            }
            GameObject spawnLocation = spawnPoints[spawnPoint];
            GameObject newAlien = Instantiate(alien) as GameObject;
            newAlien.transform.position = spawnLocation.transform.position;
            Alien alienScript = newAlien.GetComponent<Alien>();
            alienScript.target = player.transform;
            Vector3 targetRotation = new Vector3(player.transform.position.x, newAlien.transform.position.y, player.transform.position.z);
            newAlien.transform.LookAt(targetRotation);
          }
        }
      }
    }
  }
}
