using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

	[SerializeField]private float xForce;
	[SerializeField]private float xSpeed;

	public Transform targetToMove;
	public Transform pivotObj;

	public float angle = 1f;
	void Update()
	{
		float swipe = Input.GetAxis("Mouse X");
		
		if (Input.GetMouseButton(0))
		{
			print(swipe);
			targetToMove.RotateAround(pivotObj.position, Vector3.right,Time.deltaTime * angle);
		}
		
	#if UNITY_EDITOR
		xForce = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed : 0;
	#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xForce = touchDeltaPosition.x*swipeSpeed*Mathf.Deg2Rad;
          }
	#endif
	}
    
}
