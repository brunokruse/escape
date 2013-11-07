using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
	
	public Transform rotationPoint;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    	transform.RotateAround (rotationPoint.position, Vector3.back, 20 * Time.deltaTime);
	}
}
