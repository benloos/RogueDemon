using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
 
    public CharacterController controller;
    [SerializeField] private GameObject PlayerBody;
    [SerializeField] private float speed = 12f;
    [SerializeField] private CapsuleCollider groundCheck;
    [SerializeField] private float gravity = -12f; //-9.81
    public float groundDistance = 0.4f;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpforce = 10f;
    private float orig_height;
    private float crouch_height;

    void Start(){
        orig_height = controller.height;
        crouch_height = controller.height/2;
    }

    // Update is called once per frame
    void Update(){
        // Input for W A S D
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //movement direction Vektor
        Vector3 move = transform.right * x + transform.forward * z;

        // move player in direction of Vector
        controller.Move(move * speed * Time.deltaTime);

        // add gravity force
        velocity.y += gravity * Time.deltaTime;
        
        /**
        When the player is on the ground no gravity is applied to it 
        This is so we can build up velocity when we step off an edge and not plunge to the ground at full speed
        While not on the ground gravity is added with each frame to increase the speed at which we are falling
         */
        if(IsGrounded()){
            if(Input.GetKey(KeyCode.Space)){
                // give upward impulse
                velocity.y = jumpforce;
            }
            velocity.y = Mathf.Clamp(velocity.y, 0, jumpforce);
        }else{
            velocity.y = Mathf.Clamp(velocity.y, gravity, jumpforce);
        }
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetKey(KeyCode.LeftControl)){
            //transform.localScale.Set() = Mathf.Lerp(transform.localScale.y,  1f, 5*Time.deltaTime);
        }else{
            controller.height = orig_height;
        }
       
    }

    // check for floor or other object beneath the player with Raycast
    private bool IsGrounded(){
        
        bool groundhit = Physics.Raycast(groundCheck.bounds.center, Vector3.down, groundCheck.bounds.extents.y + 0.1f);
        
        if(groundhit){
            return true;
        }
        return false;
    }
}

