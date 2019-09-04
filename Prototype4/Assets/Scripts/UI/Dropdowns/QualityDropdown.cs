using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityDropdown : UIDropdown<string>
{
    private void Start()
    {
        Initialise(new List<string>(QualitySettings.names));
    }


    public override void SelectOption()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
    }
}