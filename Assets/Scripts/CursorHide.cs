using UnityEngine;

public class CursorHide : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;
        
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
