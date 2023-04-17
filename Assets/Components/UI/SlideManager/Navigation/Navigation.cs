using UnityEngine;
using UnityEngine.UI;

namespace Slides
{
    public class Navigation : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private GameObject backButton = default;
        [SerializeField] private GameObject forwardButton = default;

        [Header("Bubbles")]
        [SerializeField] private Sprite filledDisk = default;
        [SerializeField] private Sprite openCircle = default;
        [SerializeField] private CustomCursor customCursor;
        [SerializeField] private Color pastColor = Color.black;
        [SerializeField] private Color currentColor = Color.red;
        [SerializeField] private Color futureColor = Color.white;
        [SerializeField] private bool bubblesAreClickable = false;

        [Header("Progress Bar")]
        [SerializeField] private ProgressBar progressBar;
        [HideInInspector] public bool useProgressBar;

        private RectTransform bubbles;
        private int numSlides;
        private int currentSlideIndex;

        public static event System.Action<int> OnChangeSlide;

        private void OnEnable()
        {
            NavBubble.OnClick += HandleBubbleClicked;
        }

        private void OnDisable()
        {
            NavBubble.OnClick -= HandleBubbleClicked;
        }

        public void SetNumSlides(int numSlides)
        {
            this.numSlides = numSlides;
        }

        public void GenerateBubbles(int numSlides)
        {
            bubbles = (RectTransform)transform.Find("Bubbles");
            if (bubbles == null)
            {
                Debug.LogWarning("Navigation could not find a child GameObject called Bubbles");
                return;
            }

            for (int i = 0; i < numSlides; i++)
            {
                RectTransform bubbleTransform = new GameObject("Bubble" + i, typeof(Image)).GetComponent<RectTransform>();
                bubbleTransform.gameObject.AddComponent<NavBubble>();
                var cursorHover = bubbleTransform.gameObject.AddComponent<CursorHoverUI>();
                cursorHover.SetCustomCursor(customCursor);
                Image bubbleImage = bubbleTransform.GetComponent<Image>();
                bubbleTransform.SetParent(bubbles);
                bubbleTransform.anchoredPosition = Vector2.zero;
                bubbleTransform.sizeDelta = bubbles.sizeDelta.y * Vector2.one;
                bubbleTransform.localScale = Vector3.one;
                bubbleImage.color -= new Color(0, 0, 0, bubbleImage.color.a);
                bubbleImage.preserveAspect = true;
            }

            SetNumSlides(numSlides);
        }

        private void SetActiveBubble(int slideIndex)
        {
            if (bubbles == null || filledDisk == null || openCircle == null)
            {
                Debug.LogWarning("Navigation > must assign Filled Disk and Open Circle sprites.");
                return;
            }

            for (int i = 0; i < bubbles.childCount; i++)
            {
                Image image = bubbles.GetChild(i).GetComponent<Image>();
                if (i < slideIndex)
                {
                    image.sprite = filledDisk;
                    image.color = pastColor;
                }
                else if (i == slideIndex)
                {
                    image.sprite = filledDisk;
                    image.color = currentColor;
                }
                else
                {
                    image.sprite = openCircle;
                    image.color = futureColor;
                }
            }
        }

        private void SetButtonVisibility()
        {
            if (backButton == null || forwardButton == null)
            {
                Debug.LogWarning("No navigation arrows assigned.");
                return;
            }

            if (currentSlideIndex == 0)
            {
                backButton.SetActive(false);
                forwardButton.SetActive(numSlides > 1);
            }
            else if (currentSlideIndex == numSlides - 1)
            {
                backButton.SetActive(true);
                forwardButton.SetActive(false);
            }
            else
            {
                backButton.SetActive(true);
                forwardButton.SetActive(true);
            }
        }

        public void ChangeSlide(int slideIndex, bool sendMessage = true)
        {
            if (useProgressBar)
            {
                if (progressBar) progressBar.FillProgressBar(slideIndex, numSlides);
            }
            else
            {
                SetActiveBubble(slideIndex);
            }

            SetButtonVisibility();

            if (sendMessage) OnChangeSlide?.Invoke(slideIndex);
        }

        public void GoBack()
        {
            currentSlideIndex--;
            ChangeSlide(currentSlideIndex);
        }

        public void GoForward()
        {
            currentSlideIndex++;
            ChangeSlide(currentSlideIndex);
        }

        public void SetBubbleClickability(bool clickable)
        {
            bubblesAreClickable = clickable;
        }

        public void HandleBubbleClicked(int slideIndex)
        {
            if (!bubblesAreClickable) { return; }

            if (currentSlideIndex != slideIndex)
            {
                currentSlideIndex = slideIndex;
                ChangeSlide(currentSlideIndex);
            }
        }

        public void SetCurrentSlideIndex(int slideIndex)
        {
            currentSlideIndex = slideIndex;
        }

        public void HideProgressBar()
        {
            if (progressBar) progressBar.gameObject.SetActive(false);
        }
    }
}