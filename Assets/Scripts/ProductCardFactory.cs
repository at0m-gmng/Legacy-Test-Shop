using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProductCardFactory : ScriptableObject
{
    [System.Serializable]
    public class ProductData
    {
        public string ProductName;
        public Sprite ProductImage;
        public int ProductCoinPrice;
        public int ProductBrilliantPrice;
        public bool IsPurchased;

    }

    [SerializeField] public List<ProductData> ProductDatas;
}
