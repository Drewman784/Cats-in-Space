using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BaseCatalyst : MonoBehaviour
{
    private GameObject ActivationCheck;
    public CatalystMonitor catMon;
    [SerializeField] string condition;
    [SerializeField] string effect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catMon = GameObject.Find("CellGrid").GetComponent<CatalystMonitor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool CheckCatalyst(){ //here, the script should check the conditions of the catalyst trigger
        if(catMon == null){
            catMon = GameObject.Find("CellGrid").GetComponent<CatalystMonitor>();
        }
        return false;
    }

    public virtual void TriggerCatalystEffect(){ //here, the script calls the effect of the catalyst

    }

    public void CheckActivation(){
        //Debug.Log("got to check activation");
        if(ActivationCheck == null){
            ActivationCheck = catMon.GetActivationWindow();
        }
        ActivationCheck.SetActive(true);
        ActivationCheck.GetComponent<CatalystActivationScreen>().DisplayDescription(condition, GetComponent<SampleUnit>().UnitName, effect);
        ActivationCheck.GetComponent<CatalystActivationScreen>().theCatalyst = this;
        //ActivationCheck.transform.GetChild(0)
    }

    public void CloseActivationCheck(){
        ActivationCheck.SetActive(false);
    }
}
