using UnityEngine;
using System.Collections;

public class FacePlayer : MonoBehaviour {
	
	float rotationSpeed = 9.0f;
	Vector3 startingRotation;
	Quaternion endingRotation;
	Transform player;
	public bool switchSides;
	
	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			Vector3 heading = transform.position - player.position;
			
			if (switchSides)
				heading = -heading;
			//Debug.Log ("heading: " + heading);
			if (heading.x  <= 0) {
					endingRotation = Quaternion.Euler(new Vector3(0, 0, 0)); // rotate right
					transform.rotation = Quaternion.Lerp(transform.rotation, endingRotation, Time.deltaTime * rotationSpeed);	
			} else if (heading.x > 0) {
				endingRotation = Quaternion.Euler(new Vector3(0, -180, 0)); // rotate left
				transform.rotation = Quaternion.Lerp(transform.rotation, endingRotation, Time.deltaTime * rotationSpeed);		
			}
			
		}	
	}
}
