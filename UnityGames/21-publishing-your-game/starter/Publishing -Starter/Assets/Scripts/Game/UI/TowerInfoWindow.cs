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
using UnityEngine.UI;

public class TowerInfoWindow : MonoBehaviour {
  public Tower tower;

  public Text txtInfo;
  public Text txtUpgradeCost;

  private int upgradePrice;
  private GameObject btnUpgrade;

  void Awake() {
    btnUpgrade = txtUpgradeCost.transform.parent.gameObject;
  }

  void OnEnable() {
    UpdateInfo();
  }

  /// <summary>
  /// Updates the info text
  /// </summary>
  private void UpdateInfo() {
    // Calculate new price for upgrade
    upgradePrice = Mathf.CeilToInt(TowerManager.Instance.GetTowerPrice(tower.type) * 1.5f * tower.towerLevel);

    txtInfo.text = tower.type + " Tower Lv " + tower.towerLevel;

    if (tower.towerLevel < 3) {
      // Show button
      btnUpgrade.SetActive(true);
      txtUpgradeCost.text = "Upgrade\n" + upgradePrice + " Gold";
    }
    else {
      // Hide button
      btnUpgrade.SetActive(false);
    }

  }

  public void UpgradeTower() {
    if (GameManager.Instance.gold >= upgradePrice) {
      GameManager.Instance.gold -= upgradePrice;
      tower.LevelUp();

      gameObject.SetActive(false);
    }
  }
}
