using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units;
using Unity.VisualScripting;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public class DialogueReader : MonoBehaviour
{
    [SerializeField] private string name1; //character who speaks, portrait on the left
    [SerializeField] private string name2; // character who speaks, portrait on the right

    [SerializeField] private string[] Lines; //dialogue spoken, format: "1: the line"

    [SerializeField] private Sprite portraitImage1; //portrait images to be displayed
    [SerializeField] private Sprite portraitImage2;

    [SerializeField] private DialogueScriptableObject preCreatedScriptable;

    private GameObject portrait1; //portraits to be set active/inactive
    private GameObject portrait2;

    private Text lineText; //text to be displayed

    private int lineIndex;

    [SerializeField] GameObject uiToHide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineIndex = 0;

        portrait1 = transform.GetChild(0).gameObject; //define gameobjects and text to be manipulated
        portrait2 = transform.GetChild(1).gameObject;
        lineText = transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

        if (preCreatedScriptable != null)
        { //if there's an associated scriptable, import info from there
            name1 = preCreatedScriptable.Character1Name;
            name2 = preCreatedScriptable.Character2Name;
            portraitImage1 = preCreatedScriptable.Character1Sprite;
            portraitImage2 = preCreatedScriptable.Character2Sprite;
            Lines = preCreatedScriptable.theLines;
        }

        portrait1.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name1; //put in names and sprites
        portrait2.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name2;
        portrait1.GetComponent<Image>().sprite = portraitImage1;
        portrait2.GetComponent<Image>().sprite = portraitImage2;

        UpdateLineDisplayed();

        CellGrid cG = GameObject.Find("CellGrid").GetComponent<CellGrid>(); // ensure no unit popups during dialogue
        foreach (TbsFramework.Units.Unit u in cG.Units)
        {
            SampleUnit su = (SampleUnit)u;
            su.DialogueOngoing = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void NextLine(){ //check for next line, move to it or exit dialogue
        if (lineIndex < Lines.Length - 1)
        {
            lineIndex++;
            UpdateLineDisplayed();
        }
        else
        {
            //uiToHide.SetActive(false);
            CellGrid cG = GameObject.Find("CellGrid").GetComponent<CellGrid>(); // allow no unit popups after dialogue
            foreach (TbsFramework.Units.Unit u in cG.Units)
            {
                SampleUnit su = (SampleUnit)u;
                su.DialogueOngoing = false;
            }

            gameObject.SetActive(false);
        }
    }

    void UpdateLineDisplayed(){
        string fullLine = Lines[lineIndex];

        string charNum = fullLine.Substring(0,1); //figure out who is speaking
        Debug.Log(charNum);
        int charInt = int.Parse(charNum);

        if(charInt == 1){ //display correct portrait
            portrait1.SetActive(true);
            portrait2.SetActive(false);
        } else{
            portrait1.SetActive(false);
            portrait2.SetActive(true);
        }
       
        lineText.text = fullLine.Substring(2); //display text
        //uiToHide.SetActive(false);
    }

    public void OnMouseDown(){
        Debug.Log("CLICK");
        NextLine();
    }

  
}
