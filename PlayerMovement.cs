using UnityEngine;
using Photon.Pun;
public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public float wallJumpRange;
    public float rightMultiplerWallJump;
    public float acceleration = 10;
    public float maxSpeed = 12;
    public float gravity = 2f;
    public float jumpHeight = 30f;
    public float crouchSpeed = 5f;
    public float distanceDownIsGrounded;

    private Transform cam;
    private CharacterController charController;

    private Vector3 forwardMovement;
    private Vector3 rightMovement;
    private Vector3 vectorSpeed;
    private Vector3 refCurrentVeloc;
    private Vector3 smoothPosition;

    private int wallLayer = 1 << 12;

    private bool isJumping = false;
    private bool triggerJump;
    private bool previouslyJumped;
    private bool isCrouching;
    private bool triggerCrouch;
    private bool cantMove;
    private bool triggerWallJump;

    private float vertInput;
    private float horiInput;
    private float upwardsVeloc = 0;
    private float charHeight;
    private float camLocalPos;
    private float movementSpeed = 0;

    private RaycastHit wallJumpHit;
    private Ray wallJumpRay;
    private Quaternion smoothRotation;
    private void Start()
    {
        cam = transform.Find("LookRoot");
        camLocalPos = cam.localPosition.y;
        charController = GetComponent<CharacterController>();
        charHeight = charController.height;
    }
    private void Update()
    {
        if(photonView.IsMine == false)
        {
            return;
        }
        wallJumpRay = new Ray(transform.position, (transform.forward + transform.right * Input.GetAxisRaw("Horizontal") * rightMultiplerWallJump));
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && isGrounded())
        {
            triggerJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isJumping)
        {
            triggerWallJump = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            triggerCrouch = true;
        }
        else
        {
            triggerCrouch = false;
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false)
        {
            SmoothMove();
            return;
        }
        if (triggerJump)
        {
            Jump();
            triggerJump = false;
            previouslyJumped = true;
        }
        else if (triggerWallJump)
        {
            WallJump();
            triggerWallJump = false;
        }
        else
        {
            SetGravity();
        }
        PlayerMove();
        Crouch();
    }

    private void FixedUpdate2()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }
        else
        {
            SetGravity();
        }
        PlayerMove();
    }
    private void PlayerMove()
    {
        if (!isJumping && previouslyJumped)
        {
            movementSpeed = maxSpeed / 2.1f;
            previouslyJumped = false;
        }
        vertInput = Input.GetAxis("Vertical");
        horiInput = Input.GetAxis("Horizontal");
        bool isMoving = (vertInput != 0 || horiInput != 0);
        if (!isMoving || isCrouching)
        {
            movementSpeed = maxSpeed / 2.1f;
        }
        else if (isMoving)
        {
            if (movementSpeed < maxSpeed)
            {
                movementSpeed += Time.fixedDeltaTime * acceleration;
            }
        }
        forwardMovement = vertInput * transform.forward;
        rightMovement = horiInput * transform.right;
        vectorSpeed = Vector3.ClampMagnitude(rightMovement + forwardMovement, 1.1f) * movementSpeed;
        vectorSpeed.y = upwardsVeloc;
        charController.Move(vectorSpeed * Time.fixedDeltaTime);

    }
    public void SmoothMove()
    {
        //transform.position = Vector3.SmoothDamp(transform.position, smoothPosition, ref refCurrentVeloc, 0.02f);
        transform.position = Vector3.Lerp(transform.position, smoothPosition, 0.25f);
        transform.rotation = Quaternion.Lerp(transform.rotation, smoothRotation, 30 * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        isJumping = true;
        upwardsVeloc = jumpHeight;
    }

    private void SetGravity()
    {
        if (isGrounded())
        {
            wallJumpHit = new RaycastHit();
            upwardsVeloc = -gravity;
            isJumping = false;
            //highestHeight = 0;
        }
        else if ((charController.collisionFlags & CollisionFlags.Above) != 0)
        {
            upwardsVeloc = -5 * gravity;
        }
        else
        {
            upwardsVeloc -= gravity;
        }
    }

    private void WallJump()
    {
        RaycastHit tempHit;
        if (Physics.Raycast(wallJumpRay, out tempHit, wallJumpRange, wallLayer))
        {
            if (wallJumpHit.collider != tempHit.collider)
            {
                wallJumpHit = tempHit;
                movementSpeed *= 1.25f;
                movementSpeed = Mathf.Clamp(movementSpeed, 0, 2 * maxSpeed);
                upwardsVeloc = jumpHeight;
            }
            return;
        }
    }

    public bool isGrounded()
    {
        Vector3 bottom = transform.position;
        RaycastHit hit;
        //Debug.DrawLine(bottom, bottom + -transform.up * (distanceDownIsGrounded), Color.red);
        if (charController.isGrounded)
        {
            return true;
        }
        if (!isJumping)
        {
            // ray cast has to ignore players and other potential objects under the stairs
            if (Physics.Raycast(bottom, -transform.up, out hit, distanceDownIsGrounded))
            {
                if (hit.collider.tag == "Stairs")
                {
                    charController.Move(new Vector3(0, -0.25f * hit.distance, 0));
                    return true;
                }
                if (hit.normal != Vector3.up)
                {
                    charController.Move(new Vector3(0, -hit.distance, 0));
                    return true;
                }
            }
        }
        return false;
    }

    private void Crouch()
    {
        float prevHeight = charController.height;
        float h = charHeight;
        if (triggerCrouch)
        {
            isCrouching = true;
            h = charHeight * 0.5f;
            cam.localPosition = Vector3.Lerp(cam.localPosition, new Vector3(cam.localPosition.x, camLocalPos / 2, cam.localPosition.z), crouchSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - charController.height / 2 + 1f, transform.position.z), crouchSpeed * Time.fixedDeltaTime);
            cam.localPosition = Vector3.Lerp(cam.localPosition, new Vector3(cam.localPosition.x, camLocalPos, cam.localPosition.z), crouchSpeed * Time.fixedDeltaTime);
            isCrouching = false;
        }
        charController.height = Mathf.Lerp(charController.height, h, crouchSpeed * Time.fixedDeltaTime);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        } else if (stream.IsReading)
        {
            smoothPosition = (Vector3)stream.ReceiveNext();
            smoothRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
