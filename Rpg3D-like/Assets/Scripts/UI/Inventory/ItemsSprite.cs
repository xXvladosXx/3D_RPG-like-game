using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSprite : MonoBehaviour
{
    public static ItemsSprite Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] public Transform prefabItemTransform;
    
    public Sprite SwordSprite;
    public Sprite BowSprite;
    public Sprite HealthPotionSprite;
    public Sprite ManaPotionSprite;
    public Sprite GoldSprite;
}
