using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrePuzzle : MonoBehaviour
{
    public string password;
    public HintPanel hintPanel;
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
        if (input.Length == password.Length)
        {
            if (input == password)
            {
                Debug.Log("correct!");
                GameManager.instance.ShowEscaperoomPanel();
            }
            else
            {
                Debug.Log("Incorrect!");
                hintPanel.transform.parent.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() => hintPanel.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true);
                //Audio
            }
        }

    }
}
