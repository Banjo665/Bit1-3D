using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class player_script : MonoBehaviour
{
    private float health = 100;
    public float timeToHeal = 3f;
    private float timeFromHit;
    public GameObject BloodImage;

    public float distToGrounded = 0.5f;
    public LayerMask ground;

    public float downAccel = 0.5f;
    public float inputDelay = 0.1f;
    Vector3 velocity = Vector3.zero;

    public float movementSpeed = 5f;
    public float maxStamina = 3f;
    public float sprintingSpeed = 3f;
    public float staminaRegen = 0.5f;
    public float timeToRest = 2f;

    float jumpInput;
    public float jumpVel = 5;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;


    public float stamina;
    public float jumpStamina = 1;
    float startTime;
    GameObject camera;
    Rigidbody rb;
    Vector3 cameraOffset;
    playerSoundControl soundControl;

    public float crouchingSpeed = 5f;
    float nextPos;
    bool crouching;
    bool crouched;
    bool standing;

    private Image image;
    
    void Start(){
        rb = GetComponent<Rigidbody>();
        stamina = maxStamina;
        startTime = -timeToRest;
        camera = Camera.main.gameObject;
        cameraOffset = Vector3.up;
        soundControl = GetComponent<playerSoundControl>();
        image = BloodImage.GetComponent<Image>();
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }

    void Update(){
        Color tempColor = image.color;
        tempColor.a = 1f - health / 100f;
        image.color = tempColor;
        if(timeFromHit <= 0 && health <= 100){
            health += Time.deltaTime;
        }
        else if(timeFromHit > 0){
            timeFromHit -= Time.deltaTime;
        }
    }

    void FixedUpdate(){
        if(health <= 0){
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float sprintInput = Input.GetAxis("Sprint");
        jumpInput = Input.GetAxis("Jump");
        float crouchInput = Input.GetAxis("Crouch");
        sprintInput *= Mathf.Max(0, verticalInput);

        int sprintSpeed = 1;
        if(sprintInput == 1 && Time.time - startTime >= timeToRest){
            stamina = stamina - Time.deltaTime;
        }
        else{
            stamina = stamina + Time.deltaTime * staminaRegen;
            sprintSpeed = 0;
        }
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        if(stamina == 0){
            startTime = Time.time;
        }
        float speed = movementSpeed + sprintInput * sprintSpeed * sprintingSpeed;
        
        if(crouched){
            speed *= 0.3f;
        }
        if(crouchInput == 1 && !standing && !crouching && !crouched && Grounded()){
            crouching = true;
            nextPos = camera.transform.position.y - 0.4f;
        }
        if(crouchInput == 0 && !standing && !crouching && crouched){
            standing = true;
            nextPos = camera.transform.position.y + 0.4f;
        }
        if(crouching || standing)
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(camera.transform.position.x, nextPos, camera.transform.position.z), Time.deltaTime * crouchingSpeed);

        if(Vector3.Distance(camera.transform.position, new Vector3(camera.transform.position.x, nextPos, camera.transform.position.z)) < 0.05f && (crouching || standing)){
            crouched = crouching;
            standing = false;
            crouching = false;
        }
        
        Vector3 rotatedForward = Quaternion.Euler(0, 90, 0) * transform.forward;
        Vector3 movementY = rotatedForward * horizontalInput * speed * Time.deltaTime;
        Vector3 movementX = transform.forward * verticalInput * speed * Time.deltaTime;
        
        Jump();
        rb.velocity = transform.TransformDirection(velocity);
        transform.position += movementX + movementY;
        

        if(sprintSpeed == 1){
            soundControl.PlaySound(1);
        }
        else if(verticalInput != 0 || horizontalInput != 0){
            soundControl.PlaySound(0);
        }
        else{
            soundControl.Pause();
        }
    }

    void Jump()
    {
        if(jumpInput > 0 && Grounded() && stamina > 1 && !crouched && !crouching)
        {
            stamina -= jumpStamina;
            velocity.y = jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= downAccel;
        }
    }

    public void changeHealth(int value){
        health += value;
        timeFromHit = timeToHeal;
    }
}