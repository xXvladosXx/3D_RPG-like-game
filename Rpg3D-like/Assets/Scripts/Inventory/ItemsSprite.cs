using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventories
{
    public class ItemsSprite : MonoBehaviour
    {
        public static ItemsSprite Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public Sprite SwordSprite0;
        public Sprite SwordSprite1;
        public Sprite SwordSprite2;
        public Sprite SwordSprite3;
        public Sprite SwordSprite4;

        public Sprite BowSprite0;
        public Sprite BowSprite1;
        public Sprite BowSprite2;
        public Sprite BowSprite3;
        public Sprite BowSprite4;

        public Sprite HealthPotionSprite;
        public Sprite ManaPotionSprite;
    }

}
