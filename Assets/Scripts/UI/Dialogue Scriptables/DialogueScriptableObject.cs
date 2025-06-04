using UnityEngine;

[CreateAssetMenu]
public class DialogueScriptableObject : ScriptableObject //this class acts as a holder for dialogue scenes so they can be saved seperately from the inspector
{
    [SerializeField] public string Character1Name;
    [SerializeField] public string Character2Name;
    [SerializeField] public Sprite Character1Sprite;
    [SerializeField] public Sprite Character2Sprite;
    [SerializeField] public string[] theLines;

}
