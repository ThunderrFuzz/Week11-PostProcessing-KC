using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class WeatherSystem : MonoBehaviour
{
    [Header("Global")]
    public Material globalMaterial;
    public Light sunLight;
    public Material skyboxMaterial;

    [Header("Winter Assets")]
    public ParticleSystem winterParticleSystem;
    public Volume winterVolume;
    public GameObject snowVFX;
    public Color wintercolor ;

    [Header("Rain Assets")]
    public ParticleSystem rainParticleSystem;
    public Volume rainVolume;
    public Color raincolor;
    [Header("Autumn Assets")]
    public ParticleSystem autumnParticleSystem;
    public Volume autumnVolume;
    public Color autumncolor;

    [Header("Summer Assets")]
    public ParticleSystem summerParticleSystem;
    public Volume summerVolume;
    public Color summercolor;

    [Header("Other")]
    public GameObject camera;
    public VisualEffect[] treeLeafFall;
    private void Start()
    {
        foreach(VisualEffect fall in treeLeafFall)
        {
            fall.gameObject.SetActive(false);
        }
        snowVFX.SetActive(false);

    }
    private void Update()
    {
        //snowVFX.transform.position = camera.transform.position;
    }

    public void disableOtherEffects(Volume safe)
    {
        if(safe == winterVolume)
        {
            rainParticleSystem.Stop();
            snowVFX.gameObject.SetActive(false);
            foreach (VisualEffect fall in treeLeafFall)
            {
                fall.gameObject.SetActive(false);
            }
            rainVolume.gameObject.SetActive(false);
            autumnVolume.gameObject.SetActive(false);
            summerVolume.gameObject.SetActive(false);
        }
        else if(safe == autumnVolume)
        {
            rainParticleSystem.Stop();
            snowVFX.gameObject.SetActive(false);

            winterVolume.gameObject.SetActive(false);
            rainVolume.gameObject.SetActive(false);
            summerVolume.gameObject.SetActive(false);
        }
        else if (safe == summerVolume) {
            rainParticleSystem.Stop();
            snowVFX.gameObject.SetActive(false);

            foreach (VisualEffect fall in treeLeafFall)
            {
                fall.gameObject.SetActive(false);
            }

            winterVolume.gameObject.SetActive(false);
            rainVolume.gameObject.SetActive(false);
            autumnVolume.gameObject.SetActive(false);
            
        }
        else if ( safe == rainVolume)
        {

            foreach (VisualEffect fall in treeLeafFall)
            {
                fall.gameObject.SetActive(false);
            }

            snowVFX.gameObject.SetActive(false);
            winterVolume.gameObject.SetActive(false);
            autumnVolume.gameObject.SetActive(false);
            summerVolume.gameObject.SetActive(false);
        }
       
    }
    public void Winter()
    {
        disableOtherEffects(winterVolume);

        globalMaterial.SetFloat("_SnowFade", 1f);
        globalMaterial.SetFloat("_Metallic", .5f);
        globalMaterial.SetFloat("_Smoothness", .4f);
        globalMaterial.SetColor("_SnowColor", wintercolor);
   


        winterVolume.gameObject.SetActive(true);
        snowVFX.gameObject.SetActive(true);
    }

    public void Rain()
    {
        disableOtherEffects(rainVolume);

        globalMaterial.SetFloat("_SnowFade", .2f);
        globalMaterial.SetFloat("_Metallic", .3f);
        globalMaterial.SetFloat("_Smoothness", .3f);
        globalMaterial.SetColor("_SnowColor", raincolor);


        rainParticleSystem.Play(); 
        rainVolume.gameObject.SetActive(true);
    }

    public void Autumn()
    {


        globalMaterial.SetFloat("_SnowFade", .4f);
        globalMaterial.SetFloat("_Metallic", .1f);
        globalMaterial.SetFloat("_Smoothness", .25f);
        globalMaterial.SetColor("_SnowColor", autumncolor);
        foreach (VisualEffect fall in treeLeafFall)
        {
            fall.gameObject.SetActive(true);
        }
        disableOtherEffects(autumnVolume);
        autumnVolume.gameObject.SetActive(true);
    }

    public void Summer()
    {
        globalMaterial.SetFloat("_SnowFade", 0f);
        globalMaterial.SetFloat("_Metallic", .1f);
        globalMaterial.SetFloat("_Smoothness", .25f);
        globalMaterial.SetColor("_SnowColor", summercolor);

        disableOtherEffects(summerVolume);
        summerVolume.gameObject.SetActive(true);
    }
}
