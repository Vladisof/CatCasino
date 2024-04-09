using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ROULETTE.Plugins.External_Pack.Simple_Scroll_Snap.Scripts.Runtime
{
    [AddComponentMenu("UI/Simple Scroll-Snap"),RequireComponent(typeof(ScrollRect)),Serializable]
    public class SimpleScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
    #region Fields
        private int _nearestPanel,
            _targetPanel,
            _currentPanel,
            _numberOfToggles;
        private bool _dragging,
            _pressing,
            _selected;
        private float _releaseSpeed,
            _planeDistance;
        private Vector2 _contentSize,
            _previousPosition;
        private Direction _releaseDirection;
        private CanvasGroup _canvasGroup;
        private GameObject[] _panels;
        private Toggle[] _toggles;
        private Graphic[] _graphics;
        private Canvas _canvas;
        private CanvasScaler _canvasScalar;

        public MovementType movementType = MovementType.Fixed;
        public MovementAxis movementAxis = MovementAxis.Horizontal;
        public bool automaticallyLayout = true;
        public SizeControl sizeControl = SizeControl.Fit;
        public Vector2 size = new Vector2(400, 250);
        public float automaticLayoutSpacing = 0.25f;
        public float leftMargin,
            rightMargin,
            topMargin,
            bottomMargin;
        public bool infinitelyScroll;
        public float infiniteScrollingEndSpacing = 0f;
        public int startingPanel = 0;
        public bool swipeGestures = true;
        public float minimumSwipeSpeed = 0f;
        public Button previousButton;
        public Button nextButton;
        public GameObject pagination;
        public bool toggleNavigation = true;
        public SnapTarget snapTarget = SnapTarget.Next;
        public float snappingSpeed = 10f;
        public float thresholdSnappingSpeed = -1f;
        public bool hardSnap = true;
        public UnityEvent onPanelChanged,
            onPanelSelecting,
            onPanelSelected,
            onPanelChanging;
        public List<TransitionEffect> transitionEffects = new List<TransitionEffect>();
    #endregion

    #region Properties
        public int CurrentPanel => _currentPanel;
        public int TargetPanel => _targetPanel;
        public int NearestPanel => _nearestPanel;
        public int NumberOfPanels => Content.childCount;
        public ScrollRect ScrollRect => GetComponent<ScrollRect>();
        public RectTransform Content => GetComponent<ScrollRect>().content;
        public RectTransform Viewport => GetComponent<ScrollRect>().viewport;
        public GameObject[] Panels => _panels;
        public Toggle[] Toggles => _toggles;
    #endregion

    #region Enumerators
        public enum MovementType
        {
            Fixed,
            Free
        }
        public enum MovementAxis
        {
            Horizontal,
            Vertical
        }
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        public enum SnapTarget
        {
            Nearest,
            Previous,
            Next
        }
        public enum SizeControl
        {
            Manual,
            Fit
        }
    #endregion

    #region Methods
        private void Start()
        {
            if (Validate())
            {
                Setup(true);
            } else
            {
                throw new Exception("Invalid configuration.");
            }      
        }
        private void Update()
        {
            if (NumberOfPanels == 0)
                return;

            OnSelectingAndSnapping();
            OnInfiniteScrolling();
            OnTransitionEffects();
            OnSwipeGestures();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (NumberOfPanels == 0)
                return;

            if (swipeGestures)
            {
                _pressing = true;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (NumberOfPanels == 0)
                return;

            if (swipeGestures)
            {
                if (hardSnap)
                {
                    ScrollRect.inertia = true;
                }
                _selected = false;
                _dragging = true;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (NumberOfPanels == 0)
                return;

            if (swipeGestures)
            {
                Vector2 position = eventData.position;
                if (position.x != _previousPosition.x && position.y != _previousPosition.y)
                {
                    if (movementAxis == MovementAxis.Horizontal)
                    {
                        _releaseDirection = (position.x > _previousPosition.x) ? Direction.Right : Direction.Left;
                    }
                    else if (movementAxis == MovementAxis.Vertical)
                    {
                        _releaseDirection = (position.y > _previousPosition.y) ? Direction.Up : Direction.Down;
                    }
                }
                _previousPosition = eventData.position;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (NumberOfPanels == 0) return;

            if (swipeGestures)
            {
                _releaseSpeed = ScrollRect.velocity.magnitude;
                _dragging = false;
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (NumberOfPanels == 0) return;

            if (swipeGestures)
            {
                _pressing = false;
            }
        }

        public bool Validate()
        {
            bool valid = true;

            if (pagination != null)
            {
                _numberOfToggles = pagination.transform.childCount;
                if (_numberOfToggles != NumberOfPanels)
                {
                    Debug.LogError("<b>[SimpleScrollSnap]</b> The number of toggles should be equivalent to the number of panels. There are currently " + _numberOfToggles + " toggles and " + NumberOfPanels + " panels. If you are adding panels dynamically during runtime, please update your pagination to reflect the number of panels you will have before adding.", gameObject);
                    valid = false;
                }
            }

            return valid;
        }
        public void Setup(bool updatePosition)
        {
            if (NumberOfPanels == 0) return;

            // Canvas & Camera
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _canvas.planeDistance = (_canvas.GetComponent<RectTransform>().rect.height / 2f) / Mathf.Tan((_canvas.worldCamera.fieldOfView / 2f) * Mathf.Deg2Rad);
                if (_canvas.worldCamera.farClipPlane < _canvas.planeDistance)
                {
                    _canvas.worldCamera.farClipPlane = Mathf.Ceil(_canvas.planeDistance);
                }
            }

            // ScrollRect
            if (movementType == MovementType.Fixed)
            {
                ScrollRect.horizontal = (movementAxis == MovementAxis.Horizontal);
                ScrollRect.vertical = (movementAxis == MovementAxis.Vertical);
            }
            else
            {
                ScrollRect.horizontal = ScrollRect.vertical = true;
            }

            // Panels
            size = (sizeControl == SizeControl.Manual) ? size : new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
            _panels = new GameObject[NumberOfPanels];
            for (int i = 0; i < NumberOfPanels; i++)
            {
                _panels[i] = ((RectTransform)Content.GetChild(i)).gameObject;

                if (movementType == MovementType.Fixed && automaticallyLayout)
                {
                    _panels[i].GetComponent<RectTransform>().anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); ;
                    _panels[i].GetComponent<RectTransform>().anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); ;

                    float x = (rightMargin + leftMargin) / 2f - leftMargin;
                    float y = (topMargin + bottomMargin) / 2f - bottomMargin;
                    Vector2 marginOffset = new Vector2(x / size.x, y / size.y);
                    _panels[i].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f) + marginOffset;
                    _panels[i].GetComponent<RectTransform>().sizeDelta = size - new Vector2(leftMargin + rightMargin, topMargin + bottomMargin);

                    float panelPosX = (movementAxis == MovementAxis.Horizontal) ? i * (automaticLayoutSpacing + 1f) * size.x + (size.x / 2f) : 0f;
                    float panelPosY = (movementAxis == MovementAxis.Vertical) ? i * (automaticLayoutSpacing + 1f) * size.y + (size.y / 2f) : 0f;
                    _panels[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(panelPosX, panelPosY, 0f);
                }
            }

            // Content
            if (movementType == MovementType.Fixed)
            {
                // Automatic Layout
                if (automaticallyLayout)
                {
                    Content.anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Content.anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Content.pivot = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                    Vector2 min = _panels[0].transform.position;
                    Vector2 max = _panels[NumberOfPanels - 1].transform.position;

                    float contentWidth = (movementAxis == MovementAxis.Horizontal) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.x) - (size.x * automaticLayoutSpacing) : size.x;
                    float contentHeight = (movementAxis == MovementAxis.Vertical) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.y) - (size.y * automaticLayoutSpacing) : size.y;
                    Content.sizeDelta = new Vector2(contentWidth, contentHeight);
                }

                // Infinite Scrolling
                if (infinitelyScroll)
                {
                    ScrollRect.movementType = ScrollRect.MovementType.Unrestricted;

                    _contentSize = ((Vector2)_panels[NumberOfPanels - 1].transform.localPosition - (Vector2)_panels[0].transform.localPosition) + (_panels[NumberOfPanels - 1].GetComponent<RectTransform>().sizeDelta / 2f + _panels[0].GetComponent<RectTransform>().sizeDelta / 2f) + (new Vector2(movementAxis == MovementAxis.Horizontal ? infiniteScrollingEndSpacing * size.x : 0f, movementAxis == MovementAxis.Vertical ? infiniteScrollingEndSpacing * size.y : 0f));

                    if (movementAxis == MovementAxis.Horizontal)
                    {
                        _contentSize += new Vector2(leftMargin + rightMargin, 0);
                    }
                    else
                    {
                        _contentSize += new Vector2(0, topMargin + bottomMargin);
                    }

                    _canvasScalar = _canvas.GetComponent<CanvasScaler>();
                    if (_canvasScalar != null)
                    {
                        _contentSize *= new Vector2(Screen.width / _canvasScalar.referenceResolution.x, Screen.height / _canvasScalar.referenceResolution.y);
                    }
                }
            }

            // Starting Panel
            if (updatePosition)
            {
                float xOffset = (movementAxis == MovementAxis.Horizontal || movementType == MovementType.Free) ? Viewport.GetComponent<RectTransform>().rect.width / 2f : 0f;
                float yOffset = (movementAxis == MovementAxis.Vertical || movementType == MovementType.Free) ? Viewport.GetComponent<RectTransform>().rect.height / 2f : 0f;
                Vector2 offset = new Vector2(xOffset, yOffset);
                Content.anchoredPosition = -(Vector2)_panels[startingPanel].transform.localPosition + offset;
                _currentPanel = _targetPanel = _nearestPanel = startingPanel;
            }

            // Previous Button
            if (previousButton != null)
            {
                previousButton.onClick.AddListener(GoToPreviousPanel);
            }

            // Next Button
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(GoToNextPanel);
            }

            // Pagination
            if (pagination != null)
            {
                _toggles = new Toggle[_numberOfToggles];
                for (int i = 0; i < _numberOfToggles; i++)
                {
                    _toggles[i] = pagination.transform.GetChild(i).GetComponent<Toggle>();
                    if (_toggles[i] != null)
                    {
                        _toggles[i].isOn = (i == startingPanel);
                        _toggles[i].interactable = (i != _targetPanel);
                        int panelNum = i;
                        _toggles[i].onValueChanged.AddListener(delegate
                        {
                            if (_toggles[panelNum].isOn && toggleNavigation)
                            {
                                GoToPanel(panelNum);
                            }
                        });
                    }
                }
            }
        }

        private Vector2 DisplacementFromCenter(Vector2 position)
        {
            return position - (Vector2)Viewport.position;
        }
        private int DetermineNearestPanel()
        {
            int panelNumber = _nearestPanel;
            float[] distances = new float[NumberOfPanels];
            for (int i = 0; i < _panels.Length; i++)
            {
                distances[i] = DisplacementFromCenter(_panels[i].transform.position).magnitude;
            }
            float minDistance = Mathf.Min(distances);
            for (int i = 0; i < _panels.Length; i++)
            {
                if (minDistance == distances[i])
                {
                    panelNumber = i;
                }
            }
            return panelNumber;
        }
        private void SelectTargetPanel()
        {
            //nearestPanel = DetermineNearestPanel();
            if (snapTarget == SnapTarget.Nearest)
            {
                GoToPanel(_nearestPanel);
            }
            else if (snapTarget == SnapTarget.Previous)
            {
                if (_releaseDirection == Direction.Right)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).x < 0f)
                    {
                        GoToNextPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
                else if (_releaseDirection == Direction.Left)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).x > 0f)
                    {
                        GoToPreviousPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
                else if (_releaseDirection == Direction.Up)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).y < 0f)
                    {
                        GoToNextPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
                else if (_releaseDirection == Direction.Down)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).y > 0f)
                    {
                        GoToPreviousPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
            }
            else if (snapTarget == SnapTarget.Next)
            {
                if (_releaseDirection == Direction.Right)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).x > 0f)
                    {
                        GoToPreviousPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
                else if (_releaseDirection == Direction.Left)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).x < 0f)
                    {
                        GoToNextPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
                else if (_releaseDirection == Direction.Up)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).y > 0f)
                    {
                        GoToPreviousPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
                else if (_releaseDirection == Direction.Down)
                {
                    if (DisplacementFromCenter(_panels[_nearestPanel].transform.position).y < 0f)
                    {
                        GoToNextPanel();
                    }
                    else
                    {
                        GoToPanel(_nearestPanel);
                    }
                }
            }
        }
        private void SnapToTargetPanel()
        {
            float xOffset = (movementAxis == MovementAxis.Horizontal || movementType == MovementType.Free) ? Viewport.GetComponent<RectTransform>().rect.width / 2f : 0f;
            float yOffset = (movementAxis == MovementAxis.Vertical || movementType == MovementType.Free) ? Viewport.GetComponent<RectTransform>().rect.height / 2f : 0f;
            Vector2 offset = new Vector2(xOffset, yOffset);

            Vector2 targetPosition = (-(Vector2)_panels[_targetPanel].transform.localPosition + offset);
            Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, targetPosition, Time.unscaledDeltaTime * snappingSpeed);

            if (DisplacementFromCenter(_panels[_targetPanel].transform.position).magnitude < (_panels[_targetPanel].GetComponent<RectTransform>().rect.width / 10f) && _targetPanel != _currentPanel)
            {
                onPanelChanged.Invoke();
                _currentPanel = _targetPanel;
            }
            else if (ScrollRect.velocity != Vector2.zero)
            {
                onPanelChanging.Invoke();
            }
        }

        private void OnSelectingAndSnapping()
        {
            if (!_dragging && !_pressing)
            {
                // Snap/Select after Swiping
                if (_releaseSpeed >= minimumSwipeSpeed || _currentPanel != DetermineNearestPanel())
                {
                    if (ScrollRect.velocity.magnitude <= thresholdSnappingSpeed || thresholdSnappingSpeed == -1f)
                    {
                        if (_selected)
                        {
                            SnapToTargetPanel();
                        }
                        else
                        {
                            SelectTargetPanel();
                        }
                    }
                    else
                    {
                        onPanelSelecting.Invoke();
                    }
                }
                // Snap/Select after Pressing Button/Pagination Toggle
                else
                {
                    if (_selected)
                    {
                        SnapToTargetPanel();
                    }
                    else
                    {
                        GoToPanel(_currentPanel);
                    }
                }
            }
        }
        private void OnInfiniteScrolling()
        {
            if (infinitelyScroll)
            {
                if (movementAxis == MovementAxis.Horizontal)
                {
                    for (int i = 0; i < NumberOfPanels; i++)
                    {
                        float width = _contentSize.x;
                        if (_canvasScalar != null) width *= (_canvas.GetComponent<RectTransform>().localScale.x / (Screen.width / _canvasScalar.referenceResolution.x));

                        if (DisplacementFromCenter(_panels[i].transform.position).x > width / 2f)
                        {
                            _panels[i].transform.position += width * Vector3.left;
                        }
                        else if (DisplacementFromCenter(_panels[i].transform.position).x < -1f * width / 2f)
                        {
                            _panels[i].transform.position += width * Vector3.right;
                        }
                    }
                }
                else if (movementAxis == MovementAxis.Vertical)
                {
                    float height = _contentSize.y;
                    if (_canvasScalar != null) height *= (_canvas.GetComponent<RectTransform>().localScale.y / (Screen.height / _canvasScalar.referenceResolution.y));

                    for (int i = 0; i < NumberOfPanels; i++)
                    {
                        if (DisplacementFromCenter(_panels[i].transform.position).y > height / 2f)
                        {
                            _panels[i].transform.position += height * Vector3.down;
                        }
                        else if (DisplacementFromCenter(_panels[i].transform.position).y < -1f * height / 2f)
                        {
                            _panels[i].transform.position += height * Vector3.up;
                        }
                    }
                }
            }
        }
        private void OnTransitionEffects()
        {
            foreach (GameObject panel in _panels)
            {
                foreach (TransitionEffect transitionEffect in transitionEffects)
                {
                    // Displacement
                    float displacement = 0f;
                    if (movementType == MovementType.Fixed)
                    {
                        if (movementAxis == MovementAxis.Horizontal)
                        {
                            displacement = DisplacementFromCenter(panel.transform.position).x;
                        }
                        else if (movementAxis == MovementAxis.Vertical)
                        {
                            displacement = DisplacementFromCenter(panel.transform.position).y;
                        }
                    }
                    else
                    {
                        displacement = DisplacementFromCenter(panel.transform.position).magnitude;
                    }

                    // Value
                    switch (transitionEffect.Label)
                    {
                        case "localPosition.z":
                            panel.transform.localPosition = new Vector3(panel.transform.localPosition.x, panel.transform.localPosition.y, transitionEffect.GetValue(displacement));
                            break;
                        case "localScale.x":
                            panel.transform.localScale = new Vector2(transitionEffect.GetValue(displacement), panel.transform.localScale.y);
                            break;
                        case "localScale.y":
                            panel.transform.localScale = new Vector2(panel.transform.localScale.x, transitionEffect.GetValue(displacement));
                            break;
                        case "localRotation.x":
                            panel.transform.localRotation = Quaternion.Euler(new Vector3(transitionEffect.GetValue(displacement), panel.transform.localEulerAngles.y, panel.transform.localEulerAngles.z));
                            break;
                        case "localRotation.y":
                            panel.transform.localRotation = Quaternion.Euler(new Vector3(panel.transform.localEulerAngles.x, transitionEffect.GetValue(displacement), panel.transform.localEulerAngles.z));
                            break;
                        case "localRotation.z":
                            panel.transform.localRotation = Quaternion.Euler(new Vector3(panel.transform.localEulerAngles.x, panel.transform.localEulerAngles.y, transitionEffect.GetValue(displacement)));
                            break;
                        case "color.r":
                            _graphics = panel.GetComponentsInChildren<Graphic>();
                            foreach (Graphic graphic in _graphics)
                            {
                                graphic.color = new Color(transitionEffect.GetValue(displacement), graphic.color.g, graphic.color.b, graphic.color.a);
                            }
                            break;
                        case "color.g":
                            _graphics = panel.GetComponentsInChildren<Graphic>();
                            foreach (Graphic graphic in _graphics)
                            {
                                graphic.color = new Color(graphic.color.r, transitionEffect.GetValue(displacement), graphic.color.b, graphic.color.a);
                            }
                            break;
                        case "color.b":
                            _graphics = panel.GetComponentsInChildren<Graphic>();
                            foreach (Graphic graphic in _graphics)
                            {
                                graphic.color = new Color(graphic.color.r, graphic.color.g, transitionEffect.GetValue(displacement), graphic.color.a);
                            }
                            break;
                        case "color.a":
                            _graphics = panel.GetComponentsInChildren<Graphic>();
                            foreach (Graphic graphic in _graphics)
                            {
                                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, transitionEffect.GetValue(displacement));
                            }
                            break;
                    }
                }
            }
        }
        private void OnSwipeGestures()
        {
            if (swipeGestures == false && (Input.GetMouseButton(0) || Input.touchCount > 0))
            {
                // Set to False.
                ScrollRect.horizontal = ScrollRect.vertical = false;
            }
            else
            {
                // Reset.
                if (movementType == MovementType.Fixed)
                {
                    ScrollRect.horizontal = (movementAxis == MovementAxis.Horizontal);
                    ScrollRect.vertical = (movementAxis == MovementAxis.Vertical);
                }
                else
                {
                    ScrollRect.horizontal = ScrollRect.vertical = true;
                }
            }
        }

        public void GoToPanel(int panelNumber)
        {
            _targetPanel = panelNumber;
            _selected = true;
            onPanelSelected.Invoke();

            if (pagination != null)
            {
                for (int i = 0; i < _toggles.Length; i++)
                {
                    if (_toggles[i] != null)
                    {
                        _toggles[i].isOn = (i == _targetPanel);
                        _toggles[i].interactable = (i != _targetPanel);
                    }
                }
            }

            if (hardSnap)
            {
                ScrollRect.inertia = false;
            }
        }
        public void GoToPreviousPanel()
        {
            _nearestPanel = DetermineNearestPanel();
            if (_nearestPanel != 0)
            {
                GoToPanel(_nearestPanel - 1);
            }
            else
            {
                if (infinitelyScroll)
                {
                    GoToPanel(NumberOfPanels - 1);
                }
                else
                {
                    GoToPanel(_nearestPanel);
                }
            }
        }
        public void GoToNextPanel()
        {
            _nearestPanel = DetermineNearestPanel();
            if (_nearestPanel != (NumberOfPanels - 1))
            {
                GoToPanel(_nearestPanel + 1);
            }
            else
            {
                if (infinitelyScroll)
                {
                    GoToPanel(0);
                }
                else
                {
                    GoToPanel(_nearestPanel);
                }
            }
        }

        public void AddToFront(GameObject panel)
        {
            Add(panel, 0);
        }
        public void AddToBack(GameObject panel)
        {
            Add(panel, NumberOfPanels);
        }
        public void Add(GameObject panel, int index)
        {
            if (NumberOfPanels != 0 && (index < 0 || index > NumberOfPanels))
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + NumberOfPanels + ".", gameObject);
                return;
            }

            panel = Instantiate(panel, Vector2.zero, Quaternion.identity, Content);
            panel.transform.SetSiblingIndex(index);

            if (Validate())
            {
                if (_targetPanel <= index)
                {
                    startingPanel = _targetPanel;                 
                }
                else
                {
                    startingPanel = _targetPanel + 1;
                }
                Setup(true);
            }
        }

        public void RemoveFromFront()
        {
            Remove(0);
        }
        public void RemoveFromBack()
        {
            if (NumberOfPanels > 0)
            {
                Remove(NumberOfPanels - 1);
            }
            else
            {
                Remove(0);
            }
        }
        public void Remove(int index)
        {
            if (NumberOfPanels == 0) return;

            if (index < 0 || index > (NumberOfPanels - 1))
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + (NumberOfPanels - 1) + ".", gameObject);
                return;
            }

            DestroyImmediate(_panels[index]);

            if (Validate())
            {
                if (_targetPanel == index)
                {
                    if (index == NumberOfPanels)
                    {
                        startingPanel = _targetPanel - 1;
                    }
                    else
                    {
                        startingPanel = _targetPanel;
                    }
                }
                else if (_targetPanel < index)
                {
                    startingPanel = _targetPanel;
                }
                else
                {
                    startingPanel = _targetPanel - 1;
                }
                Setup(true);
            }
        }

        public void AddVelocity(Vector2 velocity)
        {
            ScrollRect.velocity += velocity;
            _selected = false;
        }
    #endregion
    }
}