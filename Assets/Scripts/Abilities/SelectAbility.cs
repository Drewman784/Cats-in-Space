using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TMPro;
using UnityEditor;
//using UnityEditor.PackageManager.UI;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.Units.Abilities
{
    public class SelectAbility : Ability
    {
        public List<Button> SelectButtons;
        public List<SelectableAbility> SelectAbilities;
        public UnitInfoPanelScript selectPanel;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private bool firstclick;
        CellGrid cg;

        void Start()
        {
            firstclick = true;
            /*firstclick = false;
            selectPanel = gameObject.GetComponent<SampleUnit>().GetSelectionPanel().GetComponent<UnitInfoPanelScript>();
            //selectPanel = GameObject.Find("UnitSelected").GetComponent<UnitInfoPanelScript>();
            // GameObject g = GameObject.Find("AbilityButtons");
            GameObject g = selectPanel.gameObject.transform.GetChild(1).gameObject;
            SelectButtons.Add(g.transform.GetChild(0).GetComponent<Button>());
            SelectButtons.Add(g.transform.GetChild(1).GetComponent<Button>());
            SelectButtons.Add(g.transform.GetChild(2).GetComponent<Button>()); */

            /*Unit cG = gameObject.GetComponent<Unit>();
            Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable()){
                    SelectAbilities.Add(a);
                }
                Debug.Log(a);
            }*/

        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            //DisplayAbilities();

            base.OnUnitClicked(unit, cellGrid);
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            //DisplayAbilities();
            base.OnCellClicked(cell, cellGrid);
        }

        void OnMouseEnter()
        {
            if (!gameObject.GetComponent<SampleUnit>().DialogueOngoing)
            {
                if (firstclick) //import necessary variables first time
                {
                    selectPanel = gameObject.GetComponent<SampleUnit>().GetSelectionPanel().GetComponent<UnitInfoPanelScript>();
                    selectPanel.gameObject.SetActive(true);
                    GameObject g = selectPanel.gameObject.transform.GetChild(1).gameObject;
                    g.transform.GetChild(0).gameObject.SetActive(true);
                    g.transform.GetChild(1).gameObject.SetActive(true);
                    g.transform.GetChild(2).gameObject.SetActive(true);
                    SelectButtons.Add(g.transform.GetChild(0).GetComponent<Button>());
                    SelectButtons.Add(g.transform.GetChild(1).GetComponent<Button>());
                    SelectButtons.Add(g.transform.GetChild(2).GetComponent<Button>());
                    selectPanel.gameObject.SetActive(false);

                    cg = gameObject.GetComponent<SampleUnit>().GetCellGrid();

                    if (SelectButtons.Count != 3)
                    {
                        Debug.Log("Missing button!");
                    }
                    else
                    {
                        //Debug.Log(SelectButtons.Count);
                    }
                    firstclick = false;
                }
                Debug.Log(selectPanel);
                if (selectPanel.isSelected == false)
                {
                    DisplayAbilities();
                }
            }  
        }

        public void DisplayAbilities(){
            //Debug.Log(GetComponent<SampleUnit>().UnitName + " displaying?");

            SelectAbilities = new List<SelectableAbility>();

            Unit cG = GetComponent<SampleUnit>();
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable() && !SelectAbilities.Contains((SelectableAbility)a)){
                   SelectAbilities.Add((SelectableAbility)a);
                   //a.enabled = false;
                }
                //Debug.Log(a);
            }

            //Debug.Log(SelectAbilities);

            //Unit cG = GetComponent<Unit>();
            //Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            /*foreach(SelectableAbility a in SelectAbilities){
                    a.selected = false;
                //Debug.Log(a);
            }*/

            int check = 0;
            //Debug.Log("select abilities count: " + SelectAbilities.Count);
            //Debug.Log("list: "+ SelectAbilities.Count);
                for(int a = 0; a < SelectAbilities.Count; a++){
                    //Debug.Log(a + " ability name = " + SelectAbilities[a].GetAbilityName());
                SelectButtons[a].gameObject.SetActive(true);
                SelectButtons[a].transform.GetChild(0).GetComponent<Text>().text = SelectAbilities[a].GetAbilityName();
                SelectButtons[a].gameObject.GetComponent<AbilityButtonScript>().SetAbility((SelectableAbility)SelectAbilities[a], cG);
                check++;
                if (GetComponent<SampleUnit>().ActionPoints < 1 || !SelectAbilities[a].CanPerform(cg))
                {
                    SelectButtons[a].GetComponent<Image>().color = Color.grey;
                    SelectButtons[a].interactable = false;
                    Debug.Log("uninteractable?");
                }
                else
                {
                    SelectButtons[a].GetComponent<Image>().color = Color.white;
                    SelectButtons[a].interactable = true;
                }
            }

            //Debug.Log("sab = " + SelectAbilities.Count + " butttons = " + SelectButtons.Count);
            if(SelectAbilities.Count < 3){
                while(check<3){
                    //Debug.Log("inactive "+check);
                    SelectButtons[check].gameObject.SetActive(false);
                    check++;
                }
            }

            /*foreach(SelectableAbility a in SelectAbilities){
                    a.enabled = false;
                //Debug.Log(a);
            }*/  
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            cg = cellGrid;
            if (UnitReference.ActionPoints > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public override onUnitSelected


    }
}
