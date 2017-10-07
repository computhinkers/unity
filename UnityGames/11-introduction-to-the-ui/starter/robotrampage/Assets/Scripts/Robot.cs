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

public class Robot : MonoBehaviour {

	[SerializeField]
	private string robotType;
	[SerializeField]
	GameObject missileprefab;
	public Animator robot;
	public int health;
	public int range;
	public float fireRate;

	[SerializeField]
	private AudioClip deathSound;
	[SerializeField]
	private AudioClip fireSound;
	[SerializeField]
	private AudioClip weakHitSound;

	public Transform missileFireSpot;
	UnityEngine.AI.NavMeshAgent agent;

	private Transform player;
	private float timeLastFired;

	private bool isDead;

	void Start() {
	  // 1
	  isDead = false;
	  agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	  player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update() {
	  // 2
	  if (isDead) {
	    return;
	  }

	  // 3
	  transform.LookAt(player);
    transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);

    // 4
    agent.SetDestination(player.position);

	  // 5
	  if (Vector3.Distance(transform.position, player.position) < range
	      && Time.time - timeLastFired > fireRate) {
	    // 6
	    timeLastFired = Time.time;
	    fire();
	  }
	}

	private void fire() {
	  GameObject missile = Instantiate(missileprefab);
	  missile.transform.position = missileFireSpot.transform.position;
	  missile.transform.rotation = missileFireSpot.transform.rotation;
	  robot.Play("Fire");
		GetComponent<AudioSource>().PlayOneShot(fireSound);
	}

	public void TakeDamage(int amount) {
	  if (isDead) {
	    return;
	  }

	  health -= amount;

	  if (health <= 0) {
	    isDead = true;
	    robot.Play("Die");
	    StartCoroutine("DestroyRobot");
			GetComponent<AudioSource>().PlayOneShot(deathSound);
	  } else {
			GetComponent<AudioSource>().PlayOneShot(weakHitSound);
	  }
	}

	// 2
	IEnumerator DestroyRobot() {
	  yield return new WaitForSeconds(1.5f);
	  Destroy(gameObject);
	}

}
