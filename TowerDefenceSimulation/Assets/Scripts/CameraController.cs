using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    [Header("Camera Settings")]
    [SerializeField] private float zoomIncrement = 1;
    [Tooltip("Zoom minimum cannot be less than 1")]
    [SerializeField] private float zoomMin = 1;
    [SerializeField] private float zoomMax = 10;

    private Vector3 dragStart;

    private void Start()
    {
        if (!mainCamera) mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            dragStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && mainCamera.orthographicSize > zoomMin)
            mainCamera.orthographicSize -= zoomIncrement;

        // Zoom out
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize < zoomMax)
            mainCamera.orthographicSize += zoomIncrement;
        
        // Drag camera
        if (Input.GetMouseButton(1))
        {
            Vector3 direction = dragStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position += direction;
        }
    }
}
