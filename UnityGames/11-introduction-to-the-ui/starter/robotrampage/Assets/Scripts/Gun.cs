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
