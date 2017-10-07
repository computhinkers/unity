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
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour {

  public Transform target;
  private NavMeshAgent agent;
  public float navigationUpdate;
  public UnityEvent OnDestroy;

  public Rigidbody head;
  public bool isAlive = true;
  private float navigationTime = 0;
  private DeathParticles deathParticles;

  // Use this for initialization
  void Start () {
    agent = GetComponent<NavMeshAgent>();
  }

  public DeathParticles GetDeathParticles() {
    if (deathParticles == null) {
      deathParticles = GetComponentInChildren<DeathParticles>();
    }
    return deathParticles;
  }

  public void Die() {
    isAlive = false;
    head.GetComponent<Animator>().enabled = false;
    head.isKinematic = false;
    head.useGravity = true;
    head.GetComponent<SphereCollider>().enabled = true;
    head.gameObject.transform.parent = null;
    head.velocity = new Vector3(0, 26.0f, 3.0f);
    OnDestroy.Invoke();
    OnDestroy.RemoveAllListeners();
    SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
    head.GetComponent<SelfDestruct>().Initiate();
    if (deathParticles) {
      deathParticles.transform.parent = null;
      deathParticles.Activate();
    }
    Destroy(gameObject);
  }

  void OnTriggerEnter(Collider other) {
    if (isAlive) {
      Die();
      SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
    }
  }

  // Update is called once per frame
  void Update() {
    if (isAlive) {
      if (target != null) {
        navigationTime += Time.deltaTime;
        if (navigationTime > navigationUpdate) {
          agent.destination = target.position;
          navigationTime = 0;
        }
      }
    }
  }
}
