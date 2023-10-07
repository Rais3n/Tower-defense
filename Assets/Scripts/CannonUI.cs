using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUI : MonoBehaviour
{



    private Animator animator;
    private string CANNON_SHOOTING = "CannonShooting";
    private Cannon cannon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cannon = GetComponentInParent<Cannon>();

    }

    private void Start()
    {
        cannon.OnAttack += Cannon_OnAttack;
    }


    private void Cannon_OnAttack(object sender, System.EventArgs e)
    {
        AnimationStart();
    }

    private void Update()
    {
        if (cannon.HasTarget()) {
            cannon.SetDirection();
        }
    }

    public void AnimationStart()
    {
        animator.Play(CANNON_SHOOTING);
    }
}
