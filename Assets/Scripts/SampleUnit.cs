//using Microsoft.Unity.VisualStudio.Editor;
using TbsFramework.Units;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SampleUnit : TbsFramework.Units.Unit
{
    //this code is borrowed from the tutorial in the documentation
    public Color LeadingColor;
    public Vector3 Offset;
    public string UnitName;
    public bool isStructure;
    Color friendlyCustomColor;

    private GameObject healthBar;
    private GameObject actionMarker;
    private GameObject selectionPanel;

    [SerializeField]
    public int Strength;
    public int Int;
    public int Morale;
    private bool selected;


    public override void Initialize()
    {
        base.Initialize();
        GetComponentInChildren<Renderer>().material.color = LeadingColor;
        transform.localPosition += Offset;
        friendlyCustomColor = LeadingColor + new Color(0f,0f,0f,0.5f);
        healthBar = transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).gameObject;
        actionMarker = transform.GetChild(2).transform.GetChild(1).gameObject;
        selectionPanel = GameObject.Find("UnitSelected");
        Debug.Log(selectionPanel);
        selected = false;
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
        selected = true;
    }

    public override void OnUnitDeselected()
    {
        base.OnUnitDeselected();
        selectionPanel.SetActive(false);
        selected = false;
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

    private void OnMouseOver() {
        selectionPanel.SetActive(true);
        if(this.PlayerNumber!=0){
            selectionPanel.transform.GetChild(1).gameObject.SetActive(false);
            selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.red; 
        }
        else{
            selectionPanel.transform.GetChild(1).gameObject.SetActive(true);
            selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(13f,133f,32f); 
        }
        selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Transform>().localScale = new Vector3((float)this.HitPoints/(float)this.TotalHitPoints,1,1);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        if(!selected)
            selectionPanel.SetActive(false);
    }





}
