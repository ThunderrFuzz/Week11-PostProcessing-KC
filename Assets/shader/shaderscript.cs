using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shaderscript : MonoBehaviour
{
    [SerializeField] MeshRenderer flagmesh;
    [SerializeField] Slider windspd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //flagmesh.material.SetFloat("_Frequency", windspd.value);
        if(windspd.value < 5)
        {
            //drop Y pos at end
        }
    }
    public void updateFlag()
    {
        flagmesh.material.SetFloat("_Frequency", windspd.value);
    }
}
