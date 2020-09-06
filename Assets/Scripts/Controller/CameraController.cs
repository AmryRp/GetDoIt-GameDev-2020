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
    private Rigidbody PlayerDrag;
    private void Start()
    {
        /*DontDestroyOnLoad(transform.gameObject);
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else { Destroy(gameObject); }*/
        Application.targetFrameRate = 60;
        PlayerDrag = followTarget.GetComponent<Rigidbody>();
    }
    void Update()
    {
        
        targetPos = new Vector3(followTarget.transform.position.x+fixedTargetPos.x, followTarget.transform.position.y+ fixedTargetPos.y, transform.position.z+ fixedTargetPos.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}