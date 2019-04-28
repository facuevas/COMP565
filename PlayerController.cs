using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private enum PlayerState
    {
        Normal,
        Rolling
    };

    private PlayerState state;

    //public handling variables
    public float rotationSpeed = 1000;
    public float walkSpeed = 5;
    public float runSpeed = 8;
    public float evadeSpeed = 300;
    public int dodgeRollCost = 25;
    private float currentEvadeSpeed;

    //System variables
    private Quaternion targetRotation;

    //Component variables
    private CharacterController controller;
    private Camera cam;
    public Gun gun;
    public PlayerStats playerStats;

    //input variables
    public bool useController;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        cam = Camera.main;
        state = PlayerState.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Normal)
        {
            if (useController)
            {
                GamepadControl();
            }
            else
            {
                NewControlMouse();
            }

            HandleDodgeRoll();

            if (Input.GetKeyDown(KeyCode.X))
                playerStats.TakeDamage(10);

            if (Input.GetKeyDown(KeyCode.E))
                playerStats.UseEnergy(10);
        }

        if (state == PlayerState.Rolling)
        {
            DodgeRoll();
        }

        Debug.Log("Health value from player: " + playerStats.GetCurrentHealth());
    }

    //use this for current gamepad controller movements
    void GamepadControl()
    {
        //left stick movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1f;
        motion *= (Input.GetButton("Run") ? runSpeed : walkSpeed);

        //right stick movement
        Vector3 horizontalDirection = Vector3.right * Input.GetAxisRaw("RHorizontal");
        Vector3 verticalDirection = Vector3.forward * -Input.GetAxisRaw("RVertical");
        Vector3 playerDirection = horizontalDirection + verticalDirection;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }

        //fire function to shoot
        ControllerPrimaryFire();

        //translate movements
        controller.Move(motion * Time.deltaTime);
    }

    //use this for current WASD and mouse movement
    void NewControlMouse()
    {
        //Keyboard movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1f;
        motion *= (Input.GetButton("Run") ? runSpeed : walkSpeed);

        //Mouse movement
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //Fire function to shoot
        PrimaryFire();

        //translate movements
        controller.Move(motion * Time.deltaTime);
    }

    //outdated. kept for legacy support
    void ControlMouse()
    {
        //Old mouse movement
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 motion = input;

        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1f;
        motion *= (Input.GetButton("Run") ? runSpeed : walkSpeed);
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            gun.Shoot();
        }
        else if (Input.GetButton("Fire1"))
        {
            gun.ShootContinuous();
        }
    }

    //outdated. kept for legacy support
    void ControlWASD()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1f;
        motion *= (Input.GetButton("Run") ? runSpeed : walkSpeed);
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);
    }

    //These two methods below are for primary fire
    void ControllerPrimaryFire()
    {
        float fire1 = Input.GetAxis("ControllerFire1");
        if (fire1 != 0)
        {
            gun.Shoot();
        }
    }

    void PrimaryFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gun.Shoot();
        }
        else if (Input.GetButton("Fire1"))
        {
            gun.ShootContinuous();
        }
    }

    //methods under this comment are for dodge roll mechanic
    private void HandleDodgeRoll()
    {
        if (Input.GetButtonDown("Roll") && playerStats.GetCurrenyEnergy() >= 25)
        {
            Debug.Log("Roll button pressed");
            state = PlayerState.Rolling;
            playerStats.UseEnergy(dodgeRollCost);
        }
        currentEvadeSpeed = evadeSpeed;
    }

    //very broken. semi-works
    //felix said he would look into it
    private void DodgeRoll()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 motion = input;
        motion *= currentEvadeSpeed;
        controller.Move(motion * Time.deltaTime);
        currentEvadeSpeed -= currentEvadeSpeed * Time.deltaTime;
        Debug.Log(currentEvadeSpeed + "fu");

        //edit this value to change nthe distance of the move.
        if (currentEvadeSpeed < evadeSpeed)
        {
            state = PlayerState.Normal;
        }
    }

    private void crap()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            //goes left
        }

        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            //goes right
        }

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            //goes up
        }

        if (Input.GetAxisRaw("Vertical") == -1)
        {
            //goes down
        }
    }
}
