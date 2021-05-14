using UnityEngine;

public class LateUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = targetToFollow.position;
        transform.rotation = targetToFollow.rotation;
    }
}
