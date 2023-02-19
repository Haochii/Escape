using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Parameters")]
    public int escapeRoomTimeLimit;
    public Color clearedColor;
    public Color disabledColor;
    public int wrongPasswordTimePenalty;
    public int totalSectionCnt;

    [Header("Status")]
    [SerializeField]
    private int finishedSectionCnt;

    [Header("Referencess")]
    public GameObject escapeRoomPanel;
    public SlideController slideController;
    public AudioSource voiceOver;
    public AudioSource bgMusic;
    public AudioSource tickSfx;
    public GameObject glitch;
    public TMPro.TMP_Text escapeRoomTimer;
    public AudioClip startVoiceOver;
    public AudioClip winVoiceOver;
    public AudioClip loseVoiceOver;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [SerializeField]
    private bool isSlides;

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SlideToPanel();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlideToPanel()
    {
        Debug.Log("Slides end");
        //Turnoff controller
        slideController.enabled = false;

        //Voiceover
        voiceOver.PlayOneShot(startVoiceOver);



        //Glitch effect
        glitch.SetActive(true);
        glitch.GetComponent<Animator>().Play("glitch");

        //enable Escape Room Panel in delay
        StartCoroutine(delayPanel());
        
    }

    public void PanelSectionFinished()
    {
        finishedSectionCnt++;
        if(finishedSectionCnt == totalSectionCnt)
        {
            winPanel.SetActive(true);
            winPanel.GetComponent<CanvasGroup>().DOFade(1f, 3f).OnComplete(()=>Win());
        }
    }

    void Win()
    {
        tickSfx.Stop();
        voiceOver.PlayOneShot(winVoiceOver);
    }

    void GameOver()
    {
        tickSfx.Stop();
        voiceOver.PlayOneShot(loseVoiceOver);
    }

    IEnumerator delayPanel()
    {
        yield return new WaitForSeconds(startVoiceOver.length);
        escapeRoomTimer.transform.parent.gameObject.SetActive(true);
        escapeRoomTimer.transform.parent.GetComponent<CanvasGroup>().DOFade(1, 3f);
        escapeRoomPanel.SetActive(true);
        escapeRoomPanel.GetComponent<CanvasGroup>().DOFade(1,3f);
        StartCoroutine(panelCountdown());
    }
    IEnumerator panelCountdown()
    {
        int timer = escapeRoomTimeLimit;
        while(timer >= 0)
        {
            string min = Mathf.FloorToInt(timer / 60).ToString();
            min = min.Length == 1 ? "0" + min : min;
            string sec = (timer - Mathf.FloorToInt(timer / 60) * 60).ToString();
            sec = sec.Length == 1 ? "0" + sec : sec;

            escapeRoomTimer.text = min + ":" + sec;
            timer--;
            //Tick
            tickSfx.Play();
            yield return new WaitForSeconds(1.0f);

        }
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(1f, 3f).OnComplete(() => GameOver());
        //Play voiceover
    }

}
