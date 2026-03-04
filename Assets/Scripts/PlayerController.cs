using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;

    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidBody;
    private bool jumpTriggered;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidBody = GetComponent<Rigidbody>();

        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(
            _playerInputController.MovementInputVector.x, 
            0, 
            _playerInputController.MovementInputVector.y)
            * speed;

        velocity.y = _rigidBody.linearVelocity.y;

        if(jumpTriggered)
        {
            velocity.y = jumpSpeed;
            jumpTriggered = false;
        }

        _rigidBody.linearVelocity = velocity;
    }

    private void JumpButtonPressed()
    {
        if(_groundController.IsGrounded)
        {
            jumpTriggered = true;
        }

    }
}
