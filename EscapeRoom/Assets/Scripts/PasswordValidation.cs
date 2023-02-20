using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PasswordValidation : MonoBehaviour
{
    public Puzzles puzzleManager;
    public string password;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMPro.TMP_InputField>().characterLimit = password.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Validate(string input)
    {

        if(input.Length == password.Length)
        {
            if (input == password)
            {
                Debug.Log("correct!");
                puzzleManager.NextPuzzle();
            }
            else
            {
                Debug.Log("Incorrect!");
                puzzleManager.WrongPassword();
                //Audio
            }
        }
        
    }
}
