using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{

    public enum Controller
    {
        Xbox,
        SwitchPro,
        Playstation
    };

    //handlers
    //public bool useController;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectControlMethod();
    }

    //function detects if the player is using a keyboard or a controller and updates the boolean value to determine what input to be using.
    void DetectControlMethod()
    {
        //Detect WASD and mouse
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            player.useController = false;

        if (Input.GetAxisRaw("Mouse X") != 0.0f || Input.GetAxisRaw("Mouse Y") != 0.0f)
            player.useController = false;

        //Detect Controller Input
        if (Input.GetAxisRaw("RHorizontal") != 0.0f || Input.GetAxisRaw("RVertical") != 0.0f || Input.GetAxis("ControllerFire1") != 0.0f)
            player.useController = true;

        if (Input.GetKey(KeyCode.JoystickButton0) ||
            Input.GetKey(KeyCode.JoystickButton1) ||
            Input.GetKey(KeyCode.JoystickButton2) ||
            Input.GetKey(KeyCode.JoystickButton3) ||
            Input.GetKey(KeyCode.JoystickButton4) ||
            Input.GetKey(KeyCode.JoystickButton5) ||
            Input.GetKey(KeyCode.JoystickButton6) ||
            Input.GetKey(KeyCode.JoystickButton7) ||
            Input.GetKey(KeyCode.JoystickButton8) ||
            Input.GetKey(KeyCode.JoystickButton9) ||
            Input.GetKey(KeyCode.JoystickButton10))
        {
            player.useController = true;
        }
    }
}
