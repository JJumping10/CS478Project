using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightScript : MonoBehaviour
{
    public Light2D spotLight, globalLight;
    public float fade, fadeMax, diminishRate, timer;
    public GameObject player, weepingCapsule;
    public WeepingCapsuleScript weepingCapsuleScript;
    public bool isFrozen, introFinished;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spotLight = GetComponent<Light2D>();
        player = GameObject.Find("Player");
        weepingCapsule = GameObject.Find("WeepingCapsule");
        weepingCapsuleScript = weepingCapsule.GetComponent<WeepingCapsuleScript>();
        weepingCapsuleScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        intro();//Runs when the level starts
        timer+=Time.deltaTime;//For intro function
        if(Input.GetMouseButtonDown(0)) fade = fadeMax;//Set the light fade over time
        if(fade > 0) fade -= Time.deltaTime * diminishRate;//Dims light over time
        Setlight();
        if(introFinished) LightFreeze();
    }

    void Setlight()//Add a fading effect that decreases to default smoothly
    {
        spotLight.intensity = fade;//Brightness of light
        spotLight.pointLightOuterRadius = fade;//Size of light
    }

    void LightFreeze()//Freeze enemy in place when light is shining on enemy
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, fade);
        foreach(var h in hitColliders)//Checks for any and all collisions
        {
            if(h.gameObject.name == "WeepingCapsule") { isFrozen = true; break; } else { isFrozen = false; continue; }//Checks if the enemy in light range
        }
        if(isFrozen) weepingCapsuleScript.enabled = false; else weepingCapsuleScript.enabled = true;//Freeze when in light range, otherwise move enemy
    }


    void intro()//Attempt to change light type
    {
        float max = 1;
        float min = 0;
        if(timer >= 0 && timer <= 1){//See all
            globalLight.enabled = true;
            spotLight.enabled = false;
        } else if(timer >= 1 && timer < 1.5f){//See little
            max -= Time.deltaTime;
            globalLight.intensity = max; 
        } else if(timer >= 1.5f && timer < 3){//See all
            min += Time.deltaTime;
            if(min<1) globalLight.intensity = min;
        } else{//See little
            globalLight.enabled = false;
            spotLight.enabled = true;
            weepingCapsuleScript.enabled = true;//Let the enemy move
            introFinished = true;//Start the rest of the light of the level
        }
    }
}
