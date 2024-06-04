/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CodecontrolledVolume : MonoBehaviour
{
    public Volume localVol1, localVol2, localVol3;
    Bloom _bloom;
    
    // Start is called before the first frame update
    void Start()
    {
        //localVol1.profile.TryGet(out _bloom);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cyberBloom(float val)
    {
        _bloom.threshold.value = val;
    }
    public void resetBloom()
    {
        _bloom.threshold.value = 0.76f;
    }
}
*/


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;

public class CodecontrolledVolume : MonoBehaviour
{
    public TMP_Dropdown volumeDropdown;
    public TMP_Dropdown parameterDropdown;
    public TMP_InputField valueInputField;
    public Button setValueButton;
    public Button resetButton;

    private List<Volume> volumes;
    private Volume selectedVolume;
    private VolumeComponent selectedComponent;
    private VolumeParameter selectedParameter;
    private Dictionary<VolumeParameter, object> defaultValues;

    public Volume localVol1, localVol2, localVol3;

    void Start()
    {
        // Initialize volumes list
        volumes = new List<Volume> { localVol1, localVol2, localVol3 };

        // Initialize default values dictionary
        defaultValues = new Dictionary<VolumeParameter, object>();

        // Populate dropdown with volume names
        volumeDropdown.ClearOptions();
        List<string> volumeOptions = new List<string>();
        foreach (var vol in volumes)
        {
            volumeOptions.Add(vol.name);
        }
        volumeDropdown.AddOptions(volumeOptions);

        // Set up button listeners
        setValueButton.onClick.AddListener(SetValue);
        resetButton.onClick.AddListener(ResetValue);

        // Select the first volume by default
        volumeDropdown.onValueChanged.AddListener(delegate { SelectVolume(volumeDropdown.value); });
        SelectVolume(0);
    }

    void SelectVolume(int index)
    {
        selectedVolume = volumes[index];
        PopulateParameterDropdown();
    }

    void PopulateParameterDropdown()
    {
        parameterDropdown.ClearOptions();
        List<string> parameterOptions = new List<string>();

        if (selectedVolume.profile.components != null)
        {
            // for each component in volume
            foreach (var component in selectedVolume.profile.components)
            {
                //for each paramater in a given component
                foreach (var parameter in component.parameters)
                {
                    //get param name to string and type so its readable
                    string parameterName = parameter.GetType().Name + ": " + parameter.ToString();
                    parameterOptions.Add(component.GetType().Name + " - " + parameterName);
                    if (!defaultValues.ContainsKey(parameter))
                    {
                        defaultValues[parameter] = GetParameterValue(parameter);
                    }
                }
            }
        }
        parameterDropdown.AddOptions(parameterOptions);

        if (parameterOptions.Count > 0)
        {
            parameterDropdown.onValueChanged.AddListener(delegate { SelectParameter(parameterDropdown.value); });
            SelectParameter(0);
        }
    }

    object GetParameterValue(VolumeParameter parameter)
    {
        
        switch (parameter)
        {
            case FloatParameter floatParam:
                return floatParam.value;
            case IntParameter intParam:
                return intParam.value;
            case BoolParameter boolParam:
                return boolParam.value;
            case ColorParameter colorParam:
                return colorParam.value;
            // Add other parameter types as needed
            default:
                return null;
        }
    }
    void SelectParameter(int index)
    {
        string[] parts = parameterDropdown.options[index].text.Split(new[] { " - " }, System.StringSplitOptions.None);
        string componentName = parts[0];
        string parameterName = parts[1].Split(new[] { ": " }, System.StringSplitOptions.None)[1];

        selectedComponent = null;
        selectedParameter = null;

        foreach (var component in selectedVolume.profile.components)
        {
            if (component.GetType().Name == componentName)
            {
                selectedComponent = component;
                selectedParameter = component.parameters.FirstOrDefault(p => p.ToString() == parameterName);
                if (selectedParameter != null)
                {
                    valueInputField.text = GetParameterValue(selectedParameter)?.ToString();
                }
                break;
            }
        }
    }

    public void SetValue()
    {
        if (selectedComponent != null && selectedParameter != null)
        {
            string inputValue = valueInputField.text;
            switch (selectedParameter)
            {
                case FloatParameter floatParam:
                    floatParam.value = float.TryParse(inputValue, out float floatVal) ? floatVal : floatParam.value;
                    if (!float.TryParse(inputValue, out _)) Debug.LogWarning("Invalid input value.");
                    break;
                case IntParameter intParam:
                    intParam.value = int.TryParse(inputValue, out int intVal) ? intVal : intParam.value;
                    if (!int.TryParse(inputValue, out _)) Debug.LogWarning("Invalid input value.");
                    break;
                case BoolParameter boolParam:
                    boolParam.value = bool.TryParse(inputValue, out bool boolVal) ? boolVal : boolParam.value;
                    if (!bool.TryParse(inputValue, out _)) Debug.LogWarning("Invalid input value.");
                    break;
                case ColorParameter colorParam:
                    colorParam.value = ColorUtility.TryParseHtmlString(inputValue, out Color colorVal) ? colorVal : colorParam.value;
                    if (!ColorUtility.TryParseHtmlString(inputValue, out _)) Debug.LogWarning("Invalid input value.");
                    break;
                default:
                    Debug.LogWarning("Invalid parameter type.");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("No component or parameter selected.");
        }
    }
    public void ResetValue()
    {
        if (selectedComponent != null && selectedParameter != null)
        {
            if (defaultValues.ContainsKey(selectedParameter))
            {
                var defaultValue = defaultValues[selectedParameter];
                switch (selectedParameter)
                {
                    case FloatParameter floatParam:
                        floatParam.value = (float)defaultValue;
                        break;
                    case IntParameter intParam:
                        intParam.value = (int)defaultValue;
                        break;
                    case BoolParameter boolParam:
                        boolParam.value = (bool)defaultValue;
                        break;
                    case ColorParameter colorParam:
                        colorParam.value = (Color)defaultValue;
                        break;
                    default:
                        Debug.LogWarning("Cannot reset value for this parameter type.");
                        break;
                }
            }
        }
    }
}
