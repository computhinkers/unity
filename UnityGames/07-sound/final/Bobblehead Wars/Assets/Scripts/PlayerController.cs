﻿using System.Collections;
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
