using Unity.Mathematics;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Camera ) ) ]
public class MainCameraManager : MonoBehaviour
{
	public Transform targetTransform;
	public float smoothSpeed;
	public float offset;

	private Camera _mainCamera;
	public static MainCameraManager Instance{ get; private set; }

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		_mainCamera = GetComponent<Camera>();
	}

	private void Update()
	{
		if( targetTransform && targetTransform.position.y >= -3 )
			FollowTarget();
	}

	private void FollowTarget()
	{
		Vector3 targetPosition = new( transform.position.x, targetTransform.position.y, offset );
		Vector3 position;

		position = smoothSpeed > 0
			           ? Vector3.Lerp( transform.position, targetPosition, smoothSpeed * Time.deltaTime )
			           : targetPosition;

		transform.position = position;
	}

	//xMin, xMax, yMin, yMax
	public float4 GetDimensionsAtDepth( float depth )
	{
		Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint( new( 0, 0, depth ) );
		Vector3 topRight = _mainCamera.ViewportToWorldPoint( new( 1, 1, depth ) );

		return new( bottomLeft.x, topRight.x, bottomLeft.y, topRight.y );
	}
}