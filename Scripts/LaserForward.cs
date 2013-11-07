using UnityEngine;
using System.Collections;

public class LaserForward : MonoBehaviour {
	
	float laserSpeed = 20.0f;
	float lifetime = 6.0f;
	
	// Use this for initialization
	void Start () {
		Destroy(gameObject, lifetime); // wait till it gets offscreen
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right * Time.deltaTime * laserSpeed);
	}
	
	void OnCollisionEnter(Collision other) {
		//Debug.Log ("collide with player!");
		
		if (other.gameObject.tag == "Player") {
			GameObject p1 = GameObject.FindGameObjectWithTag("Player");
			SimpleMovement pM = (SimpleMovement) p1.GetComponent(typeof(SimpleMovement));
			
			pM.takeDamage();
			//pM.currentHp -= 1;
			
			//Debug.Log("pm: " + pM.currentHp);
			
			//GameObject.Instantiate(damagePrefab, other.transform.position,  Quaternion.Euler(0.0f, 0.0f, 0.0f));
			Destroy(gameObject);
			
			//if (pM.playerHP <= 0 && !pM.isDead) {
				//SplitMeshIntoTriangles blowUp = (SplitMeshIntoTriangles) player.GetComponent(typeof(SplitMeshIntoTriangles));
				//blowUp.blowUp();
			//}
		}
		
    }
    
}
