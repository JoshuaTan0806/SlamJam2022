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
        public MenuHolder statParent = null;
        public System.Action onEquip = null;

        private RectTransform _rt = null;
        private Transform _originalParent = null;
        private Vector2 _pointerOffset = Vector3.zero;
        private ItemSlotUI _prevSlot = null;
        private bool _dragging = false;
        private float _appearTimer = 0f;
        private bool _hover = false;

        private void Start()
        {
            _rt = (RectTransform)transform;
        }

        public void RefreshUI()
        {
            bool hasItem = item != null;
            for (int i = 0; i < connectionObjects.Length; i++)
            {
                ConnectionDirection d = (ConnectionDirection)i;
                connectionObjects[i].gameObject.SetActive(hasItem ? item.possibleConnections.ContainsKey(d) : false);

                if (hasItem && item.possibleConnections.ContainsKey(d))
                    connectionObjects[i].color = ItemIDs.ToColor(item.possibleConnections[d]);
            }

            mainIcon.sprite = hasItem ? ItemBuilder.Instance.GetIcon(item.Type) : null;

            spillIcon.transform.parent.gameObject.SetActive(hasItem ? item.Spill : false);
            spillIcon.sprite = hasItem ? item.Spill.icon : null;

            // Build Stats
            if (hasItem)
            {
                foreach (var stat in item.GetStats().Values)
                    if (stat.FlatValue > 0)
                        statParent.SpawnDescription(stat.InGameName + ": +" + stat.FlatValue.ToString("N0"));
                    else
                        statParent.SpawnDescription(stat.InGameName + ": " + stat.PercentValue.ToString("N0") + "%");
            }

            statParent.gameObject.SetActive(false);
            gameObject.SetActive(hasItem);
        }

        private void Update()
        {
            if (!_hover)
                return;

            _appearTimer += Time.deltaTime;
            bool show = _appearTimer > 0.75f;
            statParent.gameObject.SetActive(show);
            //Once it shows, disable Update
            if (show)
                _hover = false;
        }

        #region DragDrop
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            //Reparent with same position
            transform.SetParent(MasterUIParent.s_masterParent, true);

            gameObject.layer = 2;
            ToggleInteractability(false);
            _hover = false;
            statParent.gameObject.SetActive(false);
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
                onEquip?.Invoke();
                onEquip = null;
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
            statParent.gameObject.SetActive(false);
            _hover = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _appearTimer = 0f;
            _hover = true;
        }
    }
}