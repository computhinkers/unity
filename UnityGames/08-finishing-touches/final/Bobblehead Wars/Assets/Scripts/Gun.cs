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

public class Gun : MonoBehaviour {

  public GameObject bulletPrefab;
  public Transform launchPosition;
  public bool isUpgraded;
  public float upgradeTime = 10.0f;
  private float currentTime;
  private AudioSource audioSource;

  // Use this for initialization
  void Start () {
    audioSource = GetComponent<AudioSource>();
  }
	
	// Update is called once per frame
	void Update () {
    if (Input.GetMouseButtonDown(0)) {
      if (!IsInvoking("fireBullet")) {
        InvokeRepeating("fireBullet", 0f, 0.1f);
      }
    }
    if (Input.GetMouseButtonUp(0)) {
      CancelInvoke("fireBullet");
    }
    currentTime += Time.deltaTime;
    if (currentTime > upgradeTime && isUpgraded == true) {
      isUpgraded = false;
    }
  }

  void fireBullet() {
    Rigidbody bullet = createBullet();
    bullet.velocity = transform.parent.forward * 100;
    
    if (isUpgraded) {
      Rigidbody bullet2 = createBullet();
      bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;

      Rigidbody bullet3 = createBullet();
      bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
    }
    if (isUpgraded) {
      audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
    } else {
      audioSource.PlayOneShot(SoundManager.Instance.gunFire);
    }
  }

  private Rigidbody createBullet() {
    GameObject bullet = Instantiate(bulletPrefab) as GameObject;
    bullet.transform.position = launchPosition.position;
    return bullet.GetComponent<Rigidbody>();
  }

  public void UpgradeGun() {
    isUpgraded = true;
    currentTime = 0;
  }

}
