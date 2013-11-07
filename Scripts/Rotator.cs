using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
    
	public int rotationType;
	
	void Update() {
        //transform.Rotate(Vector3.right * Time.deltaTime);
		
		if (rotationType == 0)
        	transform.Rotate(Vector3.right * Time.deltaTime * 45, Space.World);
		
		if (rotationType == 1)
			transform.Rotate(Vector3.up * Time.deltaTime * 45, Space.World);

		if (rotationType == 2)
			transform.Rotate(-Vector3.back * Time.deltaTime * 5, Space.World);
    }
}