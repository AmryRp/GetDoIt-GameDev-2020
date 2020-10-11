using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public PlayerController followTarget;
    [SerializeField]
    private Vector3 fixedTargetPos;
    private Vector3 targetPos;
    public float moveSpeed;
    //private static bool cameraExists;
    private Rigidbody2D PlayerDrag;
    public Camera camera;
    public float minZoom, maxZoom,maxVelocity;
    private void Start()
    {
        if (followTarget == null)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        Application.targetFrameRate = 60;
        PlayerDrag = followTarget.GetComponent<Rigidbody2D>();
        camera.GetComponent<Camera>();
    }
    void Update()
    {
        if (followTarget == null)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        cameraMove();
    }
    public void cameraMove()
    {
        cameraZoom();
        targetPos = new Vector3(followTarget.transform.position.x + fixedTargetPos.x, 
            followTarget.transform.position.y + fixedTargetPos.y, transform.position.z + fixedTargetPos.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        
    }
    public void cameraZoom()
    {
        camera.orthographicSize = Mathf.Lerp(minZoom, maxZoom, 
        Mathf.InverseLerp(0.0f, maxVelocity, PlayerDrag.velocity.magnitude));
    }
}