using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * 20f;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 60f, 100f);
        }
    }
}
