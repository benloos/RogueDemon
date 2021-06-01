using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
 
    public CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private CapsuleCollider groundCheck;
    [SerializeField] private float gravity = -12f; //-9.81
    public float groundDistance = 0.4f;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpforce = 10f;

    // Update is called once per frame
    void Update(){

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        
        if(IsGrounded()){
            if(Input.GetKey(KeyCode.Space)){
                velocity.y = jumpforce;
            }
            velocity.y = Mathf.Clamp(velocity.y, 0, jumpforce);
        }else{
            velocity.y = Mathf.Clamp(velocity.y, gravity, jumpforce);
        }
        controller.Move(velocity * Time.deltaTime);
    }
    private bool IsGrounded(){
        bool groundhit = Physics.Raycast(groundCheck.bounds.center, Vector3.down, groundCheck.bounds.extents.y + 0.1f);
        
        if(groundhit){
            return true;
        }
        return false;
    }
}

