using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class UIDropdown<T> : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private List<T> list;

    private void Update()
    {
        SelectOption();
    }

    public void Initialise(List<T> list)
    {
        this.list = list;
        List<string> strings = new List<string>();

        list.ForEach(s => strings.Add(s.ToString()));

        dropdown.AddOptions(strings);
        dropdown.RefreshShownValue();
    }

    public T GetElement(int i)
    {
        return list[i];
    }

    public abstract void SelectOption();
}
