using UnityEngine;

public class BowVisual : MonoBehaviour
{
    private Bow bow;
    private LineRendererController lineRendererController;
    private bool isShooting;

    private void Awake()
    {
        bow = GetComponentInParent<Bow>();
        bow.OnAttack += Bow_OnAttack;
        lineRendererController = GetComponentInChildren<LineRendererController>();
    }

    private void Bow_OnAttack(object sender, System.EventArgs e)
    {
        isShooting = true;
    }

    private void Update()
    {
        
        if (bow.HasTarget())
        {
            bow.SetDirection();
        }

        if(isShooting)
        {
            lineRendererController.DrawBow();
        }
    }

    public void OnFalseIsShooting()
    {
        isShooting = false;
    }
}
