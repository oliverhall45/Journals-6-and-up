using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowCamera2D : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform target;
    private Vector3 offset;
    public float speed;

    private float leftCameraBoundary;
    private float rightCameraBoundary;
    private float bottomCameraBoundary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - target.position;
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        tilemap.CompressBounds();

        Camera cam = GetComponent<Camera>();

        float orthoSize = cam.orthographicSize;
        Vector3 viewportHalfSize = new(orthoSize * cam.aspect, orthoSize);

        Vector3Int min = tilemap.cellBounds.min;
        Vector3Int max = tilemap.cellBounds.max;

        leftCameraBoundary = min.x + viewportHalfSize.x;
        rightCameraBoundary = max.x - viewportHalfSize.x;
        bottomCameraBoundary = min.y + viewportHalfSize.y;
    }

    private void LateUpdate()
    {

        Vector3 desiredPosition = target.position + offset;
        Vector3 steppedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);

        steppedPosition.x = Mathf.Clamp(steppedPosition.x, leftCameraBoundary, rightCameraBoundary);
        steppedPosition.y = Mathf.Clamp(steppedPosition.y, bottomCameraBoundary, steppedPosition.y);

        transform.position = steppedPosition;
    }
}
