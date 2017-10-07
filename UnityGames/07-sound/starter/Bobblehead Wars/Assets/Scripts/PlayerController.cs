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

public class PlayerController : MonoBehaviour {

  public float moveSpeed = 50.0f;
  private CharacterController characterController;
  public Rigidbody head;
  public LayerMask layerMask;
  public Animator bodyAnimator;
  private Vector3 currentLookTarget = Vector3.zero;

  // Use this for initialization
  void Start () {
    characterController = GetComponent<CharacterController>();
  }
	
	// Update is called once per frame
	void Update () {
    Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    characterController.SimpleMove(moveDirection * moveSpeed);
  }

  void FixedUpdate() {
    Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    if (moveDirection == Vector3.zero) {
      bodyAnimator.SetBool("IsMoving", false);
    } else {
      head.AddForce(transform.right * 150, ForceMode.Acceleration);
      bodyAnimator.SetBool("IsMoving", true);
    }
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
    if (Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore)) {
      if (hit.point != currentLookTarget) {
        currentLookTarget = hit.point;
        Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        // 2
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        // 3
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
      }
    }
  }

}
