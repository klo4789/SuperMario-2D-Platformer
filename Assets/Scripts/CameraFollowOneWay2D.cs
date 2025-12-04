using UnityEngine;

public class CameraFollowOneWay2D : MonoBehaviour
{
    public Transform target;     
    public float yOffset = 0f;   
    public float zOffset = -10f;  
    private float maxX;         

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }

        maxX = transform.position.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 타겟의 X가 더 오른쪽이면 갱신
        if (target.position.x > maxX)
            maxX = target.position.x;

        Vector3 pos = transform.position;
        pos.x = maxX;                         
        pos.y = target.position.y + yOffset; 
        pos.z = zOffset;
        transform.position = pos;
    }
}
