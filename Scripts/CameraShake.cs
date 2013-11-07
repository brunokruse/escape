using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
	float duration = 0.3f;
	float magnitude = 0.5f;
	GameObject cam;
	
	// Use this for initialization
	void Start () {
		cam = GameObject.Find("tk2dCamera");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void shakeCam() {
	
		StartCoroutine(Shake());
	}
	
	IEnumerator Shake() {
        
	    float elapsed = 0.0f;
	    
	    Vector3 originalCamPos = cam.transform.position; //Camera.main.transform.position;
	    
	    while (elapsed < duration) {
	        
	        elapsed += Time.deltaTime;          
	        
	        float percentComplete = elapsed / duration;         
	        float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
	        
	        // map noise to [-1, 1]
	        float x = Random.value * 2.0f - 1.0f;
	        float y = Random.value * 2.0f - 1.0f;
	        x *= magnitude * damper;
	        y *= magnitude * damper;
	        
	        cam.transform.position = new Vector3(x, y, originalCamPos.z);
	            
	        yield return null;
	    }
	    
	    cam.transform.position = originalCamPos;
	}
}
