using UnityEngine;
using System.Collections;

public class EightWayModified : MonoBehaviour {
	
	public Transform laser;
	
	int radiusX = 1;
	int radiusY = 1;
	int numPoints = 8;
	
	// rate of fire
	private float NextFireAt = -1;
	private float FireRate = 5.0f;
	private float ventTime = 2.0f;
	private float ventOverTime = -1;
	
	SteamBotAI checker;
		
	// Use this for initialization
	void Start () {
		//fireLasers();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		checker = gameObject.transform.parent.GetComponent<SteamBotAI>();
		
		if (checker.isTeleported) {			
			if(Time.time >= NextFireAt) {
				if(checker.fire){
					fireLasers();
					NextFireAt = Time.time + FireRate;
					checker.fire = false;
				}
			}
			if(Time.time >= ventOverTime) {
				checker.isVenting = false;
			}
		}
	}
	
	public void fireLasers() {
		
			Vector3 centerPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			int theta = 0;
			
			for (var pointNum = 0; pointNum < numPoints; pointNum++) {
				
			    // "i" now represents the progress around the circle from 0-1
			    // we multiply by 1.0 to ensure we get a fraction as a result.
			    float i = (float)(pointNum * 1.0) / numPoints;
			 
			    // get the angle for this step (in radians, not degrees)
			    float angle = i * Mathf.PI * 2;
			 
			    // the X & Y position for this angle are calculated using Sin & Cos
			    float x = Mathf.Sin(angle) * radiusX;
			    float y = Mathf.Cos(angle) * radiusY;
			 
			    Vector3 pos = new Vector3(x, y, 0) + centerPos;
	
				Instantiate (laser, pos, Quaternion.Euler(0, 0, theta + 90));
				theta -= 45;
				
			}
			ventOverTime = Time.time + ventTime;
			checker.vent ();
	}
	
}
