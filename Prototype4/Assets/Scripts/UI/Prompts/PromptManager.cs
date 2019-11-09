using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    private static PromptManager instance;

    public static PromptManager Instance { get { return instance; } }

    private Prompt[] prompts;

    
    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    void Refresh()
    {

    }

    public Prompt[] GetPrompts()
    {
        return prompts;
    }
}
