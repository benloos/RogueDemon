using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float gravity = -12f; //-9.81
    [SerializeField] private GameObject weapon;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Camera camera;
    [SerializeField] private Image DeathImage;
    [SerializeField] private Image DeathText;
    private CapsuleCollider playerCollider;
    private Vector3 move;
    private float orig_height;
    private float crouch_height;
    private bool sprinting = false;
    private Vector3 weaponOrigin;
    private float movementCounter;
    private float idleCounter;
    private Vector3 targetWeaponBobPosition;
    private float DashStartTime;
    public bool isDashing;
    private bool canDash;
    public float dashCD;

    // Player Stats
    public int maxHP = 100;
    public int HP = 100;
    public int DMG = 10;
    public int Firerate = 2;

    void Start(){
        orig_height = controller.height;
        crouch_height = controller.height/2;
        weaponOrigin = weapon.transform.localPosition;
        canDash = true;
        dashCD = 1.5f;
        playerCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update(){

        // Input for W A S D
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //movement direction Vektor
        move = transform.right * x + transform.forward * z;

        HandleDash();

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
        Sprinten LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL
        */
        /*
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            speed = 18f;
            sprinting = true;
        } else if(Input.GetKeyUp(KeyCode.LeftShift)){
            speed = 12f;
            sprinting = false;
        }
        */
        
        //Debug.Log("Weapon Origin: (" + weaponOrigin.x + ", " + weaponOrigin.y + ", " + weaponOrigin.z + ")");
        //HeadBob
        if(IsGrounded()){
            if(x == 0 && z == 0){
                HeadBob(idleCounter, 0.025f, 0.025f);
                idleCounter += Time.deltaTime;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);   
            } else if(!sprinting){
                HeadBob(movementCounter, 0.035f, 0.035f);
                movementCounter += Time.deltaTime * 3.5f;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
            } else {
                HeadBob(movementCounter, 0.045f, 0.045f);
                movementCounter += Time.deltaTime * 8f;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
            }
        }      

        
    }

    // check for floor or other object beneath the player with Raycast
    private bool IsGrounded(){
        Collider[] groundhit = Physics.OverlapSphere(groundCheck.transform.position, 0.3f); //Physics.Raycast(groundCheck.bounds.center, Vector3.down, groundCheck.bounds.extents.y + 0.1f);
        foreach (Collider col in groundhit)
        {
            if (col.gameObject.layer != 7 && !col.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    private void HeadBob (float z, float x_intensity, float y_intensity){
        targetWeaponBobPosition = weaponOrigin + new Vector3(Mathf.Cos(z) * x_intensity, Mathf.Sin(z * 2) * y_intensity, 0);
    }

    public void Damage(int dmg)
    {

        HP -= dmg;
        if (HP < 1)
        {
            HP = 0;
            Time.timeScale = 0.25f;
            StartCoroutine(DeathSequence());
        }
    }

    public void Heal(int hp)
    {
        HP += hp;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
    }

    IEnumerator DeathSequence()
    {
        LeanTween.alpha(DeathImage.rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.alpha(DeathText.rectTransform, 1f, 1f).setEase(LeanTweenType.easeInCubic);
        yield return new WaitForSeconds(1.75f);
        GameManager.current.LoadMenuScene();
    }

    void HandleDash(){
        
        bool isTryingToDash = Input.GetKeyDown(KeyCode.LeftShift);

        if (canDash && isTryingToDash && !isDashing)
        {
            onStartDash();
            Invoke(nameof(ResetDash), dashCD);
        }

        if (isDashing)
        {
            if (Time.time - DashStartTime <= 0.2f)
            {
                if (move.Equals(Vector3.zero))
                {
                    controller.Move(transform.forward * 30f * Time.deltaTime);
                }
                else
                {
                    controller.Move(move.normalized * 30f * Time.deltaTime);
                }
            }
            else
            {
                onEndDash();
            }
        }
    }

    void onStartDash(){
        canDash = false;
        isDashing = true;
        DashStartTime = Time.time;
    }

    void onEndDash(){
        isDashing = false;
        DashStartTime= 0;
    }

    void ResetDash()
    {
        canDash = true;
    }
}

