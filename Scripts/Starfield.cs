using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Starfield : MonoBehaviour {
	
	public Transform star;
	float speed = 1.0f;
	
	List<Vector3> starPositions = new List<Vector3>();
	List<Quaternion> rotations = new List<Quaternion>();
	// Use this for initialization
	void Start () {
	
		// first generate the start coords to keep track of
		for (int s = 0; s < 130; s++) {
		
			// 10 - 160x
			// 10 -10 y
			
			Vector3 position = new Vector3(Random.Range(-600.0f, 400.0f), 
										   Random.Range(-25.0f, 50.0f),
										   Random.Range(10.0f, 30.0f));
			
			Quaternion tmpRot =  Random.rotation;
			tmpRot.x = 0;
			tmpRot.y = 0;
			
			rotations.Add(tmpRot);
			starPositions.Add(position);
		}
		
		
		for (int w = 0; w < 130; w++) {
		
			// generate random pos for the
			Vector3 placePosition = starPositions[w];
			Instantiate(star, placePosition,  /*rotations[w]*/ Quaternion.identity);
				
			
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
	}
}
