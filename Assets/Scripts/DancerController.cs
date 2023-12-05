using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancerController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    private bool spaceKey = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Inputs();

        ChangeAnimation();
    }

    private void Inputs()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !spaceKey)
        //{
        //    spaceKey = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.Space) && spaceKey)
        //{
        //    spaceKey = false;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceKey = !spaceKey;
        }
    }

    private void ChangeAnimation()
    {
        animator.SetBool("IsReady", spaceKey);
    }
}