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
    [SerializeField] private GameObject weapon;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpforce = 10f;
    public float groundDistance = 0.4f;
    private float orig_height;
    private float crouch_height;
    private float normal_extend = 0.1f;
    private float crouch_extend = 1.6f;
    private Vector3 weaponOrigin;
    private float movementCounter;
    private float idleCounter;

    void Start(){
        orig_height = controller.height;
        crouch_height = controller.height/2;
        weaponOrigin = weapon.transform.localPosition;
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
            if(Input.GetKeyDown(KeyCode.Space)){
                // give upward impulse
                velocity.y = jumpforce;
            }
            velocity.y = Mathf.Clamp(velocity.y, 0, jumpforce);
        }else{
            velocity.y = Mathf.Clamp(velocity.y, gravity, jumpforce);
        }
        controller.Move(velocity * Time.deltaTime);
        

        /**
        wenn LeftControl gehalten wird wird die hitbox des Spielers kleiner um crouchen zu erzeugen
        wird LeftControl wieder losgelassen wird die spielfigur auf die ursprüngliche höhe des charakters + 0.1
        !!! damit der bums hier richtig funktioniert muss man sich beim crouchen bewegen sonst bleibt der im boden stecken... !!!
        */
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            controller.height = crouch_height;
            groundCheck.height = crouch_height;
        }else if(Input.GetKeyUp(KeyCode.LeftControl)){
            Vector3 temp = transform.position;
            temp.y = 0.1f; 
            transform.position = temp;
            groundCheck.height = orig_height;
            controller.height = orig_height;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            speed = 18f;
        } else if(Input.GetKeyUp(KeyCode.LeftShift)){
            speed = 12f;
        }
        
        //Debug.Log("Weapon Origin: (" + weaponOrigin.x + ", " + weaponOrigin.y + ", " + weaponOrigin.z + ")");
        //HeadBob
        if(x == 0 && z == 0){
            HeadBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            //Debug.Log("we are standing still");
        } else {
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 3.5f;
            //Debug.Log("moveing");
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

    private void HeadBob (float z, float x_intensity, float y_intensity){
        weapon.transform.localPosition = weaponOrigin + new Vector3(Mathf.Cos(z) * x_intensity, Mathf.Sin(z * 2) * y_intensity, 0);
    }
}

