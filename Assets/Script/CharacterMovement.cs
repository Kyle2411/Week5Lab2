using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public Vector3 gravity;
    public Vector3 playerVelocity;
    public bool groundedPlayer;
    public float mouseSensitivy = 5.0f;
    private float jumpHeight = 1f;
    private float gravityValue = -9.81f;
    private CharacterController controller;
    private float walkSpeed = 5;
    private float runSpeed = 8; 
    public float bulletSpeed = 10f;
public float maxDistance = 100f;
    AudioSource audio;
     private GameManager manage;

 
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        manage = GetComponent<GameManager>();
        //Ray ray = Camera.mainViewPointToRay(new Vector3 (0.5f, 0.5f, 0));
        
    }

    public void Update()
    {
       UpdateRotation();
       ProcessMovement();
       shootGun();
       
      

    
    }

    
    void UpdateRotation()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X")* mouseSensitivy, 0, Space.Self);
         
    }

    void shootGun()
    {
        if (Input.GetButton("Fire1"))// Left shift
        {
                audio.Play();

        Camera camera = Camera.main;
        Vector3 cameraCenter = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)); // Get the center of the camera viewport in world space
        Ray ray = new Ray(cameraCenter, camera.transform.forward); // Create a new ray that starts from the center of the camera and goes in the direction that the camera is facing
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance)) {
            
            if (hit.collider.CompareTag("Target")) {
                
                manage.IncrementScore();
            }
        }
        }
    }
    

    void ProcessMovement()
    { 
        // Moving the character foward according to the speed
        float speed = GetMovementSpeed();

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Making sure we dont have a Y velocity if we are grounded
        // controller.isGrounded tells you if a character is grounded ( IE Touches the ground)
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            if (Input.GetButtonDown("Jump") )
            {
                gravity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            else 
            {
                // Dont apply gravity if grounded and not jumping
                gravity.y = -1.0f;
            }
        }
        else 
        {
            // Since there is no physics applied on character controller we have this applies to reapply gravity
            gravity.y += gravityValue * Time.deltaTime;
        }  
        Vector3 movement = move.z *transform.forward  + move.x * transform.right;
        playerVelocity = gravity * Time.deltaTime + movement * Time.deltaTime * speed;
        controller.Move(playerVelocity);
    }

    float GetMovementSpeed()
    {
        if (Input.GetButton("Fire3"))// Left shift
        {
            return runSpeed;
        }
        else
        {
            return walkSpeed;
        }
    }


}
