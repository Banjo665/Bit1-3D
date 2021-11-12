using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    float distToGround;
    Rigidbody rb;

    void Start(){
        distToGround = GetComponent<Collider>().bounds.extents.y;
        rb = GetComponent<Rigidbody>(); 
    }

    bool IsGrounded(){
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Update(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float sprintInput = Input.GetAxis("Sprint");
        float jumpInput = Input.GetAxis("Jump");
        
        float speed = movementSpeed + sprintInput * 3 * Mathf.Max(0, verticalInput);

        Vector3 rotatedForward = new Vector3(transform.forward.z, -transform.forward.y, -transform.forward.x); // 90 degree 3D vector rotation around Y
        Vector3 movementY = rotatedForward * horizontalInput * speed * Time.deltaTime;
        Vector3 movementX = transform.forward * verticalInput * speed * Time.deltaTime;
        Vector3 jump = Vector3.up * jumpInput * jumpForce * System.Convert.ToInt32(IsGrounded());

        transform.position += movementX + movementY;
        rb.AddForce(jump);
    }
}
