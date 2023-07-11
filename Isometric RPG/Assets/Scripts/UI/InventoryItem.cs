using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Item item;

    [Header("UI")]
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;

    public void initItem(Item newItem) {
        item  =  newItem;
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData) {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
