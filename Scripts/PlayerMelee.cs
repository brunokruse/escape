using UnityEngine;
using System.Collections;

public class PlayerMelee : MonoBehaviour {
	
	float lifetime = 0.15f;
	public float speed = 90.0f;
	public Vector3 dir;
	float alpha;
	float lifetimeMod = 0.0f;

	// Use this for initialization
	void Start () {
		alpha = 1.0f;
		
		
		float calc = PlayerPrefs.GetInt("W1Range");
		lifetimeMod = lifetime + calc / 32.0f;
		
		Destroy(gameObject, lifetimeMod); // destroy over time
		
		//Destroy(gameObject, lifetime); // destroy over time
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
		
		//fadeOut(0.04f);
		//renderer.material.color.a
	}
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "enemySpawn") {
			Destroy(gameObject);
		}
		
		if (collision.gameObject.tag == "enemySteamBot") {
			Destroy(gameObject);
			//Destroy(gameObject);
			//Destroy(collision.gameObject);

		}
	}
	
	void fadeOut (float amt) {
		tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
		mySprite.color -= new Color(255,255,255, amt);
		
		/*
		LineRenderer line = gameObject.GetComponent<LineRenderer>();
		alpha -= 10;//amt; //Time.deltaTime * amt;
       	Color start = Color.white;
       	start.a = alpha;
       	Color end = Color.black;
       	end.a = alpha;
       	line.SetColors (start, end);
		*/
		//gameObject.GetComponent<TrailRenderer>().renderer.material.SetColor("_TintColor", new Color(255,255,255, amt));
		
	}
}
