using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HintPanel : MonoBehaviour
{
    public Puzzles puzzleManager;
    public void Fade()
    {
        Debug.Log("Pressed");
        transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.parent.GetComponent<CanvasGroup>().DOFade(0, 1f);

        if(puzzleManager!=null && puzzleManager.currentIndex == puzzleManager.puzzles.Count)
        {
            puzzleManager.ClearPanel();
        }
    }
}
