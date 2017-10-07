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
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  //1
  public static GameManager Instance;
  //2
  public int gold;
  //3
  public int waveNumber;
  //4
  public int escapedEnemies;
  //5
  public int maxAllowedEscapedEnemies = 5;
  //6
  public bool enemySpawningOver;
  //7
  public AudioClip gameWinSound;
  public AudioClip gameLoseSound;
  //8
  public bool gameOver;

  //1
  void Awake() {
    Instance = this;
  }

  void Update() {
    //2
    if (!gameOver && enemySpawningOver) {
      // Check if no enemies left, if so win game
      //3
      if (EnemyManager.Instance.Enemies.Count == 0) {
        OnGameWin();
      }
    }

    // When ESC is pressed, quit to the title screen
    //4
    if (Input.GetKeyDown(KeyCode.Escape)) {
      QuitToTitleScreen();
    }
  }

  //5
  private void OnGameWin() {
    AudioSource.PlayClipAtPoint(gameWinSound, Camera.main.transform.position);
    gameOver = true;
    UIManager.Instance.ShowWinScreen();
  }
  //6
  public void QuitToTitleScreen() {
    SceneManager.LoadScene("TitleScreen");
  }

  //1
  public void OnEnemyEscape() {
    escapedEnemies++;
    UIManager.Instance.ShowDamage();

    if (escapedEnemies == maxAllowedEscapedEnemies) {
      // Too many enemies escaped, you lose the game
      OnGameLose();
    }
  }

  //2
  private void OnGameLose() {
    gameOver = true;

    AudioSource.PlayClipAtPoint(gameLoseSound, Camera.main.transform.position);
    EnemyManager.Instance.DestroyAllEnemies();
    WaveManager.Instance.StopSpawning();

    UIManager.Instance.ShowLoseScreen();
  }

  //3
  public void RetryLevel() {
    SceneManager.LoadScene("Game");
  }
}
