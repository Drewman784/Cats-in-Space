using TbsFramework.Grid;
using TbsFramework.Units;
//using UnityEditor.PackageManager.UI;
using UnityEngine;

public class UnitInfoPanelScript : MonoBehaviour
{
    public bool isSelected;
    public SampleUnit lastUnit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSelected = false;
        lastUnit = null;
        /*Debug.Log(GameObject.Find("CellGrid").GetComponent<CellGrid>().Units + " here?");
        foreach(Unit unit in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units){
            SampleUnit toUse = (SampleUnit)unit;
            toUse.AddInfoPanel(this.gameObject);
        }*/
    }

    void Initialize(){
        /*foreach(Unit unit in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units){
            SampleUnit toUse = (SampleUnit)unit;
            toUse.AddInfoPanel(this.gameObject);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
