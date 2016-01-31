using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    public float walkSpeed = 0.15f;
    public float runSpeed = 1.0f;
    public float sprintSpeed = 2.0f;
    public float flySpeed = 4.0f;

    public float turnSmoothing = 3.0f;
    public float aimTurnSmoothing = 15.0f;
    public float speedDampTime = 0.1f;

    public float jumpHeight = 5.0f;
    public float jumpCooldown = 1.0f;

    private float timeToNextJump = 0;

    private float speed;

    private Vector3 lastDirection;

    private Animator anim;
    private int speedFloat;
    private int jumpBool;
    private int hFloat;
    private int vFloat;
    private int aimBool;
    private int flyBool;
    private int groundedBool;
    private Transform cameraTransform;
    private AudioSource audioSource;

    private float h;
    private float v;

    private bool aim;

    private bool run;
    private bool sprint;

    private bool isMoving;

    // fly
    private bool fly = false;
    private float distToGround;
    private float sprintFactor;

    private Rigidbody rigidBody;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        rigidBody = GetComponent<Rigidbody>();

        speedFloat = Animator.StringToHash("Speed");
        jumpBool = Animator.StringToHash("Jump");
        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");
        aimBool = Animator.StringToHash("Aim");
        // fly
        flyBool = Animator.StringToHash("Fly");
        groundedBool = Animator.StringToHash("Grounded");
        distToGround = GetComponent<Collider>().bounds.extents.y;
        sprintFactor = sprintSpeed / runSpeed;

        audioSource = GetComponent<AudioSource>();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.05f);
    }

    void Update()
    {
        // fly
        /*if(Input.GetButtonDown ("Fly"))
			fly = !fly;*/
        //aim = Input.GetButton("Aim");
        aim = Input.GetKey(KeyCode.F);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        run = !Input.GetButton("Sprint");
        //run = !Input.GetButton ("Run");
        //sprint = Input.GetButton ("Sprint");
        isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.01;

    }

    void FixedUpdate()
    {
        anim.SetBool(aimBool, IsAiming());
        anim.SetFloat(hFloat, h);
        anim.SetFloat(vFloat, v);

        // Fly
        anim.SetBool(flyBool, fly);
        rigidBody.useGravity = !fly;
        anim.SetBool(groundedBool, IsGrounded());
        if (fly)
            FlyManagement(h, v);

        else
        {
            MovementManagement(h, v, run, sprint);
            JumpManagement();
        }
    }
    void LateUpdate()
    {
        //if()
    }
    // fly
    void FlyManagement(float horizontal, float vertical)
    {
        Vector3 direction = Rotating(horizontal, vertical);
        rigidBody.AddForce(direction * flySpeed * 100 * (sprint ? sprintFactor : 1));
    }

    void JumpManagement()
    {
        if (rigidBody.velocity.y < 10) // already jumped
        {
            anim.SetBool(jumpBool, false);
            if (timeToNextJump > 0)
                timeToNextJump -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            //anim.SetBool(jumpBool, true);
            if (speed > 0 && timeToNextJump <= 0 && !aim)
            {
                //GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0) + transform.forward;
                rigidBody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
                rigidBody.AddForce(transform.forward * jumpHeight, ForceMode.Acceleration);

                timeToNextJump = jumpCooldown;
            }
            else if (timeToNextJump <= 0 && !aim)
            {
                //GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0) + transform.forward;
                rigidBody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
                rigidBody.AddForce(transform.forward * jumpHeight, ForceMode.Acceleration);
                timeToNextJump = jumpCooldown;
            }

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (h != 0 || v != 0)
            {
                rigidBody.AddRelativeForce(Vector3.forward * speed * 10, ForceMode.Force);
            }
        }
    }

    void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
    {
        Rotating(horizontal, vertical);

        if (isMoving)
        {
            if (sprinting)
            {
                speed = sprintSpeed;
            }
            else if (running)
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }

            anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
            if(!audioSource.isPlaying)
                audioSource.Play();

            //Pokretanje pomoću sile
            //Vector3 direction = Rotating(horizontal, vertical);
            //GetComponent<Rigidbody>().AddForce(transform.forward * speed * (sprint ? sprintFactor : 1));
            //GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed * 10, ForceMode.Force);


        }
        else
        {
            speed = 0f;
            anim.SetFloat(speedFloat, 0f);
            audioSource.Pause();
        }
        rigidBody.AddForce(Vector3.forward * speed);
    }

    Vector3 Rotating(float horizontal, float vertical)
    {
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        if (!fly)
            forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;

        float finalTurnSmoothing;

        if (IsAiming())
        {
            targetDirection = forward;
            finalTurnSmoothing = aimTurnSmoothing;
        }
        else
        {
            targetDirection = forward * vertical + right * horizontal;
            finalTurnSmoothing = turnSmoothing;
        }

        if ((isMoving && targetDirection != Vector3.zero) || IsAiming())
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            // fly
            if (fly)
                targetRotation *= Quaternion.Euler(90, 0, 0);

            Quaternion newRotation = Quaternion.Slerp(rigidBody.rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
            rigidBody.MoveRotation(newRotation);
            lastDirection = targetDirection;
        }
        //idle - fly or grounded
        if (!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
        {
            Repositioning();
        }

        return targetDirection;
    }

    private void Repositioning()
    {
        Vector3 repositioning = lastDirection;
        if (repositioning != Vector3.zero)
        {
            repositioning.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(repositioning, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }

    void OnCollision(Collider col)
    {
        if (col.GetComponent<BoxCollider>())
        {
            //h = 0;
           // v = 0;
        }
    }
    public bool IsFlying()
    {
        return fly;
    }

    public bool IsAiming()
    {
        return aim && !fly;
    }

    public bool isSprinting()
    {
        return sprint && !aim && (isMoving);
    }
}
