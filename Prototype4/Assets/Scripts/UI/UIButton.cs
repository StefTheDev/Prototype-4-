using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Animator animator;
#pragma warning restore CS0649

    void Start()
    {
        animator.SetBool("Open", false);
    }
}
