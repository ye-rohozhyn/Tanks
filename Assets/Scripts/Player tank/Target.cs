using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Camera followCamera;
    [SerializeField] private LayerMask layers;
    [SerializeField] private float rayDistance = 100;

    private void FixedUpdate()
    {
        Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, layers))
        {
            transform.position = hit.point;
        }
    }
}
