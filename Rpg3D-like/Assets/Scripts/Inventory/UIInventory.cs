using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
   
   [SerializeField] private Transform _itemContainerTemplate;
   
   private Inventory _inventory;
   private Transform _itemContainer;
   private TextMeshPro _itemAmount;

   private void Awake()
   {
      _itemContainer = transform.Find("ItemContainer");
   }

   public void SetInventory(Inventory inventory)
   {
      _inventory = inventory;
      
      inventory.OnInventoryChanged += InventoryOnOnInventoryChanged;
      RefreshInventory();
   }

   private void InventoryOnOnInventoryChanged(object sender, EventArgs e)
   {
      RefreshInventory();
   }

   private void RefreshInventory()
   {
      if(_itemContainer == null) return;

      foreach (Transform child in _itemContainer)
      {
         if(child == _itemContainerTemplate) continue;
         
         Destroy(child.gameObject);
      }
      
      int x = 0;
      int y = 0;
      float itemContainerSize = 30f;
      
      foreach (var item in _inventory.GetInventory)
      {
         RectTransform itemContainerTransform =
            Instantiate(_itemContainerTemplate, _itemContainer).GetComponent<RectTransform>();
         itemContainerTransform.gameObject.SetActive(true);
         itemContainerTransform.anchoredPosition = new Vector2(x * itemContainerSize, y * itemContainerSize);
         x++;

       /*  itemContainerTransform.GetComponent<Button_UI>().ClickFunc = () =>
         {
            _inventory.RemoveItem(item);

            _playerPosition.gameObject.GetComponent<PlayerInventory>().DropItem(item, _playerPosition.position);
         };*/

         itemContainerTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
         {
            _inventory.UseItem(item);
         };
         
         Image itemImage = itemContainerTransform.Find("Image").GetComponent<Image>();
         TextMeshProUGUI itemAmount = itemContainerTransform.Find("Amount").GetComponent<TextMeshProUGUI>();
         
         itemImage.sprite = item.GetItemSprite();
         itemAmount.text = item.GetItemAmount();

         if (x >= 3)
         {
            x = 0;
            y--;
         }
      }
   }

 
}
