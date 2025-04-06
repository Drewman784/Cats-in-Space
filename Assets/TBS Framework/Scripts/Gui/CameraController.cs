using UnityEngine;

namespace TbsFramework.Gui
{
    /// <summary>
    /// Simple movable camera implementation.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public float ScrollSpeed = 1;
        public float ScrollEdge = 0.05f;

        // Camera Zoom Script from: https://gist.github.com/bendux/3cb282d58a7c76103e303088a3905c8c
        private float zoom;
        private float zoomMultiplier = 4f;
        private float minZoom = 0.5f;
        private float maxZoom = 2f;
        private float velocity = 0f;
        private float smoothTime = 0.25f;

        [SerializeField] private Camera cam;

        private void Start()
        {
            zoom = cam.orthographicSize;
        }

        void Update()
        {
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
            {
                transform.Translate(transform.right * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
            {
                transform.Translate(transform.right * Time.deltaTime * -ScrollSpeed, Space.World);
            }
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
            {
                transform.Translate(transform.up * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
            {
                transform.Translate(transform.up * Time.deltaTime * -ScrollSpeed, Space.World);
            }

            // Clamp camera position
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0f, 3f);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0f, 3f);
            transform.position = clampedPosition;

            // Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zoom -= scroll * zoomMultiplier;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
        }
    }
}

