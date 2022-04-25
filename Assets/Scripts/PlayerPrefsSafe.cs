using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPrefsSafe : MonoBehaviour
{
    private const int MaxScoresInTable = 100; // максимальное кол-во рекордов в таблице
    private const string ScoreTableSaveKey = "ProductTable.saveData"; // имя для сохранения в PlayerPrefs

    [Space]
    [Header("DebugSaveData")]
    [SerializeField]
    private ProductTable _productTable; // отображение таблицы в инспектрое для дебага

    public void Awake() 
    {
        LoadTableOrDefault(); // загрузили таблицу, либо создали пустую, если сохранений не было
        if (_productTable.IsEmpty) {
            AddHightScoreAndSave("Tom", null, 666,666, true);
        }
    }
   
    public void AddHightScoreAndSave(string productName, Sprite productImage, int productCoinPrice, int productBrilliantPrice, bool isPurchased) 
    { 
        _productTable.AddScore( productName,  productImage,  productCoinPrice,  productBrilliantPrice,  isPurchased);
        SaveTable();
    }
    public void SaveTable() { // сохранение таблицы в PlayerPrefs 
        SortAndCutTable();
        PlayerPrefs.SetString(ScoreTableSaveKey, JsonUtility.ToJson(_productTable));
    }
    public ProductTable LoadTableOrDefault() { // загружка таблицы из PlayerPrefs  
        _productTable = JsonUtility.FromJson<ProductTable>(PlayerPrefs.GetString(ScoreTableSaveKey));
        _productTable = _productTable ?? new ProductTable(); //-> если сохранения не было и вернуло NULL, то создаём новую таблицу
        return _productTable;
    }
    private void SortAndCutTable() { // корректировка таблицы
        _productTable.ProductList = _productTable.ProductList
                                                        .OrderByDescending(i => i.ProductName) //-> сортировка от большего к меньшему
                                                        .Take(MaxScoresInTable) //-> оставляем только нужное кол-во рекордов
                                                        .ToList();
    }

    [System.Serializable]
    public class ProductTable {
        public List<ProductData> ProductList = new List<ProductData>();
        public bool IsEmpty => ProductList.Count == 0;
        public void AddScore(string productName, Sprite productImage, int productCoinPrice, int productBrilliantPrice, bool isPurchased) 
        {
            ProductList.Add(new ProductData( productName,  productImage,  productCoinPrice,  productBrilliantPrice,  isPurchased));
        }
    }

    [System.Serializable]
    public class ProductData
    {
        public string ProductName;
        public Sprite ProductImage;
        public int ProductCoinPrice;
        public int ProductBrilliantPrice;
        public bool IsPurchased;
    
        public ProductData(string productName, Sprite productImage, int productCoinPrice, int productBrilliantPrice, bool isPurchased) 
        {
            ProductName = productName;
            ProductImage = productImage;
            ProductCoinPrice = productCoinPrice;
            ProductBrilliantPrice = productBrilliantPrice;
            IsPurchased = isPurchased;
        }
    }
}
