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
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  //1
  public static UIManager Instance;
  //2
  public GameObject addTowerWindow;
  public GameObject towerInfoWindow;
  public GameObject winGameWindow;
  public GameObject loseGameWindow;
  public GameObject blackBackground;
  public GameObject centerWindow;
  public GameObject damageCanvas;
  public Transform enemyHealthBars;
  public GameObject enemyHealthBarPrefab;
  //3
  public Text txtGold;
  public Text txtWave;
  public Text txtEscapedEnemies;

  public static float vrUiScaleDivider = 12;

  //1
  void Awake() {
    Instance = this;
  }
  //2
  private void UpdateTopBar() {
    txtGold.text = GameManager.Instance.gold.ToString();
    txtWave.text = "Wave " + GameManager.Instance.waveNumber + " / " + WaveManager.Instance.enemyWaves.Count;
    txtEscapedEnemies.text = "Escaped Enemies " + GameManager.Instance.escapedEnemies + " / " +
                             GameManager.Instance.maxAllowedEscapedEnemies;
  }
  //3
  //4
  public void ShowAddTowerWindow(GameObject towerSlot) {
    if (GameManager.Instance.gameOver) {
      return;
    }

    addTowerWindow.SetActive(true);
    addTowerWindow.GetComponent<AddTowerWindow>().towerSlotToAddTowerTo = towerSlot;

    UtilityMethods.MoveUiElementToWorldPosition(addTowerWindow.GetComponent<RectTransform>(), towerSlot.transform.position);
  }

  // Update is called once per frame
  void Update () {
    UpdateTopBar();
  }

  public void ShowTowerInfoWindow(Tower tower) {
    if (GameManager.Instance.gameOver) {
      return;
    }
    towerInfoWindow.GetComponent<TowerInfoWindow>().tower = tower;
    towerInfoWindow.SetActive(true);

    UtilityMethods.MoveUiElementToWorldPosition(towerInfoWindow.GetComponent<RectTransform>(), tower.transform.position);
  }

  public void ShowWinScreen() {
    winGameWindow.SetActive(true);
  }

  public void ShowLoseScreen() {
    loseGameWindow.SetActive(true);
  }

  //1
  public void CreateHealthBarForEnemy(Enemy enemy) {
    //2
    GameObject healthBar = Instantiate(enemyHealthBarPrefab);
    //3
    healthBar.transform.SetParent(enemyHealthBars, false);
    //4
    healthBar.GetComponent<EnemyHealthBar>().enemy = enemy;
  }

  //1
  public void ShowCenterWindow(string text) {
    centerWindow.transform.Find("TxtWave").GetComponent<Text>().text = text;
    StartCoroutine(EnableAndDisableCenterWindow());
  }

  //2
  private IEnumerator EnableAndDisableCenterWindow() {
    for (int i = 0; i < 3; i++) {
      yield return new WaitForSeconds(.4f);
      centerWindow.SetActive(true);

      yield return new WaitForSeconds(.4f);
      centerWindow.SetActive(false);
    }
  }

  //1
  public void ShowDamage() {
    StartCoroutine(DoDamageAnimation());
  }
  //2
  private IEnumerator DoDamageAnimation() {
    for (int i = 0; i < 3; i++) {
      yield return new WaitForSeconds(.1f);
      damageCanvas.SetActive(true);

      yield return new WaitForSeconds(.1f);
      damageCanvas.SetActive(false);
    }
  }


}
