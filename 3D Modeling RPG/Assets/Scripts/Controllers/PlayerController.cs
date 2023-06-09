using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    public Interactable focus;

    public LayerMask movementMask;

    PlayerMotor motor;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Mouse.current.leftButton.wasPressedThisFrame)
        //if (Input.GetMouseButtonDown(0))
        {
            
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);

                //stop focusing any objects
                RemoveFocus();
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
            //if (Input.GetMouseButtonDown(1))
        {
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable =  hit.collider.GetComponent<Interactable>();

                if(interactable != null)
                {
                    SetFocus(interactable);
                }

            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if(newFocus != focus)
        {
            if(focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }

        //keep this out of the above if statement so that we can notify
        //the interactable every time we click on it
        newFocus.OnFocused(transform);
        


    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
        motor.StopFollowingTarget();
    }
}
