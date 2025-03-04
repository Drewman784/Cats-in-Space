using Microsoft.Unity.VisualStudio.Editor;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SampleUnit : Unit
{
    //this code is borrowed from the tutorial in the documentation
    public Color LeadingColor;
    public Vector3 Offset;
    public string UnitName;
    public bool isStructure;
    Color friendlyCustomColor;

    private GameObject healthBar;
    private GameObject actionMarker;

    public override void Initialize()
    {
        base.Initialize();
        GetComponentInChildren<Renderer>().material.color = LeadingColor;
        transform.localPosition += Offset;
        friendlyCustomColor = LeadingColor + new Color(0f,0f,0f,0.5f);
        healthBar = transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).gameObject;
        actionMarker = transform.GetChild(2).transform.GetChild(1).gameObject;
        if(this.PlayerNumber!=0){
            UnityEngine.UI.Image hBar = healthBar.GetComponent<UnityEngine.UI.Image>();
            hBar.color = Color.red; 
            actionMarker.SetActive(false);
        }
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
        actionMarker.SetActive(false);
    }

    public override void UnMark()
    {
        //Debug.Log("Unmark");
        if(this.HitPoints>0){
            GetComponentInChildren<Renderer>().material.color = LeadingColor;
            if(this.PlayerNumber==0){
                actionMarker.SetActive(true);
            }
        }
    }

    public void ShowHealth(){ //updates healthbar
        float barHealth = (float)this.HitPoints/(float)this.TotalHitPoints;
        healthBar.GetComponent<Transform>().localScale = new Vector3(barHealth,1,1);
        Debug.Log("hp:" + this.HitPoints + "%"+ this.TotalHitPoints+ " -> "+(barHealth));
    }


    protected override void DefenceActionPerformed(){ //runs after unit has been attacked
        ShowHealth();
    }


}
