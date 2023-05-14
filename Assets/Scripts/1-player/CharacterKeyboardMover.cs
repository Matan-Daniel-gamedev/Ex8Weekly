using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * This component moves a player controlled with a CharacterController using the keyboard.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover : MonoBehaviour
{
    [Tooltip("Speed of player keyboard-movement, in meters/second")]
    [SerializeField] float speed = 3.5f;
    [SerializeField] float gravity = 9.81f;

    private CharacterController cc;

    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction jumpAction;
    [SerializeField] InputAction sprintAction;
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
    }
    void OnValidate()
    {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);
        if (moveAction.bindings.Count == 0)
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");

        if (jumpAction == null)
            jumpAction = new InputAction(type: InputActionType.Button);
        if (jumpAction.bindings.Count == 0)
            jumpAction.AddBinding("<Keyboard>/space");

        if (sprintAction == null)
            sprintAction = new InputAction(type: InputActionType.Button);
        if (sprintAction.bindings.Count == 0)
            sprintAction.AddBinding("<Keyboard>/leftShift");
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    Vector3 velocity = new Vector3(0, 0, 0);

    void Update()
    {
        if (cc.isGrounded)
        {
            Vector3 movement = moveAction.ReadValue<Vector2>(); // Implicitly convert Vector2 to Vector3, setting z=0.
            velocity.x = movement.x * speed;
            velocity.z = movement.y * speed;

            if (Keyboard.current.ctrlKey.isPressed)
            {
                velocity.x /= speed;
                velocity.z /= speed;
                // Move the camera up:
                Vector3 cameraPosition = transform.Find("UpDown/Main Camera").position;
                //Debug.Log(cameraPosition.y);
                cameraPosition.y = 15.88f;
                transform.Find("UpDown/Main Camera").position = cameraPosition;
            }
            else
            {
                Vector3 cameraPosition = transform.Find("UpDown/Main Camera").position;
                //Debug.Log(cameraPosition.y);
                cameraPosition.y = 16.88f;
                transform.Find("UpDown/Main Camera").position = cameraPosition;
            }

            if (sprintAction.ReadValue<float>() > 0)
            {
                velocity.x *= 10.0f / speed;
                velocity.z *= 10.0f / speed;
            }

            if (jumpAction.triggered)
            {
                velocity.y = Mathf.Sqrt(2 * gravity * 3); // Calculate jump velocity
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        // Move in the direction you look:
        velocity = transform.TransformDirection(velocity);

        cc.Move(velocity * Time.deltaTime);
    }
}