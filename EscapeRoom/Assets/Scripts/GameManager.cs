using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public int preTimerLimit;


    [Header("Status")]
    [SerializeField]
    private int finishedSectionCnt;

    [Header("Referencess")]
    public GameObject prePuzzle;
    public GameObject escapeRoomPanel;
    public SlideController slideController;
    public AudioSource voiceOver;
    public AudioSource bgMusic;
    public AudioSource tickSfx;
    public GameObject glitch;
    public TMPro.TMP_Text escapeRoomTimer;
    public AudioClip glitchSFX;
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
        string answer = "";
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                var seed = Random.Range(0, 35);
               
                if (seed >= 26)
                {
                    answer += (char)((int)'0' + (seed - 26)) + " ";
                }
                else
                {
                    answer += (char)((int)'A' + (seed)) + " ";
                } 
                
            }
        }
        Debug.Log(answer);
    }


    // Update is called once per frame
    void Update()
    {
        //DEV SHORTCUT
        if(Input.GetKeyDown(KeyCode.LeftControl) )
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadScene(0);
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                StopAllCoroutines();
                escapeRoomPanel.GetComponent<CanvasGroup>().alpha = 0;
                escapeRoomPanel.SetActive(false);
                escapeRoomTimer.transform.parent.gameObject.SetActive(false);
                winPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                voiceOver.Stop();

                SlideToPanel();
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                StopAllCoroutines();
                escapeRoomPanel.GetComponent<CanvasGroup>().alpha = 0;
                escapeRoomPanel.SetActive(false);
                escapeRoomTimer.transform.parent.gameObject.SetActive(false);
                winPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                voiceOver.Stop();

                escapeRoomTimer.transform.parent.GetComponent<Image>().enabled = false;
                escapeRoomTimer.transform.parent.gameObject.SetActive(true);
                escapeRoomTimer.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
                escapeRoomTimer.transform.parent.GetComponent<CanvasGroup>().DOFade(1, 1f);
                ShowEscaperoomPanel();
                tickSfx.PlayOneShot(glitchSFX);
            }
        }
    }

    public void SlideToPanel()
    {
        Debug.Log("Slides end");
        //Turnoff controller
        slideController.enabled = false;

        //Voiceover
        tickSfx.PlayOneShot(glitchSFX);
        voiceOver.PlayDelayed(5.0f);


        //Glitch effect
        glitch.SetActive(true);
        glitch.GetComponent<Animator>().Play("glitch");

        //enable Escape Room Panel in delay
        StartCoroutine(PreTimer());
        
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
        StopAllCoroutines();
        voiceOver.PlayOneShot(winVoiceOver);
        StartCoroutine(ThankYou(winVoiceOver.length));

    }

    void GameOver()
    {
        tickSfx.Stop();
        StopAllCoroutines();
        voiceOver.PlayOneShot(loseVoiceOver);
        StartCoroutine(ThankYou(loseVoiceOver.length));
    }

    IEnumerator ThankYou(float sec)
    {
        yield return new WaitForSeconds(sec+5);
        winPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().DOColor(Color.white, 1f);
    }

    IEnumerator PreTimer()
    {
        yield return new WaitForSeconds(startVoiceOver.length + 10);
        prePuzzle.SetActive(true);
        prePuzzle.GetComponent<CanvasGroup>().alpha = 0;
        prePuzzle.GetComponent<CanvasGroup>().DOFade(1, 5f);

        //After start voiceover
        //escapeRoomTimer.transform.parent.GetComponent<Image>().enabled = false;
        //escapeRoomTimer.transform.parent.gameObject.SetActive(true);
        //escapeRoomTimer.transform.parent.GetComponent<CanvasGroup>().DOFade(1, 5f);

        //int timer = preTimerLimit;
        //while(timer >= 0)
        //{
        //    string min = Mathf.FloorToInt(timer / 60).ToString();
        //    min = min.Length == 1 ? "0" + min : min;
        //    string sec = (timer - Mathf.FloorToInt(timer / 60) * 60).ToString();
        //    sec = sec.Length == 1 ? "0" + sec : sec;

        //    escapeRoomTimer.text = min + ":" + sec;
        //    timer--;
        //    Tick
        //    tickSfx.Play();
        //    yield return new WaitForSeconds(1.0f);
        //}
        //ShowEscaperoomPanel();


    }
    public void ShowEscaperoomPanel()
    {
        tickSfx.PlayOneShot(glitchSFX);
        prePuzzle.SetActive(false);
        escapeRoomTimer.transform.parent.gameObject.SetActive(true);
        escapeRoomTimer.transform.parent.GetComponent<CanvasGroup>().DOFade(1, 5f);
        escapeRoomPanel.SetActive(true);
        escapeRoomPanel.GetComponent<CanvasGroup>().DOFade(1,1f).SetEase(Ease.InOutBounce);
        escapeRoomTimer.transform.parent.GetComponent<Image>().enabled = true;
        escapeRoomTimer.transform.parent.GetComponent<Image>().color = Color.black;
        escapeRoomTimer.transform.parent.GetComponent<Image>().DOColor(Color.white, 1f);
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
        winPanel.SetActive(true);
        winPanel.GetComponent<CanvasGroup>().DOFade(1f, 3f).OnComplete(() => GameOver());
        //Play voiceover
    }

}
