using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    AudioSource death_sound;
    [SerializeField] private float speed = 12f;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float gravity = -12f; //-9.81
    [SerializeField] private GameObject standard_weapon;
    [SerializeField] private GameObject special_weapon;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Camera camera;
    [SerializeField] private Image blackImage;
    [SerializeField] private Image DeathText;
    private shoot_wumms sniper_script;
    private GameObject weapon;
    private Vector3 standard_weaponOrigin;
    private Vector3 special_weaponOrigin;
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
    private bool SniperEnabled;
    private int sniper_ammo;

    public AudioClip deathClip;
    public AudioClip weapon_swap_clip;

    // Player Stats
    public int maxHP = 100;
    public int HP = 100;
    public int DMG = 10;
    public int Firerate = 2;

    void Start(){
        weapon = standard_weapon;
        weapon.SetActive(true);
        orig_height = controller.height;
        crouch_height = controller.height/2;
        standard_weaponOrigin = standard_weapon.transform.localPosition;
        special_weaponOrigin = special_weapon.transform.localPosition;
        weaponOrigin = weapon.transform.localPosition;
        canDash = true;
        dashCD = 1.5f;
        playerCollider = GetComponent<CapsuleCollider>();
        death_sound = GetComponent<AudioSource>();
        death_sound.clip = deathClip;
        SniperEnabled = false;
        sniper_script = special_weapon.GetComponent<shoot_wumms>();
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
        
    
        
        
        
        if(sniper_script.ammo > 0){
            SniperEnabled = true;
        } else {
            SniperEnabled = false;
        }
        /**
        WAFFEN WECHSEL
        */
        if((Input.mouseScrollDelta.y != 0.0) && SniperEnabled == true){
            change_weapon();
        }
        /**
        FORCED WECHSEL WEIL KEINE AMMO
        */
        if(weapon.Equals(special_weapon) && sniper_script.ammo == 0){
            SniperEnabled = false;
            change_weapon();
        }
    
        
        //Debug.Log("Weapon Origin: (" + weaponOrigin.x + ", " + weaponOrigin.y + ", " + weaponOrigin.z + ")");
        //HeadBob
        if(IsGrounded()){
            if(x == 0 && z == 0){
                HeadBob(idleCounter, 0.0125f, 0.0125f);
                idleCounter += Time.deltaTime;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);   
            } else if(!sprinting){
                HeadBob(movementCounter, 0.0175f, 0.0175f);
                movementCounter += Time.deltaTime * 3.5f;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
            } else {
                HeadBob(movementCounter, 0.0225f, 0.0225f);
                movementCounter += Time.deltaTime * 8f;
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
            }
        }      

        
    }


    private void OnParticleCollision(GameObject other)
    {
        Damage(20);
    }
    // check for floor or other object beneath the player
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
        StartCoroutine(Shake(0.15f, 0.15f));
        HP -= dmg;
        if (HP < 1)
        {
            HP = 0;
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
        Time.timeScale = 0.25f;
        death_sound.clip = deathClip;
        death_sound.Play();
        LeanTween.alpha(blackImage.rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeInOutCubic);
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
        camera.fieldOfView = 75f;
        canDash = false;
        isDashing = true;
        DashStartTime = Time.time;
    }

    void onEndDash(){
        camera.fieldOfView = 70f;
        isDashing = false;
        DashStartTime= 0;
    }

    void ResetDash()
    {
        canDash = true;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude + 1;

            camera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        camera.transform.localPosition = originalPos;
    }

    private void change_weapon(){
        //put away location
        Vector3 put_away = new Vector3(-0.3f,0.3f,0);
        put_away = weaponOrigin-put_away;
        weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, put_away, 0.7f);
        weapon.SetActive(false);
        if(weapon.Equals(standard_weapon)){
            weapon = special_weapon;
            weaponOrigin = special_weaponOrigin;
        } else{
            weapon = standard_weapon;
            weaponOrigin = standard_weaponOrigin;
        }
        weapon.transform.localPosition = put_away;
        weapon.SetActive(true);
        death_sound.clip = weapon_swap_clip;
        death_sound.Play();
        weapon.transform.localPosition = Vector3.Lerp(put_away, weaponOrigin, 0.7f);
        
        
    }
}

