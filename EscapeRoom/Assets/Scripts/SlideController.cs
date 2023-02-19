using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlideController : MonoBehaviour
{
    [SerializeField]
    private int currentSlideIndex;
    [SerializeField]
    private List<GameObject> slides = new List<GameObject>();
    private Tween slideChangeTween;


    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform slide in transform)
        {
            slides.Add(slide.gameObject);
        }
        currentSlideIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //Slide Switch
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwitchSlide(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchSlide(false);
        }

        
    }

    void SwitchSlide(bool isNext)
    {
        if(!slideChangeTween.IsActive())
        {
            int nextSlideIndex = isNext ? currentSlideIndex + 1 : currentSlideIndex - 1;
            if (0 <= nextSlideIndex && nextSlideIndex < slides.Count)
            {
                //slide switching
                GameObject currentSlide = slides[currentSlideIndex];
                GameObject nextSlide = slides[nextSlideIndex];

                if(nextSlide.GetComponent<Slide>().isFading)
                {
                    var duration = nextSlide.GetComponent<Slide>().fadingDuration;
                    slideChangeTween = currentSlide.GetComponent<Image>().DOColor(Color.clear, duration);
                    nextSlide.GetComponent<Image>().DOColor(Color.white, duration);
                }
                else
                {
                    currentSlide.GetComponent<Image>().color = Color.clear;
                    nextSlide.GetComponent<Image>().color = Color.white;
                }
                currentSlideIndex = nextSlideIndex;
            }
            else if (nextSlideIndex >= slides.Count)
            {

                //trigger glitch and voice over
                GameManager.instance.SlideToPanel();
            }
        }
        
    }

}
