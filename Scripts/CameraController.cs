using UnityEngine;

public class CameraController : MonoBehaviour
{
    // [SerializeField]
    // private float speed;
    // private float currentPostX;
    // private Vector3 velocity = Vector3.zero;

    // //Follow the player object
    // [SerializeField]
    // private Transform player;

    // [SerializeField]
    // private float aheadDistance;

    // [SerializeField]
    // private float cameraSpeed;
    // private float lookAhead;

    // private void Update()
    // {
    //     //Follow player
    //     transform.position = new Vector3(
    //         player.position.x,
    //         transform.position.y,
    //         transform.position.z
    //     );
    //     lookAhead = Mathf.Lerp(
    //         lookAhead,
    //         aheadDistance * player.localScale.x,
    //         Time.deltaTime * cameraSpeed
    //     );
    // }

    [SerializeField]
    private float FollowSpeed;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    private Transform target;

    //Update is called once per frame
    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(
            transform.position,
            newPos,
            FollowSpeed * Time.deltaTime
        );
    }
}
