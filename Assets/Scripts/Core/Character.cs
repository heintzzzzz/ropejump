using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    /*public enum CharacterTypes
    {
        Player,
        AI
    }*/ 

    [SerializeField] private MyLibrary.CharacterTypes characterType; 
    [SerializeField] private GameObject characterSprite;
    [SerializeField] private Animator characterAnimator;

    public MyLibrary.CharacterTypes CharacterType => characterType;  
    public GameObject CharacterSprite => characterSprite;
    public Animator CharacterAnimator => characterAnimator;

    void Awake()
    {
        // если в инспекторе не назначен — берём аниматор со спрайта
        if (characterAnimator == null) characterAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}