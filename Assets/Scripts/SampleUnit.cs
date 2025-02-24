using TbsFramework.Units;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleUnit : Unit
{
    //this code is borrowed from the tutorial in the documentation
    public Color LeadingColor;
    public Vector3 Offset;
    public override void Initialize()
    {
        base.Initialize();
        GetComponentInChildren<Renderer>().material.color = LeadingColor;
        transform.localPosition += Offset;
    }

    public override void MarkAsFriendly()
    {
        GetComponentInChildren<Renderer>().material.color  = new Color (0.8f,1,0.8f);
    }

    public override void MarkAsReachableEnemy()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    public override void MarkAsSelected()
    {
        GetComponentInChildren<Renderer>().material.color = Color.green;
    }

    public override void MarkAsFinished()
    {
        GetComponentInChildren<Renderer>().material.color = Color.gray;
    }

    public override void UnMark()
    {
        GetComponentInChildren<Renderer>().material.color = LeadingColor;
    }

    public void OnMouseOver() {
        Debug.Log("mouse over!!");
    }

}
