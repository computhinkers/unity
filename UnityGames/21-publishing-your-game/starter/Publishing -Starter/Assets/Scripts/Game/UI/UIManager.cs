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
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  public static UIManager Instance;

  //References
  public Transform enemyHealthBars;

  public GameObject addTowerWindow;
  public GameObject towerInfoWindow;
  public GameObject winGameWindow;
  public GameObject loseGameWindow;
  public GameObject centerWindow;

  public GameObject damageCanvas;

  // Black transparent background
  public GameObject blackBackground;

  // TopBar
  public Text txtGold;
  public Text txtWave;
  public Text txtEscapedEnemies;

  //Prefabs
  public GameObject enemyHealthBarPrefab;

  void Awake() {
    Instance = this;
  }

  void Update() {
    UpdateTopBar();
  }

  private void UpdateTopBar() {
    txtGold.text = GameManager.Instance.gold.ToString();
    txtWave.text = "Wave " + GameManager.Instance.waveNumber + " / " + WaveManager.Instance.enemyWaves.Count;
    txtEscapedEnemies.text = "Escaped Enemies " + GameManager.Instance.escapedEnemies + " / " +
                             GameManager.Instance.maxAllowedEscapedEnemies;
  }

  public void ShowAddTowerWindow(GameObject towerSlot) {
    addTowerWindow.SetActive(true);
    addTowerWindow.GetComponent<AddTowerWindow>().towerSlotToAddTowerTo = towerSlot;

    UtilityMethods.MoveUiElementToWorldPosition(addTowerWindow.GetComponent<RectTransform>(), towerSlot.transform.position);
  }

  public void ShowDamage() {
    StartCoroutine(DoDamageAnimation());
  }

  private IEnumerator DoDamageAnimation() {
    for (int i = 0; i < 3; i++) {
      yield return new WaitForSeconds(.1f);
      damageCanvas.SetActive(true);

      yield return new WaitForSeconds(.1f);
      damageCanvas.SetActive(false);
    }
  }

  public void ShowCenterWindow(string text) {
    centerWindow.transform.Find("TxtWave").GetComponent<Text>().text = text;
    StartCoroutine(EnableAndDisableCenterWindow());
  }

  private IEnumerator EnableAndDisableCenterWindow() {
    for (int i = 0; i < 3; i++) {
      yield return new WaitForSeconds(.4f);
      centerWindow.SetActive(true);

      yield return new WaitForSeconds(.4f);
      centerWindow.SetActive(false);
    }
  }

  public void ShowTowerInfoWindow(Tower tower) {
    towerInfoWindow.GetComponent<TowerInfoWindow>().tower = tower;
    towerInfoWindow.SetActive(true);

    UtilityMethods.MoveUiElementToWorldPosition(towerInfoWindow.GetComponent<RectTransform>(), tower.transform.position);
  }

  public void CreateHealthBarForEnemy(Enemy enemy) {
    GameObject healthBar = Instantiate(enemyHealthBarPrefab);
    healthBar.transform.SetParent(enemyHealthBars, false);
    healthBar.GetComponent<EnemyHealthBar>().enemy = enemy;
  }

  public void ShowWinScreen() {
    blackBackground.SetActive(true);
    winGameWindow.SetActive(true);
  }

  public void ShowLoseScreen() {
    blackBackground.SetActive(true);
    loseGameWindow.SetActive(true);
  }


}
