using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionDropdown : UIDropdown<Resolution>
{
    private void Start()
    {
        Initialise(new List<Resolution>(Screen.resolutions));
    }


    public override void SelectOption()
    {
        Resolution resolution = GetElement(dropdown.value);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }
}