//using Microsoft.Unity.VisualStudio.Editor;
using System;
using TbsFramework.Units;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TbsFramework.Cells;
using TMPro;
using TbsFramework.Grid;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TbsFramework.Units.Abilities;


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
    private CellGrid cellGrid;

    private GameObject hitDisplay;
    private bool hitDisplayed;
    private float hitct;

    public Animator anim;

    public AudioClip AttackSound; // Sound to play when attacking
    private AudioSource audioSource; // Audio source component
    //[SerializeField] GameObject deathPrefab;

    public SampleUnit lastTarget;
    public int totalDamageTaken;

    private CatalystMonitor catMon;
    [SerializeField] Sprite portrait;

    protected override int Defend(TbsFramework.Units.Unit other, int damage)
    {
        //other.GetComponent<SampleUnit>().anim.SetTrigger("Shoot");
        return damage - (Cell as Tile_Script).DefenseBoost;
    }

    public void Update()
    {
        if(hitDisplayed){
            hitct+=Time.deltaTime;
            if(hitct>2){
                hitDisplayed = false;
                hitDisplay.SetActive(false);
            }
        }
    }


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

        hitDisplayed = false;
        hitDisplay = transform.GetChild(2).transform.GetChild(2).gameObject;
        hitDisplay.SetActive(false);

        anim = gameObject.transform.GetChild(1).GetComponent<Animator>();

        if(this.PlayerNumber!=0){
            UnityEngine.UI.Image hBar = healthBar.GetComponent<UnityEngine.UI.Image>();
            hBar.color = Color.red; 
            actionMarker.SetActive(false);
        }

        cellGrid = GameObject.Find("CellGrid").GetComponent<CellGrid>();

        catMon = GameObject.Find("CellGrid").GetComponent<CatalystMonitor>();
    }

    public void AddInfoPanel(GameObject what){
        selectionPanel = what;
    }

    public override void MarkAsFriendly()
    {
        //Debug.Log("Mark");
        GetComponentInChildren<Renderer>().material.color  = friendlyCustomColor;
    }

    public override void MarkAsReachableEnemy()
    {
        //GetComponentInChildren<Renderer>().material.color = Color.red;
        this.Cell.GetComponent<Renderer>().material.color = Color.red;
    }

    public void MarkAsReachableAlly(){
         this.Cell.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void MarkAsSelected()
    {
        GetComponentInChildren<Renderer>().material.color = Color.green;
        selected = true;
        selectionPanel.SetActive(true);
        selectionPanel.GetComponent<UnitInfoPanelScript>().isSelected = true;
    }

    public override void OnUnitDeselected()
    {
        base.OnUnitDeselected();
        selectionPanel.SetActive(false);
        selected = false;
        selectionPanel.GetComponent<UnitInfoPanelScript>().isSelected = false;
        //Debug.Log(this.UnitName + " was deselected");
    }

    public override void MarkAsFinished()
    {
        GetComponentInChildren<Renderer>().material.color = Color.gray;
        actionMarker.SetActive(false);
        //GetComponent<BaseCatalyst>().CheckCatalyst();
        //anim.SetBool("Walking", false);
        GameObject.Find("CellGrid").GetComponent<CellGrid>().CheckUnitsFinished();

        if(this.PlayerNumber == 0){ //check the catalysts
            cellGrid.CheckCatalysts();
        }
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
        this.Cell.GetComponent<Renderer>().material.color = Color.white;
    }

    public void ShowHealth(){ //updates healthbar
        float barHealth = (float)this.HitPoints/(float)this.TotalHitPoints;
        healthBar.GetComponent<Transform>().localScale = new Vector3(barHealth,1,1);
        //Debug.Log("hp:" + this.HitPoints + "%"+ this.TotalHitPoints+ " -> "+(barHealth));

        if(selected){ //if selected, also update stats
            OnMouseOver();
        }
    }


    protected override void DefenceActionPerformed(){ //runs after unit has been attacked
        ShowHealth();
    }

    private void OnMouseOver() { // show the details window
        if(this.HitPoints>0){
            selectionPanel.SetActive(true);
            if(selectionPanel.GetComponent<UnitInfoPanelScript>().isSelected == false){
                //Debug.Log(selectionPanel.GetComponent<UnitInfoPanelScript>().isSelected + " no?");
                if(this.PlayerNumber!=0){ // change healthbar color based on unit status
                    selectionPanel.transform.GetChild(1).gameObject.SetActive(false);
                    selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.red; 
                }
                else{
                    selectionPanel.transform.GetChild(1).gameObject.SetActive(true);
                    selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = friendlyHealthColor; 
                }
                //modify health bar
                selectionPanel.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = portrait;
                selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Transform>().localScale = new Vector3((float)this.HitPoints/(float)this.TotalHitPoints,1,1);
                selectionPanel.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = this.UnitName;
                selectionPanel.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = this.HitPoints +"/"+ this.TotalHitPoints;
                selectionPanel.transform.GetChild(2).transform.GetChild(2).GetComponent<Text>().text = "Atk: " + this.AttackFactor + " | " + "Range: " + this.AttackRange;
            }

        }
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        if(!selected && selectionPanel.GetComponent<UnitInfoPanelScript>().isSelected == false)
            selectionPanel.SetActive(false);
    }

    public override void AttackHandler(TbsFramework.Units.Unit unitToAttack)
    {
        ///base.AttackHandler(unitToAttack);
        NewAttackHandler(unitToAttack, "PHYSICAL", 0);
    }

    public void NewAttackHandler(TbsFramework.Units.Unit unitToAttack, String attackType, int addedDamage)
    {
        AttackAction attackAction = NewDealDamage(unitToAttack, attackType, addedDamage);
        MarkAsAttacking(unitToAttack);
        //Debug.Log("reached new attack handler - "+(attackAction.Damage + addedDamage));
        unitToAttack.DefendHandler(this, attackAction.Damage);
        AttackActionPerformed(attackAction.ActionCost); 
        GetComponent<BaseCatalyst>().CheckCatalyst();

        /*if(unitToAttack.HitPoints>0){
            unitToAttack.GetComponent<BaseCatalyst>().CheckCatalyst();
        }*/
    }

    public AttackAction NewDealDamage(TbsFramework.Units.Unit unitToAttack, String attackType, int addedDamage){
        anim.SetTrigger("Shoot");
        //Debug.Log("here!");
        int TempAttackFactor = AttackFactor+addedDamage;
        catMon.RegisterDamage(TempAttackFactor,this.PlayerNumber,this,(SampleUnit)unitToAttack);
        switch(attackType){
            case "PHYSICAL":
                return new AttackAction(TempAttackFactor, 1f);
            case "PSIONIC":
                int dmg = unitToAttack.Morale - this.Morale;
                if((float)unitToAttack.Morale/(float)this.Morale >= 2){
                    dmg = dmg * 3;
                }
                return new AttackAction(dmg, 1f);
            default:
                return new AttackAction(TempAttackFactor, 1f);
        }
        //return new AttackAction(AttackFactor, 1f);
    }

    public void TakeEnvironmentalDamage(int damage){ // this unit is damaged by the environment
        catMon.RegisterDamage(damage, this.PlayerNumber, this, this);
        AttackAction attackAction = new AttackAction(damage, 1f);
        DefendHandler(this, attackAction.Damage);

        if(HitPoints>0){ //if still alive, check the catalyst
            GetComponent<BaseCatalyst>().CheckCatalyst();
        }
    }

    public override bool IsCellTraversable(Cell cell)
        {
            return base.IsCellTraversable(cell) || (cell.CurrentUnits.Count > 0 && !cell.CurrentUnits.Exists(u => !(u as SampleUnit).isStructure && u.PlayerNumber != PlayerNumber));
        }

    protected override void OnDestroyed()
    {
        selectionPanel.SetActive(false);
        /*GameObject dPrefab = Instantiate(deathPrefab, this.transform);
        dPrefab.transform.parent = this.transform.parent;
        dPrefab.transform.localScale = transform.localScale;
        dPrefab.transform.position = this.transform.position;
        if(PlayerNumber!=0){
            dPrefab.GetComponent<SpriteRenderer>().flipX = false;
        }
        dPrefab.GetComponent<Animator>().SetTrigger("Dead");
        Debug.Log(dPrefab);*/
        GameObject spriter = transform.GetChild(1).gameObject;
        spriter.transform.parent = this.Cell.transform;
        spriter.transform.position = new Vector3(0,0.16f,0);
        //spriter.transform.localScale = this.transform.localScale;
        anim.SetTrigger("Dead");
        catMon.RegisterKill(this.PlayerNumber);
        Debug.Log(selectionPanel + "selection panel gone?");
        base.OnDestroyed();
    }

    public void HealUnit(int amount){
        Debug.Log("heal for: " + amount);
        this.HitPoints += amount;
        if(this.HitPoints>this.TotalHitPoints){
            this.HitPoints = this.TotalHitPoints;
        }
        ShowHealth();
    }

    /**public override void DefendHandler(TbsFramework.Units.Unit aggressor, int damage)
    {
        base.DefendHandler(aggressor, damage);

        Debug.Log("marking!");
        hitDisplayed = true;
        hitDisplay.SetActive(true);
        hitDisplay.GetComponent<TextMeshProUGUI>().text = "-" + damage;
        hitct = 0;
    }**/

    protected override IEnumerator MovementAnimation(IList<Cell> path)
    {
        anim.SetBool("Walking", true);
        yield return base.MovementAnimation(path);
        anim.SetBool("Walking", false);
    }

    // Attack sounds and such
    public override void OnUnitSelected()
    {
        base.OnUnitSelected();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if(selectionPanel.GetComponent<UnitInfoPanelScript>().lastUnit != null){
            selectionPanel.GetComponent<UnitInfoPanelScript>().lastUnit.OnUnitDeselected();
        }
        selectionPanel.GetComponent<UnitInfoPanelScript>().isSelected = true;
        //Debug.Log("selected unit " + this.UnitName);
    
        if(this.PlayerNumber!=0){ // change healthbar color based on unit status
                    selectionPanel.transform.GetChild(1).gameObject.SetActive(false);
                    selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.red; 
                }
                else{
                    //Debug.Log("call display abilities?");
                    GetComponent<SelectAbility>().DisplayAbilities();
                    selectionPanel.transform.GetChild(1).gameObject.SetActive(true);
                    selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = friendlyHealthColor; 
                }

                //modify health bar
                selectionPanel.transform.GetChild(2).transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Transform>().localScale = new Vector3((float)this.HitPoints/(float)this.TotalHitPoints,1,1);
                selectionPanel.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = this.UnitName;
                selectionPanel.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = this.HitPoints +"/"+ this.TotalHitPoints;
                selectionPanel.transform.GetChild(2).transform.GetChild(2).GetComponent<Text>().text = "Atk: " + this.AttackFactor + " | " + "Range: " + this.AttackRange;
                selectionPanel.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = portrait;
    }


    protected override void AttackActionPerformed(float actionCost)
    {
        base.AttackActionPerformed(actionCost);
        if (AttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(AttackSound);
        }
    }

    public override bool CatalystRelay() //called by cellgrid -> unit -> sample unit -> catalyst script
    {
        return GetComponent<BaseCatalyst>().CheckCatalyst();
    }

    public override void DefendHandler(TbsFramework.Units.Unit aggressor, int damage)
    {
        base.DefendHandler(aggressor, damage);
        anim.SetTrigger("TakeDamage");
    }


}

