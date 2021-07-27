using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsSprite : MonoBehaviour
{
    private Lookup<string, int> _itemLevels;
    public static ItemsSprite Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite SwordSprite;
    public Sprite SwordSprite1;
    public Sprite BowSprite;
    public Sprite HealthPotionSprite;
    public Sprite ManaPotionSprite;
    public Sprite GoldSprite;
}
