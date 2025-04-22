using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CatalystActivationScreen : MonoBehaviour
{
    public BaseCatalyst theCatalyst;
    [SerializeField] Text descText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDescription(string condition, string name, string effect){
        string toDisplay = condition + " has triggered " + name +"'s Cat-Alyst ability: " + effect;
        toDisplay += "\n Activate now?";
        descText.text = toDisplay;
    }

    public void CancelAbility(){ //window is set inactive, catalyst set null
        theCatalyst = null;
        this.gameObject.SetActive(false);
    }

    public void ActivateAbility(){ //catalyst triggered, then window closed
        theCatalyst.TriggerCatalystEffect();
        CancelAbility();
    }
}
