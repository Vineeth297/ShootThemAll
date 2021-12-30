using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	
	[SerializeField]private float xForce;
	[SerializeField]private float xSpeed;
	
	public Transform targetToMove;
	public Transform pivotObj;

	RaycastHit hit;
	
	public int totalBounces = 3;
	public float lineOffset = 0.01f;
	public float angle = 1f;
	
	
	void Update()
	{

		Vector3 direction = targetToMove.up;
		Vector3 rayOrigin = targetToMove.position + lineOffset * direction;
print(rayOrigin);
		float swipedDirection = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
		
		Debug.DrawLine(rayOrigin,hit.point);
		
		if (Input.GetMouseButton(0))
		{
			if (swipedDirection > 0)
			{
				targetToMove.RotateAround(pivotObj.position, Vector3.forward, Time.deltaTime * angle);
				for (int i = 1; i <= totalBounces; i++)
				{
					Physics.Raycast(rayOrigin, direction,out hit,Mathf.Infinity);

					direction = Vector2.Reflect(direction.normalized, hit.normal);
					rayOrigin = hit.point + lineOffset * direction;
				}
			}

			if(swipedDirection < 0)
			{	
				targetToMove.RotateAround(pivotObj.position, Vector3.back,Time.deltaTime * angle);
				for (int i = 1; i <= totalBounces; i++)
				{
					Physics.Raycast(rayOrigin, direction,out hit,Mathf.Infinity);
					
					direction = Vector2.Reflect(direction.normalized, hit.normal);
					rayOrigin = hit.point + lineOffset * direction;
				}
			}
				
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
