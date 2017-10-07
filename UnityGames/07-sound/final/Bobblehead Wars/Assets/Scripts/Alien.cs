using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour {

  public Transform target;
  private NavMeshAgent agent;
  public float navigationUpdate;
  private float navigationTime = 0;

  // Use this for initialization
  void Start () {
    agent = GetComponent<NavMeshAgent>();
  }

  void OnTriggerEnter(Collider other) {
    Destroy(gameObject);
    SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
  }

  // Update is called once per frame
  void Update () {
    if (target != null) {
      navigationTime += Time.deltaTime;
      if (navigationTime > navigationUpdate) {
        agent.destination = target.position;
        navigationTime = 0;
      }
    }
  }
}
