using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public Transform hidePoint;       // assign in Inspector
    public Transform playerCamera;    // assign or auto-detect

    public float hideDistance = 2f;

    private Vector3 originalCamPos;
    public bool isHidden = false;

    void Start()
    {
        // auto assign camera if not set
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        originalCamPos = playerCamera.localPosition;
    }

    void Update()
    {
        //runtimecheck
        if (hidePoint == null)
        {
            Debug.LogError("HidePoint NOT assigned!");
            return;
        }

        float dist = Vector3.Distance(transform.position, hidePoint.position);
        //only allow hiding when near bed
        if (dist < hideDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Hiding triggered");

                isHidden = !isHidden;

                if (isHidden)
                    Hide();
                else
                    Unhide();
            }
        }
    }
    void Hide()
    {
        Debug.Log("Hiding now");
        //move camera down
        playerCamera.localPosition = new Vector3(0f, -1f, 0f);
    }

    void Unhide()
    {
        Debug.Log("Unhiding");
        playerCamera.localPosition = originalCamPos;
    }
}