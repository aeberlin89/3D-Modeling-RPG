using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;

    Transform ui;
    Image healthSlider;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;

        //search for world space canvas
        foreach(Canvas c in FindObjectsOfType<Canvas>())
        {
            
            if(c.renderMode == RenderMode.WorldSpace)
            {
                //instantiate the health bar and save it's transform
                ui = Instantiate(uiPrefab, c.transform).transform;


                //since we know the green part of the ui slider or 'health' part
                //is the first child of the prefab transform, we can access it
                //and get it's image component and save it to healthslider
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                break;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //update the position of the prefab
        ui.position = target.position;

        //align ui with main camera, set the forward to opposite of main cam
        //this way, the bar will always be facing the camera.
        ui.forward = -cam.forward;


    }
}
