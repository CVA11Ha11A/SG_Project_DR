using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 날짜 : 2021-03-28 PM 10:42:48
// 작성자 : Rito

namespace Rito.InventorySystem
{
    /// <summary> 소비 아이템 정보 </summary>
    [CreateAssetMenu(fileName = "Item_Bomb_", menuName = "Inventory System/Item Data/Bomb", order = 5)]
    public class BombItemData : CountableItemData
    {
        /// <summary> 효과량(회복량 등) </summary>
        public float Value => _value;
        public float Duration => _duration;
        public float Radius => _radius;
        [SerializeField] public float _value;
        [SerializeField] public float _duration;
        [SerializeField] public float _radius;
        public override Item CreateItem()
        {
            return new BombItem(this);
        }
    }
}