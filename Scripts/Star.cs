using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public float speed = 9.0F;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// update speed based on placer movement
		
		// how to call another script
		//GameObject script = GameObject.Find("Player");
		//PlayerMovement other = (PlayerMovement) script.GetComponent(typeof(PlayerMovement));
		
		/*
		if (other.isMoving) {
			speed = 10.0f;
		} else {
			speed = 1.0f;
		}*/
		
		transform.position -=  transform.right * speed * Time.deltaTime;
		
		if (transform.position.x <= -600.0f) {
			transform.position = new Vector3(400.0f, transform.position.y, transform.position.z);
		}

	}
	
	
}
