using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButtonScript : MonoBehaviour, IPointerEnterHandler,  IPointerExitHandler
{
    private SelectableAbility theAbility;
    CellGrid cG;
    TbsFramework.Units.Unit unit;
    [SerializeField] GameObject AbilityDescriptionPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cG = GameObject.Find("CellGrid").GetComponent<CellGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallAbility(){
        //cG.cellGridState = new CellGridStateAbilitySelected(cG, unit, theAbility);
        //theAbility.DoAction(cG);
        Debug.Log("button click!");
        if(theAbility.IsInstant()){
            theAbility.DoAction(cG);
            //Debug.Log("instant! " + theAbility);
        } else{
            //Debug.Log("noninstant");
            cG.cellGridState = new CellGridStateAbilitySelected(cG, unit, new List<Ability>() { theAbility });
            theAbility.GetComponent<SelectableAbility>().selected = true;
        }
        unit.GetComponent<AttackAbility>().enabled  = false;

    }

    public void SetAbility(SelectableAbility toSet, TbsFramework.Units.Unit tUnit){
        theAbility = toSet;
        unit = tUnit;
    }

    void OnMouseOver()
    {
        AbilityDescriptionPanel.SetActive(true);
        AbilityDescriptionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = theAbility.GetAbilityName();
        AbilityDescriptionPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = theAbility.GetAbilityDescription();
    }

    void OnMouseExit()
    {
        AbilityDescriptionPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AbilityDescriptionPanel.SetActive(true);
        AbilityDescriptionPanel.transform.GetChild(0).GetComponent<Text>().text = theAbility.GetAbilityName();
        AbilityDescriptionPanel.transform.GetChild(1).GetComponent<Text>().text = theAbility.GetAbilityDescription();
    }

    public void OnPointerExit(PointerEventData eventData){
         AbilityDescriptionPanel.SetActive(false);
    }
}
