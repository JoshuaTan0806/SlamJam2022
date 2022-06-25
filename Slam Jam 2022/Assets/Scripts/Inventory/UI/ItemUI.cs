using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Items;

namespace Items.UI
{
    public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector]
        public ItemData item = null;

        [Header("UI")]
        public Image mainIcon = null;
        public Image spillIcon = null;
        [Tooltip("Should be in order N, E, S, W")]
        public Image[] connectionObjects = new Image[4];
        public GameObject statParent = null;

        private RectTransform _rt = null;
        private Transform _originalParent = null;
        private Vector2 _pointerOffset = Vector3.zero;
        private ItemSlotUI _prevSlot = null;
        private bool _dragging = false;
        private float _appearTimer = 0f;

        private void Start()
        {
            _rt = (RectTransform)transform;
            enabled = false;
        }

        public void RefreshUI()
        {
            bool hasItem = item != null;
            for (int i = 0; i < connectionObjects.Length; i++)
            {
                connectionObjects[i].gameObject.SetActive(hasItem);
                connectionObjects[i].color = ItemIDs.ToColor((ConnectionType)i);
            }

            mainIcon.sprite = hasItem ? ItemBuilder.Instance.GetIcon(item.Type) : null;

            spillIcon.transform.parent.gameObject.SetActive(hasItem);
            spillIcon.sprite = hasItem ? item.Spill.icon : null;

            // Build Stats

            statParent.SetActive(false);
        }

        private void Update()
        {
            _appearTimer += Time.deltaTime;
            bool show = _appearTimer > 0.75f;
            statParent.SetActive(show);
            //Once it shows, disable Update
            if (show)
                enabled = false;
        }

        #region DragDrop
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            //Reparent with same position
            transform.SetParent(MasterUIParent.s_masterParent, true);

            gameObject.layer = 2;
            ToggleInteractability(false);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {   //Apply mouse drag
            _rt.position = eventData.position + _pointerOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject obj = eventData.pointerCurrentRaycast.gameObject;
            //Get the ItemSlotUI
            ItemSlotUI slot;
            if (!obj || (slot = obj.GetComponent<ItemSlotUI>()) == null)
                //Return to original parent
                transform.SetParent(_originalParent);
            else
            {   //New parent
                slot.SetSlot(this);
                transform.SetParent(slot.transform);

                if (_prevSlot)
                    _prevSlot.SetSlot(null);

                _prevSlot = slot;
            }


            transform.localPosition = Vector3.zero;
            _pointerOffset = Vector2.zero;
            gameObject.layer = 5;
            ToggleInteractability(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_dragging)
            {
                _pointerOffset = (Vector2)_rt.position - eventData.position;
                _dragging = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _dragging = false;
        }

        private void ToggleInteractability(bool enabled)
        {
            //foreach (var ui in GetComponentsInChildren<Graphic>())
            //    ui.raycastTarget = enabled;
            GetComponent<Graphic>().raycastTarget = enabled;
        }
        #endregion
        public void OnPointerExit(PointerEventData eventData)
        {
            statParent.SetActive(false);
            enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _appearTimer = 0f;
            enabled = true;
        }
    }
}