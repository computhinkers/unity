using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

  public GameObject bulletPrefab;
  public Transform launchPosition;

  // Use this for initialization
  void Start () {
		
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
  }

  void fireBullet() {
    // 1
    GameObject bullet = Instantiate(bulletPrefab) as GameObject;
    // 2
    bullet.transform.position = launchPosition.position;
    // 3
    bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
  }
}
