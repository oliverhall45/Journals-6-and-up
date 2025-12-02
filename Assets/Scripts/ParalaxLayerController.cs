using UnityEngine;

public class ParalaxLayerController : MonoBehaviour
{
    [SerializeField] private Camera viewCamera;
    [SerializeField] private float cameraDeltaScalar = 1f;

    private Vector3 cameraStartPos;
    private Vector3 layerStartPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraStartPos = viewCamera.transform.position;
        layerStartPos = transform.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 cameraDelta = viewCamera.transform.position - cameraStartPos;

        float deltaX = cameraDelta.x * cameraDeltaScalar;
        float deltaY = cameraDelta.y * cameraDeltaScalar;

        transform.position = layerStartPos + new Vector3(deltaX, deltaY);
    }
}
