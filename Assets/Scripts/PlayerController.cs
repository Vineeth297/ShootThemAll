using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;
	
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private GameObject projectile;  
	
	public Transform targetToMove;
	public Transform bulletSpawnPosition;
	public Transform pivotObj;

	public List<Vector3> pathPoints;
	[HideInInspector]public RaycastHit hit;
	private Ray _ray;
	
	
	[SerializeField]private LineRenderer _lineRenderer;
	
	public int totalBounces = 3;
	public float lineOffset = 0.01f;
	public float angle = 1f;

	public Vector3 direction;
	public Vector3 rayOrigin;
	
	private Camera _camera;

	void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		_camera = Camera.main;
		_lineRenderer.positionCount = totalBounces + 1;
		pathPoints = new List<Vector3>();
	}

	// void FixedUpdate()
	// {
	// 	direction = targetToMove.up;
	// 	rayOrigin = targetToMove.position + lineOffset * direction;
	//
	// 	_lineRenderer.enabled = true;
	// 	_lineRenderer.SetPosition(0,targetToMove.position);
	// 	
	// 	for (int i = 1; i <= totalBounces; i++)
	// 	{
	// 		Physics.Raycast(rayOrigin, direction,out hit,Mathf.Infinity);
	// 		Debug.DrawLine(rayOrigin,hit.point);
	// 		direction = Vector3.Reflect(direction.normalized, hit.normal);
	// 		rayOrigin = hit.point + lineOffset * direction;
	// 		_lineRenderer.SetPosition(i,hit.point);
	// 		pathPoints[i] = hit.point;
	// 	}
	// }
	
	void Update()
	{
		direction = targetToMove.up;
		rayOrigin = targetToMove.position + lineOffset * direction;

		_lineRenderer.enabled = true;
		_lineRenderer.SetPosition(0,targetToMove.position);
		
		for (int i = 1; i <= totalBounces; i++)
		{
			Physics.Raycast(rayOrigin, direction,out hit,Mathf.Infinity);
			Debug.DrawLine(rayOrigin,hit.point);
			direction = Vector3.Reflect(direction.normalized, hit.normal);
			rayOrigin = hit.point + lineOffset * direction;
			_lineRenderer.SetPosition(i,hit.point);
			
			//pathPoints[i] = hit.point;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			for (int j = 0; j < _lineRenderer.positionCount; j++)
			{
				pathPoints.Add(_lineRenderer.GetPosition(j));
				
			}
		}
		print(_lineRenderer.positionCount);
		float swipedDirection = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;

		if (Input.GetMouseButton(0))
		{
			if (swipedDirection > 0)
				targetToMove.RotateAround(pivotObj.position, Vector3.forward, Time.deltaTime * angle);
			if (swipedDirection < 0)
				targetToMove.RotateAround(pivotObj.position, Vector3.back, Time.deltaTime * angle);
		}

		// if (Input.GetMouseButtonDown(0))
		// {
		// 	
		// 	GameObject obj = Instantiate(projectile, bulletSpawnPosition.position, Quaternion.identity);
		// }
		
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

	public void Shoot()
	{
		
	}
	
}
