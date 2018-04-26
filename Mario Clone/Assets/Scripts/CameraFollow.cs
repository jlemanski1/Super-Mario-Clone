using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform cameraTarget;      // Target to follow
    public float cameraSpeed;           // Follow speed

    // Camera position constraints
    public float minX;
    public float minY;

    public float maxX;
    public float maxY;

	
    // Adjust the camera's position to smoothly follow a target
	void FixedUpdate () {
        // Camera has target
		if (cameraTarget) {
            // Lerp to new position
            var newPos = Vector2.Lerp(transform.position, cameraTarget.position, cameraSpeed);
            
            // Define camera's new position
            var pos = new Vector3(newPos.x, newPos.y, -10f);

            //Clamp camera's position within constraints
            var clampX = Mathf.Clamp(pos.x, minX, maxX);
            var clampY = Mathf.Clamp(pos.y, minY, maxY);

            // Set new position
            transform.position = new Vector3(clampX, clampY, -10f);
        }
	}
}
