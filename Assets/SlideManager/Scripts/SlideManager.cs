using UnityEngine;

namespace Slides
{
    public class SlideManager : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] private Transform header;
        [SerializeField] private bool showHeader = true;

        [Header("Slides")]
        [SerializeField] private Transform slideContainer;
        [SerializeField] private int currentSlideIndex = 0;

        [Header("Navigation")]
        [SerializeField] private Transform navigation;
        [SerializeField] private bool bubblesAreClickable = true;
        [SerializeField] private bool useProgressBar;

        [Header("Slide Transitions")]
        [SerializeField, Min(0)] private float fadeInTime = 0.3f;
        [SerializeField, Min(0)] private float fadeInDelay = 0f;
        [SerializeField, Min(0)] private float fadeOutTime = 0.3f;
        [SerializeField, Min(0)] private float fadeOutDelay = 0f;

        private void Awake()
        {
            // Set header visibility
            if (header) header.gameObject.SetActive(showHeader);

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

            if (navigation == null)
            {
                Debug.LogWarning("SlideManager did not find a child GameObject called Navigation.");
                return;
            }

            // Create navigation bubbles (or a progress bar) and activate the correct one
            if (navigation.TryGetComponent(out Navigation nav))
            {
                if (useProgressBar)
                {
                    nav.useProgressBar = true;
                    nav.SetNumSlides(slideContainer.childCount);
                }
                else
                {
                    nav.SetBubbleClickability(bubblesAreClickable);
                    nav.GenerateBubbles(slideContainer.childCount);
                }
                nav.SetCurrentSlideIndex(currentSlideIndex);
                nav.ChangeSlide(currentSlideIndex, false);
            }
        }

        private void LoadInitialSlide()
        {
            if (!slideContainer) return;
            if (currentSlideIndex < 0 || currentSlideIndex >= slideContainer.childCount) return;

            Slide initialSlide = slideContainer.GetChild(currentSlideIndex).GetComponent<Slide>();
            initialSlide.FadeIn(fadeInTime, fadeOutTime, true);
        }

        public void LoadSlide(int slideIndex)
        {
            if (!slideContainer) return;
            if (currentSlideIndex == slideIndex || currentSlideIndex >= slideContainer.childCount) return;

            //Debug.Log("Slide Manager > Loading slide index " + slideIndex);

            // Turn off the current slide
            Slide currentSlide = slideContainer.GetChild(currentSlideIndex).GetComponent<Slide>();
            currentSlide.FadeOut(fadeOutTime, fadeOutDelay);

            // Turn on the requested slide
            Slide nextSlide = slideContainer.GetChild(slideIndex).GetComponent<Slide>();
            nextSlide.FadeIn(fadeInTime, fadeInDelay);

            currentSlideIndex = slideIndex;
        }
    }
}