using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyFunction : MonoBehaviour
{
	
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private GameObject projectile;  
	[SerializeField] private LineRenderer _lineRenderer;

	public Transform targetToMove;
	public Transform bulletSpawnPosition;
	public Transform pivotObj;
	
	public List<GameObject> colliedObjects;
	public List<Vector3> pathPoints;

	[HideInInspector]public RaycastHit hit;

	public int totalBounces = 2;
	
	public float lineOffset = 0.01f;
	public float angle = 1f;

	public Vector3 direction;
	public Vector3 rayOrigin;

	private Camera _camera;
	private Ray _ray;
	
	private void Start()
	{
		_camera = Camera.main;
		_lineRenderer.positionCount = totalBounces + 1;
		pathPoints = new List<Vector3>();
	}

	void Update()
	{
		direction = targetToMove.up;
		rayOrigin = targetToMove.position + lineOffset * direction;

		_lineRenderer.enabled = true;
		_lineRenderer.SetPosition(0,targetToMove.position);
		
		for (int i = 1; i <= totalBounces; i++)
		{
			//Physics.Raycast(rayOrigin, direction,out hit,Mathf.Infinity);
			//Debug.DrawLine(rayOrigin,hit.point);
			
			if (!Physics.Raycast(rayOrigin, direction, out hit,50f)) return;
			
			if (hit.transform.CompareTag("Mirror"))
			{
				direction = Vector3.Reflect(direction.normalized, hit.normal);
				rayOrigin = hit.point + lineOffset * direction;
				_lineRenderer.SetPosition(i,hit.point);

				colliedObjects.Add(hit.collider.gameObject);
			}
			else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("BoundingBox") || hit.collider.CompareTag("Enemy"))
			{
				Debug.DrawLine(rayOrigin,hit.point);
				_lineRenderer.SetPosition(i,hit.point);
			}
		}
		//_lineRenderer.SetPosition(1,targetToMove.position + targetToMove.transform.up * 50);

		if (Input.GetKeyDown(KeyCode.A))
		{
			for (int j = 0; j < _lineRenderer.positionCount; j++)
			{
				pathPoints.Add(_lineRenderer.GetPosition(j));
			}
			StartCoroutine(Shoot());
		}
		
		float swipedDirection = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;

		if (Input.GetMouseButton(0))
		{
			if (swipedDirection > 0)
				targetToMove.RotateAround(pivotObj.position, Vector3.forward, Time.deltaTime * angle);
			if (swipedDirection < 0)
				targetToMove.RotateAround(pivotObj.position, Vector3.back, Time.deltaTime * angle);
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

	IEnumerator Shoot()
	{
		GameObject bullet = Instantiate(projectile, bulletSpawnPosition);
		bullet.transform.position = targetToMove.position;
		for (int i = 1; i < _lineRenderer.positionCount; i++)
		{
			bullet.transform.DOMove(_lineRenderer.GetPosition(i),0.6f).SetEase(Ease.Linear); 
			yield return new WaitForSeconds(0.6f);
		}
		
		pathPoints.Clear();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Bullet"))
		{
			gameObject.SetActive(false);
		}
	}
}
