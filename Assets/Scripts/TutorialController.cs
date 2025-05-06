using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] List<GameObject> theTutorials;
    private int index;
    void Start()
    {
        index = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPanel(){
        index++;
        if(index < theTutorials.Count){
            theTutorials[index-1].SetActive(false);
            theTutorials[index].SetActive(true);
        } else{
            QuitTutorial();
        }
    }

    public void QuitTutorial(){
        gameObject.SetActive(false);
    }
}
