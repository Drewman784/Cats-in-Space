//using Microsoft.Unity.VisualStudio.Editor;
using System;
using TbsFramework.Units;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TbsFramework.Cells;

public class SampleUnit : TbsFramework.Units.Unit
{
    //this code is borrowed from the tutorial in the documentation
    public Color LeadingColor;
    public Vector3 Offset;
    public string UnitName;
    public bool isStructure;
    Color friendlyCustomColor;
    Color friendlyHealthColor = new Color32(13,133,32,255); 

    private GameObject healthBar;
    private GameObject actionMarker;
    private GameObject selectionPanel;
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
        //Debug.Log(selectionPanel);
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
        if(this.HitPoints>0){
            selectionPanel.SetActive(true);
            if(this.PlayerNumber!=0){
                selectionPanel.transform.GetChild(1).gameObject.SetActive(false);
                selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.red; 
            }
            else{
                selectionPanel.transform.GetChild(1).gameObject.SetActive(true);
                selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = friendlyHealthColor; 
            }
            selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Transform>().localScale = new Vector3((float)this.HitPoints/(float)this.TotalHitPoints,1,1);
        }
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        if(!selected)
            selectionPanel.SetActive(false);
    }

    public void NewAttackHandler(TbsFramework.Units.Unit unitToAttack, String attackType)
    {
        AttackAction attackAction = NewDealDamage(unitToAttack, attackType);
        MarkAsAttacking(unitToAttack);
        unitToAttack.DefendHandler(this, attackAction.Damage);
        AttackActionPerformed(attackAction.ActionCost); 
    }

    public AttackAction NewDealDamage(TbsFramework.Units.Unit unitToAttack, String attackType){
        switch(attackType){
            case "PHYSICAL":
                return new AttackAction(AttackFactor, 1f);
            case "PSIONIC":
                int dmg = unitToAttack.Morale - this.Morale;
                if((float)unitToAttack.Morale/(float)this.Morale >= 2){
                    dmg = dmg * 3;
                }
                return new AttackAction(dmg, 1f);
            default:
                return new AttackAction(AttackFactor, 1f);
        }
        //return new AttackAction(AttackFactor, 1f);
    }

    public override bool IsCellTraversable(Cell cell)
        {
            return base.IsCellTraversable(cell) || (cell.CurrentUnits.Count > 0 && !cell.CurrentUnits.Exists(u => !(u as SampleUnit).isStructure && u.PlayerNumber != PlayerNumber));
        }

    protected override void OnDestroyed()
    {
        selectionPanel.SetActive(false);
        Debug.Log(selectionPanel + "selection panel gone?");
        base.OnDestroyed();
    }

    public void HealUnit(int amount){
        this.HitPoints += amount;
        if(this.HitPoints>this.TotalHitPoints){
            this.HitPoints = this.TotalHitPoints;
        }
        ShowHealth();
    }


}
