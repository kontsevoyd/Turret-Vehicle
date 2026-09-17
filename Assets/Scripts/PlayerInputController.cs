using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent Tap;
    public UnityEvent<float> SwipeHorizontal = new();

    private void Update()
    {
        bool tapped = false;

        if (Touchscreen.current != null)
        {
            tapped = Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
        }
        else if (Mouse.current != null)
        {
            tapped = Mouse.current.leftButton.wasPressedThisFrame;
        }

        if (tapped)
        {
            Tap.Invoke();
        }

        float deltaX = 0f;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            deltaX = Touchscreen.current.primaryTouch.delta.ReadValue().x;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            deltaX = Mouse.current.delta.ReadValue().x;
        }

        if (Mathf.Abs(deltaX) > 0.01f)
            SwipeHorizontal.Invoke(deltaX);
    }
}
