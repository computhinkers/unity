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

public class Player : MonoBehaviour {

  public int health;
  public int armor;
  public GameUI gameUI;
  private GunEquipper gunEquipper;
  private Ammo ammo;
  public Game game;
  public AudioClip playerDead;

  // Use this for initialization
  void Start() {
    ammo = GetComponent<Ammo>();
    gunEquipper = GetComponent<GunEquipper>();
  }

  public void TakeDamage(int amount) {
    int healthDamage = amount;

    if (armor > 0) {
      int effectiveArmor = armor * 2;
      effectiveArmor -= healthDamage;

      // If there is still armor, don't need to process
      // health damage
      if (effectiveArmor > 0) {
        armor = effectiveArmor / 2;
        gameUI.SetArmorText(armor);
        return;
      }

      armor = 0;
      gameUI.SetArmorText(armor);
    }

    health -= healthDamage;
    gameUI.SetHealthText(health);

    if (health <= 0) {
      GetComponent<AudioSource>().PlayOneShot(playerDead);
      game.GameOver();
    }
  }

  // Update is called once per frame
  void LateUpdate() {
    transform.position = new Vector3(transform.position.x, 2.68f, transform.position.z);
  }

  // 1
  private void pickupHealth() {
    health += 50;
    if (health > 200) {
      health = 200;
    }
    gameUI.SetPickUpText("Health picked up + 50 Health");
    gameUI.SetHealthText(health);
  }

  private void pickupArmor() {
    armor += 15;
    gameUI.SetPickUpText("Armor picked up + 15 armor");
    gameUI.SetArmorText(armor);
  }

  // 2
  private void pickupAssaultRifleAmmo() {
    ammo.AddAmmo(Constants.AssaultRifle, 50);
    gameUI.SetPickUpText("Assault rifle ammo picked up + 50 ammo");
    if (gunEquipper.GetActiveWeapon().tag == Constants.AssaultRifle) {
      gameUI.SetAmmoText(ammo.GetAmmo(Constants.AssaultRifle));
    }
  }

  private void pickupPisolAmmo() {
    ammo.AddAmmo(Constants.Pistol, 20);
    gameUI.SetPickUpText("Pistol ammo picked up + 20 ammo");
    if (gunEquipper.GetActiveWeapon().tag == Constants.Pistol) {
      gameUI.SetAmmoText(ammo.GetAmmo(Constants.Pistol));
    }
  }

  private void pickupShotgunAmmo() {
    ammo.AddAmmo(Constants.Shotgun, 10);
    gameUI.SetPickUpText("Shotgun ammo picked up + 10 ammo");
    if (gunEquipper.GetActiveWeapon().tag == Constants.Shotgun) {
      gameUI.SetAmmoText(ammo.GetAmmo(Constants.Shotgun));
    }
  }

  public void PickUpItem(int pickupType) {
    switch (pickupType) {
      case Constants.PickUpArmor:
        pickupArmor();
        break;
      case Constants.PickUpHealth:
        pickupHealth();
        break;
      case Constants.PickUpAssaultRifleAmmo:
        pickupAssaultRifleAmmo();
        break;
      case Constants.PickUpPistolAmmo:
        pickupPisolAmmo();
        break;
      case Constants.PickUpShotgunAmmo:
        pickupShotgunAmmo();
        break;
      default:
        Debug.LogError("Bad pickup type passed" + pickupType);
        break;
    }
  }


}
