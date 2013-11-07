using UnityEngine;
using System.Collections;
 
public class SmoothFollow : MonoBehaviour
{
 
    GameObject aggroTarget; // object to look at or follow
    GameObject player; // player object for moving
 
    public float smoothTime = 1.5f;    // time for dampen
    public bool cameraFollowX = true; // camera follows on horizontal
    public bool cameraFollowY = true; // camera follows on vertical
    public bool cameraFollowHeight = true; // camera follow CameraTarget object height
    public float cameraHeight = 2.5f; // height of camera adjustable
    public Vector2 velocity; // speed of camera movement
 
    private Transform thisTransform; // camera Transform
 
	
	public int aggroDistance;
	public bool isAggro;
	
    // Use this for initialization
    void Start()
    {
        thisTransform = transform;
		isAggro = true;
		
		aggroTarget = GameObject.FindGameObjectWithTag("Player");
		player = gameObject;
    }
 
    // Update is called once per frame
    void Update()
    {
		//SimpleMovement pM = aggroTarget.GetComponent<SimpleMovement>();
		//int lastDir = pM.lastDir;
		Vector3 heading = thisTransform.position - aggroTarget.transform.position;
		
		//float distance = heading.magnitude;
		//Vector3 direction = heading / distance;  // This is now the normalized direction.		
		
		//Debug.Log("heading: " + heading);
		
		if (isAggro) {
			if (cameraFollowX) {
				if (heading.x > 0)
	            	thisTransform.position = new Vector3(Mathf.SmoothDamp(thisTransform.position.x, aggroTarget.transform.position.x + aggroDistance, ref velocity.x, smoothTime), thisTransform.position.y, thisTransform.position.z);
	        	else if (heading.x <= 0)
	            	thisTransform.position = new Vector3(Mathf.SmoothDamp(thisTransform.position.x, aggroTarget.transform.position.x - aggroDistance, ref velocity.x, smoothTime), thisTransform.position.y, thisTransform.position.z);
			}
	        if (cameraFollowY) {
	            thisTransform.position = new Vector3(thisTransform.position.x, Mathf.SmoothDamp(thisTransform.position.y, aggroTarget.transform.position.y, ref velocity.y, smoothTime), thisTransform.position.z);
	        }
	        if (!cameraFollowX & cameraFollowHeight) {
	            // to do
	        }
		}
    }
}