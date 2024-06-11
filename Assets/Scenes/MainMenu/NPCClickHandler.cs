using UnityEngine;

public class NPCClickHandler : MonoBehaviour
{
    private StatusManager statusManager;

    private void Start()
    {
        statusManager = GetComponent<StatusManager>();
        if (statusManager == null)
        {
            Debug.LogError("StatusManager component not found on " + gameObject.name);
        }
    }

    private void OnMouseDown()
    {
        if (statusManager != null)
        {
            statusManager.ToggleStatusWindow();
        }
    }
}
