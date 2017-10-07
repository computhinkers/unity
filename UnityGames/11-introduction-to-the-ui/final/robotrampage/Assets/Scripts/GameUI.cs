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

public class GameUI : MonoBehaviour {

	[SerializeField]
	Sprite redReticle;
	[SerializeField]
	Sprite yellowReticle;
	[SerializeField]
	Sprite blueReticle;
	[SerializeField]
	Image reticle;
	[SerializeField]
	private Text ammoText;
	[SerializeField]
	private Text healthText;
	[SerializeField]
	private Text armorText;
	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text pickupText;
	[SerializeField]
	private Text waveText;
	[SerializeField]
	private Text enemyText;
	[SerializeField]
	private Text waveClearText;
	[SerializeField]
	private Text newWaveText;
	[SerializeField]
	Player player;

	public void UpdateReticle() {
	  switch (GunEquipper.activeWeaponType) {
	    case Constants.Pistol:
	      reticle.sprite = redReticle;
	      break;
	    case Constants.Shotgun:
	      reticle.sprite = yellowReticle;
	      break;
	    case Constants.AssaultRifle:
	      reticle.sprite = blueReticle;
	      break;
	    default:
	      return;
	  }
	}

	// 1
	void Start() {
	  SetArmorText(player.armor);
	  SetHealthText(player.health);
	}

	// 2
	public void SetArmorText(int armor) {
	  armorText.text = "Armor: " + armor;
	}

	public void SetHealthText(int health) {
	  healthText.text = "Health: " + health;
	}

	public void SetAmmoText(int ammo) {
	  ammoText.text = "Ammo: " + ammo;
	}

	public void SetScoreText(int score) {
	  scoreText.text = "" + score;
	}

	public void SetWaveText(int time) {
	  waveText.text = "Next Wave: " + time;
	}

	public void SetEnemyText(int enemies) {
	  enemyText.text = "Enemies: " + enemies;
	}

	// 1
	public void ShowWaveClearBonus() {
	  waveClearText.GetComponent<Text>().enabled = true;
	  StartCoroutine("hideWaveClearBonus");
	}

	// 2
	IEnumerator hideWaveClearBonus() {
	  yield return new WaitForSeconds(4);
	  waveClearText.GetComponent<Text>().enabled = false;
	}

	// 3
	public void SetPickUpText(string text) {
	  pickupText.GetComponent<Text>().enabled = true;
	  pickupText.text = text;
	  // Restart the Coroutine so it doesn't end early
	  StopCoroutine("hidePickupText");
	  StartCoroutine("hidePickupText");
	}

	// 4
	IEnumerator hidePickupText() {
	  yield return new WaitForSeconds(4);
	  pickupText.GetComponent<Text>().enabled = false;
	}

	// 5
	public void ShowNewWaveText() {
	  StartCoroutine("hideNewWaveText");
	  newWaveText.GetComponent<Text>().enabled = true;
	}

	// 6
	IEnumerator hideNewWaveText() {
	  yield return new WaitForSeconds(4);
	  newWaveText.GetComponent<Text>().enabled = false;
	}

}
