using UnityEngine;
using System.Collections;

public class PlayerScatter : MonoBehaviour {
	
	float lifetime = 0.75f;
	public float speed = 35.0f;
	public Vector3 dir;
	
	float lifetimeMod = 0.0f;

	// Use this for initialization
	void Start () {
		
		float calc = PlayerPrefs.GetInt("W1Range");
		lifetimeMod = lifetime  + calc / 4.0f;

		Destroy(gameObject, lifetimeMod); // destroy over time
		
		GameObject p1 = GameObject.FindGameObjectWithTag("Player");
		if (p1) {
			SimpleMovement sM = (SimpleMovement) p1.GetComponent(typeof(SimpleMovement));
	
			if (sM.lastDir == 0) {
				dir = transform.right;
			}	else if (sM.lastDir == 1) {
				//dir = -transform.right;
				dir = Vector3.Reflect(transform.right, Vector3.right);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position +=  dir * speed * Time.deltaTime;
	}
	void OnCollisionEnter(Collision collision) {
		
		
		if (collision.gameObject.tag == "enemySteamBot") {
			Destroy(gameObject);
			//Destroy(gameObject);
			//Destroy(collision.gameObject);

		}
	}
}
