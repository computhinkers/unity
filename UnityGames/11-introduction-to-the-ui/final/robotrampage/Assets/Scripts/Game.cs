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
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement; 

public class Game : MonoBehaviour {

	private static Game singleton;
	[SerializeField]	
	RobotSpawn[] spawns;
	public int enemiesLeft;
	public GameObject gameOverPanel;
	public GameUI gameUI;
	public GameObject player;
	public int score;
	public int waveCountdown;
	public bool isGameOver;

	// 1
	void Start() {
	  singleton = this;
		StartCoroutine("increaseScoreEachSecond");
		isGameOver = false;
		Time.timeScale = 1;
		waveCountdown = 30;
		enemiesLeft = 0;
		StartCoroutine("updateWaveTimer");
	  SpawnRobots();
	}

	// 2
	private void SpawnRobots() {
	  foreach (RobotSpawn spawn in spawns) {
	    spawn.SpawnRobot();
	    enemiesLeft++;
	  }
		gameUI.SetEnemyText(enemiesLeft);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator updateWaveTimer() {
	  while (!isGameOver) {
	    yield return new WaitForSeconds(1f);
	    waveCountdown--;
	    gameUI.SetWaveText(waveCountdown);

	    // Spawn next wave and restart count down
	    if (waveCountdown == 0) {
	      SpawnRobots();
	      waveCountdown = 30;
	      gameUI.ShowNewWaveText();
	    }
	  }
	}

	IEnumerator increaseScoreEachSecond() {
	  while (!isGameOver) {
	    yield return new WaitForSeconds(1);
	    score += 1;
	    gameUI.SetScoreText(score);
	  }
	}

	public void AddRobotKillToScore() {
	  score += 10;
	  gameUI.SetScoreText(score);
	}

	public static void RemoveEnemy() {
	  singleton.enemiesLeft--;
	  singleton.gameUI.SetEnemyText(singleton.enemiesLeft);

	  // Give player bonus for clearing the wave before timer is done
	  if (singleton.enemiesLeft == 0) {
	    singleton.score += 50;
	    singleton.gameUI.ShowWaveClearBonus();
	  }
	}

	// 1
	public void OnGUI() {
	  if (isGameOver && Cursor.visible == false) {
	    Cursor.visible = true;
	    Cursor.lockState = CursorLockMode.None;
	  }
	}

	// 2
	public void GameOver() {
	  isGameOver = true;
	  Time.timeScale = 0;
	  player.GetComponent<FirstPersonController>().enabled = false;
	  player.GetComponent<CharacterController>().enabled = false;
	  gameOverPanel.SetActive(true);
	}

	// 3
	public void RestartGame() {
	  SceneManager.LoadScene(Constants.SceneBattle);
	  gameOverPanel.SetActive(true);
	}

	// 4
	public void Exit() {
	  Application.Quit();
	}

	// 5
	public void MainMenu() {
	  SceneManager.LoadScene(Constants.SceneMenu);
	}

}
