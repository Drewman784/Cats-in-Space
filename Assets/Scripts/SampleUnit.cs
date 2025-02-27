using TbsFramework.Units;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleUnit : Unit
{
    //this code is borrowed from the tutorial in the documentation
    public Color LeadingColor;
    public Vector3 Offset;
    Color friendlyCustomColor;
    public override void Initialize()
    {
        base.Initialize();
        GetComponentInChildren<Renderer>().material.color = LeadingColor;
        transform.localPosition += Offset;
        friendlyCustomColor = LeadingColor + new Color(0f,0f,0f,0.5f);
    }

    public override void MarkAsFriendly()
    {
        //Debug.Log("Mark");
        GetComponentInChildren<Renderer>().material.color  = friendlyCustomColor;
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
        //Debug.Log("Unmark");
        GetComponentInChildren<Renderer>().material.color = LeadingColor;
    }

    public void OnMouseOver() {
        //Debug.Log("mouse over!!");
    }

}
