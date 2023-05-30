using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;
    float visibleTime = 5f;

    float lastMadeVisibleTime;

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
                ui.gameObject.SetActive(false);
                break;
            }
        }

        //subscribe to on health changed
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        //check to make sure ui is not null
        if(ui != null)
        {
            //set the healthBar ui to visible
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            //set the green part of the slider to the current health percentage
            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;

            //if character dies, make sure to destroy this game object
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //check to make sure ui is not null
        if(ui != null)
        {
            //update the position of the prefab
            ui.position = target.position;

            //align ui with main camera, set the forward to opposite of main cam
            //this way, the bar will always be facing the camera.
            ui.forward = -cam.forward;

            if(Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
        
    }

    
}
