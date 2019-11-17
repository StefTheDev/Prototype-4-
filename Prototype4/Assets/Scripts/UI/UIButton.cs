using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        animator.SetBool("Open", false);
    }
}
