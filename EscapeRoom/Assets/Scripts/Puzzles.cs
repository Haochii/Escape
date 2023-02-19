using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzles : MonoBehaviour
{
    private Color clearedColor;
    private Color disabledColor;
    private int timePenalty;
    private Transform penaltyPanel;
    private Transform hintPanel;
    private Transform clearPanel;

    private AudioSource audioSource;
    public string sectionName;
    public int currentIndex;
    public List<string> passwords;
    public List<GameObject> puzzles;
    public List<string> hints;

    public AudioClip correctPasswordSFX;
    public AudioClip wrongAnswerSFX;


    private void Awake()
    {
        
        
    }
    public void NextPuzzle()
    {
        audioSource.PlayOneShot(correctPasswordSFX);
        puzzles[currentIndex].GetComponentInChildren<TMPro.TMP_InputField>().interactable = false;
        var currentInputField = puzzles[currentIndex].GetComponentInChildren<Image>();
        currentInputField.DOColor(clearedColor, 1f);
        currentInputField.transform.DOShakeScale(.1f);
        if(currentIndex + 1 >= puzzles.Count)
        {
            //End this section
            Debug.Log("Section End");
            clearPanel.gameObject.SetActive(true);
            clearPanel.GetComponent<CanvasGroup>().DOFade(1f,1f).OnComplete(()=>GameManager.instance.PanelSectionFinished());
        }
        else
        {
            hintPanel.GetComponentInChildren<TMPro.TMP_Text>().text = hints[currentIndex + 1];
            hintPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(()=> hintPanel.GetComponent<CanvasGroup>().blocksRaycasts = true);
            
            puzzles[currentIndex + 1].GetComponentInChildren<TMPro.TMP_InputField>().interactable = true;
            var nextInputField = puzzles[currentIndex + 1].GetComponentInChildren<Image>();
            nextInputField.DOColor(Color.white, 1f);
            currentIndex++;
        }
        
        
    }   

    public void WrongPassword()
    {
        //TimePenalty
        penaltyPanel.gameObject.SetActive(true);
        audioSource.PlayOneShot(wrongAnswerSFX);
        penaltyPanel.DOScale(Vector3.one, .5f).SetEase(Ease.OutBounce);
        puzzles[currentIndex].GetComponentInChildren<TMPro.TMP_InputField>().interactable = false;
        StartCoroutine(WaitTimePenalty());

    }

   IEnumerator WaitTimePenalty()
    {
        int timer = timePenalty;
        while(timer > 0)
        {
            penaltyPanel.GetChild(1).GetComponent<TMPro.TMP_Text>().text = timer.ToString();
            timer--;
            yield return new WaitForSeconds(1.0f);
        }
        penaltyPanel.DOScale(Vector3.zero, .5f).SetEase(Ease.InBounce).OnComplete(() => penaltyPanel.gameObject.SetActive(false)) ;
        puzzles[currentIndex].GetComponentInChildren<TMPro.TMP_InputField>().interactable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        disabledColor = GameManager.instance.disabledColor;
        int cnt = 0;
        foreach (PasswordValidation puzzle in GetComponentsInChildren<PasswordValidation>())
        {
            if (cnt != 0)
            {
                puzzle.GetComponentInChildren<Image>().color = disabledColor;
            }
            puzzle.password = passwords[cnt];
            puzzle.puzzleManager = this;
            puzzles.Add(puzzle.gameObject);
            cnt++;
            
        }

        currentIndex = 0;
        transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = sectionName;
        clearedColor = GameManager.instance.clearedColor;
        timePenalty = GameManager.instance.wrongPasswordTimePenalty;
        penaltyPanel = transform.GetChild(2);
        hintPanel = transform.GetChild(3);
        clearPanel = transform.GetChild(4);
        puzzles[currentIndex].GetComponentInChildren<TMPro.TMP_InputField>().interactable = true;
        audioSource = GetComponentInChildren<AudioSource>();

        hintPanel.GetComponentInChildren<TMPro.TMP_Text>().text = hints[currentIndex];
        hintPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() => hintPanel.GetComponent<CanvasGroup>().blocksRaycasts = true);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
