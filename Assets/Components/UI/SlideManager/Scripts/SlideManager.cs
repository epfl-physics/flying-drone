using UnityEngine;

namespace Slides
{
    public class SlideManager : MonoBehaviour
    {
        [Header("Slides")]
        [SerializeField] private Transform slideContainer;
        [SerializeField] private int currentSlideIndex = 0;

        [Header("Navigation")]
        [SerializeField] private Navigation navigation;
        [SerializeField] private bool bubblesAreClickable = true;
        [SerializeField] private bool useProgressBar;

        [Header("Slide Transitions")]
        [SerializeField, Min(0)] private float fadeInTime = 0.3f;
        [SerializeField, Min(0)] private float fadeOutTime = 0.3f;

        private void Awake()
        {
            // Do not proceed if no slide container has been assigned in the inspector
            if (!slideContainer)
            {
                Debug.LogWarning("A SlideContainer has not been assigned.");
                return;
            }

            // Deactivate all slides at the start
            foreach (var slide in slideContainer.GetComponentsInChildren<Slide>())
            {
                slide.Deactivate();
            }
        }

        private void OnEnable()
        {
            Navigation.OnChangeSlide += LoadSlide;
        }

        private void OnDisable()
        {
            Navigation.OnChangeSlide -= LoadSlide;
        }

        private void Start()
        {
            // Dynamically create the navigation bubbles or progress bar
            GenerateNavigationUI();

            // Load the starting slide
            LoadInitialSlide();
        }

        private void GenerateNavigationUI()
        {
            // Do not create navigation UI if there are no slides
            if (!slideContainer) return;

            if (!navigation)
            {
                Debug.LogWarning("SlideManager did not find a child GameObject called Navigation.");
                return;
            }

            // Create navigation bubbles (or a progress bar) and activate the correct one
            if (useProgressBar)
            {
                navigation.useProgressBar = true;
                navigation.SetNumSlides(slideContainer.childCount);
            }
            else
            {
                navigation.HideProgressBar();
                navigation.SetBubbleClickability(bubblesAreClickable);
                navigation.GenerateBubbles(slideContainer.childCount);
            }

            navigation.SetCurrentSlideIndex(currentSlideIndex);
            navigation.ChangeSlide(currentSlideIndex, false);
        }

        private void LoadInitialSlide()
        {
            if (!slideContainer) return;
            if (currentSlideIndex < 0 || currentSlideIndex >= slideContainer.childCount) return;

            Slide initialSlide = slideContainer.GetChild(currentSlideIndex).GetComponent<Slide>();
            initialSlide.FadeIn(fadeInTime, true);
        }

        public void LoadSlide(int slideIndex)
        {
            if (!slideContainer) return;
            if (currentSlideIndex == slideIndex || currentSlideIndex >= slideContainer.childCount) return;

            //Debug.Log("Slide Manager > Loading slide index " + slideIndex);

            // Turn off the current slide
            Slide currentSlide = slideContainer.GetChild(currentSlideIndex).GetComponent<Slide>();
            currentSlide.FadeOut(fadeOutTime);

            // Turn on the requested slide
            Slide nextSlide = slideContainer.GetChild(slideIndex).GetComponent<Slide>();
            nextSlide.FadeIn(fadeInTime);

            currentSlideIndex = slideIndex;
        }
    }
}