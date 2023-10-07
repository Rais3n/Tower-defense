using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUnhideButton : MonoBehaviour
{
    [SerializeField] private RectTransform image1;
    [SerializeField] private RectTransform image2;
    private Animator animator;
    

    private bool isHidden;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        animator.SetBool("isHidden", isHidden);
        Invoke("RotateSprites", .16f);
        ChangeBool();
    }

    private void RotateSprites()
    {
        image1.Rotate(Vector3.forward, 180f);
        image2.Rotate(Vector3.forward, 180f);
    }
    private void ChangeBool()
    {
        isHidden = !isHidden;
    }
}
