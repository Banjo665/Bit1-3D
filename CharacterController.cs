using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float jumpVel = 25;
        public float distToGrounded = 0.1f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, turnInput, jumpInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }


    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rb = GetComponent<Rigidbody>();
        forwardInput = turnInput = jumpInput = 0;
    }

    void GetInput()
    {
       
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
    }

    void FixedUpdate()
    {
        
        Jump();

        rb.velocity = transform.TransformDirection(velocity);
    }

   

    void Jump()
    {
        if(jumpInput > 0 && Grounded())
        {
            velocity.y = moveSetting.jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= physSetting.downAccel;
        }
    }
    
}
