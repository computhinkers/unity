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

public class Gun : MonoBehaviour {

	public Ammo ammo;
	public AudioClip liveFire;
	public AudioClip dryFire;
	public float fireRate;
	protected float lastFireTime;
	public float zoomFactor;
	public int range;
	public int damage;

	private float zoomFOV;
	private float zoomSpeed = 6;

	// Use this for initialization
	void Start() {
		zoomFOV = Constants.CameraDefaultZoom / zoomFactor;
  	lastFireTime = Time.time - 10;
	}

	protected virtual void Update() {
	  // Right Click (Zoom)
	  if (Input.GetMouseButton(1)) {
	    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
	  } else {
	    Camera.main.fieldOfView = Constants.CameraDefaultZoom;
	  }
	}

	protected void Fire() {
	  if (ammo.HasAmmo(tag)) {
	    GetComponent<AudioSource>().PlayOneShot(liveFire);
	    ammo.ConsumeAmmo(tag);
	  } else {
	    GetComponent<AudioSource>().PlayOneShot(dryFire);
	  }
	  GetComponentInChildren<Animator>().Play("Fire");
		Ray ray = Camera.main.ViewportPointToRay(new
            Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, range)) {
  		processHit(hit.collider.gameObject);
		}
	}

	private void processHit(GameObject hitObject) {
	  if (hitObject.GetComponent<Player>() != null) {
	    hitObject.GetComponent<Player>().TakeDamage(damage);
	  }

	  if (hitObject.GetComponent<Robot>() != null) {
	    hitObject.GetComponent<Robot>().TakeDamage(damage);
	  }
	}

}
