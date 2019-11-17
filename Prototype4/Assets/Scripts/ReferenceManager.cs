using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReferenceManager : MonoBehaviour
{
    // Singleton
    private static ReferenceManager instance;

    public static ReferenceManager Instance { get { return instance; } }

    public GameObject[] humanPlayerPrefabs;
    public GameObject[] AIPlayerPrefabs;
    public GameObject playerManagerPrefab;
    public GameObject[] fireParticlePrefabs;

    public Material[] playerMaterials;

    public GameObject promptParent;

    public GameObject muzzleFlashParticle;
    public GameObject airBlastCollisionParticle;
    public GameObject[] respawnParticle;
    public GameObject logPrefab;
    public GameObject lightningStrike;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    private GameObject[] humanPrompts;
    private GameObject[] aiPrompts;

    private GameObject[] activatedPrompts = new GameObject[4];
   
    private GridLayout gridLayout;

    private void Start()
    {
        humanPrompts = Resources.LoadAll<GameObject>("Human Prompts");
        aiPrompts = Resources.LoadAll<GameObject>("AI Prompts");

        for (int i = 0; i < 4; i++)
        {
            ActivateAI(i);
        }
    }

    public void ActivateHuman(int index)
    {
        if (activatedPrompts[index] != null)
        {
            Destroy(activatedPrompts[index]);
            Debug.Log("Human Destroyed");
        }

        activatedPrompts[index] = Instantiate(humanPrompts[index], promptParent.transform);
        activatedPrompts[index].GetComponent<PromptReferences>().scoreText.text = GameManager.Instance.playerManagers[index].normalKills.ToString();
        // activatedPrompts[index].GetComponentInChildren<TMP_Text>().text = "Human";

        activatedPrompts[index].transform.SetSiblingIndex(index);
    }

    public void ActivateAI(int index)
    {
        if (activatedPrompts[index] != null)
        {
            Destroy(activatedPrompts[index]);
            Debug.Log("AI Destroyed");
        }
        activatedPrompts[index] = Instantiate(aiPrompts[index], promptParent.transform);
        activatedPrompts[index].GetComponent<PromptReferences>().scoreText.text = GameManager.Instance.playerManagers[index].normalKills.ToString();
        // activatedPrompts[index].GetComponentInChildren<TMP_Text>().text = "AI";
        activatedPrompts[index].transform.SetSiblingIndex(index);
    }

    public GameObject GetPlayerPrompt(int index)
    {
        return activatedPrompts[index];
    }
}
