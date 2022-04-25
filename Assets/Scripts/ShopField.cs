using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopField : MonoBehaviour
{
    [SerializeField] private ProductCard _tilePrefab; 
    [SerializeField] private ProductCard _tilePrefabTemporary;
    [SerializeField] private TextMeshProUGUI _coinField;
    [SerializeField] private TextMeshProUGUI _brilliantField;
    
    private int _size; // размер поля
    private ProductCard[] _tiles; // хранит созданные tile
    private ProductCardFactory _productCardFactory;
    private GridLayoutGroup _gridLayoutGroup;

    public PlayerPrefsSafe Save = null;
    public PlayerPrefsSafe.ProductTable SaveTable = null;


    private void Start()
    {        
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.cellSize = _tilePrefab.GetComponent<RectTransform>().sizeDelta;
        transform.localPosition = new Vector3(0f, -(_size * _gridLayoutGroup.cellSize.y), 0f); // устанавливаем начало поля отображения к 1ому элементу

        SetCurrencyUI();
        GameUpdate();
    }

    public void Init(int size, ProductCardFactory contentFactory)
    {
        _size = size;
        _productCardFactory = contentFactory;
        _tiles = new ProductCard[size + Shop.Instance.TemporalyProductCard]; // инициализируем массив по размеру поля

        Save = FindObjectOfType<PlayerPrefsSafe>();
        SaveTable = Save.LoadTableOrDefault();
        
        if (SaveTable.ProductList.Count == 0) // если данных нет, загружаем рандомные для тестов, после чего сохраняем в PP
            LoadPlayerPrefs(_productCardFactory, Save);

        for (int i = 0; i < _tiles.Length; i++)
        {
            ProductCard tile = _tiles[i] = Instantiate(_tilePrefab);
            if (i < Shop.Instance.TemporalyProductCard)
            {
                tile = _tiles[i] = Instantiate(_tilePrefabTemporary);
                    // _productCardFactory.Get(ProductCardType.Temporary);
            }
            SetObject(_tiles[i], i, SaveTable);
        }
    }

    public void LoadPlayerPrefs(ProductCardFactory productCardFactory, PlayerPrefsSafe product)
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            product.AddHightScoreAndSave(
                productCardFactory.ProductDatas[i].ProductName,
                productCardFactory.ProductDatas[i].ProductImage,
                productCardFactory.ProductDatas[i].ProductCoinPrice,
                productCardFactory.ProductDatas[i].ProductBrilliantPrice, 
                !productCardFactory.ProductDatas[i].IsPurchased);
        }

        SaveTable = product.LoadTableOrDefault();
    }

    private void SetObject(ProductCard tile, int i, PlayerPrefsSafe.ProductTable Save)
    {                 
        tile.transform.SetParent(transform,false); //  делаем её дочерней у поля
        tile.transform.name = "Product " + i;

        if(_productCardFactory.ProductDatas[i].ProductCoinPrice == 0)
            _productCardFactory.ProductDatas[i].ProductCoinPrice = Random.Range(100, 1000);
        if(_productCardFactory.ProductDatas[i].ProductBrilliantPrice == 0)
            _productCardFactory.ProductDatas[i].ProductBrilliantPrice = Random.Range(100, 1000);

        var coinP = Save.ProductList[i].ProductCoinPrice; 
            // _productCardFactory.ProductDatas[i].ProductCoinPrice ;
        var BrilliantP = Save.ProductList[i].ProductBrilliantPrice; 
            // _productCardFactory.ProductDatas[i].ProductBrilliantPrice;
        tile._coinPrice.GetComponentInChildren<TextMeshProUGUI>().text = coinP.ToString(); //_productCardFactory.ProductDatas[i].ProductCoinPrice.ToString();;
        tile._brilliantPrice.GetComponentInChildren<TextMeshProUGUI>().text = BrilliantP.ToString();   //_productCardFactory.ProductDatas[i].ProductCoinPrice.ToString();
        tile._coinPrice.interactable = tile._brilliantPrice.interactable = Save.ProductList[i].IsPurchased; 
            // !_productCardFactory.ProductDatas[i].IsPurchased;
        // tile._brilliantPrice.interactable = !_productCardFactory.ProductDatas[i].IsPurchased;
        tile._productImage =Save.ProductList[i].ProductImage; 
            // _productCardFactory.ProductDatas[i].ProductImage;
            tile._productName.text = Save.ProductList[i].ProductName; 
            // _productCardFactory.ProductDatas[i].ProductName;
        tile._coinPrice.AddEventListener(i, OnShopItemBtnClickedCoin);
        tile._brilliantPrice.AddEventListener(i, OnShopItemBtnClickedBrilliant);
    }
    private void SetCurrencyUI()
    {
        _coinField.text = Shop.Instance.Coins.ToString();
        _brilliantField.text = Shop.Instance.Brilliant.ToString();
    }
    private void GameUpdate()
    {
        SaveTable = Save.LoadTableOrDefault();
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i]._coinPrice.interactable = SaveTable.ProductList[i].IsPurchased;
            // !_productCardFactory.ProductDatas[i].IsPurchased;    
            _tiles[i]._brilliantPrice.interactable = SaveTable.ProductList[i].IsPurchased;
            // !_productCardFactory.ProductDatas[i].IsPurchased;

            if (_tiles[i]._brilliantPrice.interactable == false || _tiles[i]._coinPrice.interactable == false)
                _tiles[i]._lockWindow.SetActive(false);
        }

    }

    public void OnShopItemBtnClickedCoin(int index)
    {
        if (Shop.Instance.HasEnoughCurrencyCoins(_productCardFactory.ProductDatas[index].ProductCoinPrice))
        {
            Shop.Instance.UseCoinCurrency(_productCardFactory.ProductDatas[index].ProductCoinPrice);
            
            SaveTable.ProductList[index].IsPurchased = false;
            Save.AddHightScoreAndSave(SaveTable.ProductList[index].ProductName,
                SaveTable.ProductList[index].ProductImage,
                SaveTable.ProductList[index].ProductCoinPrice,
                SaveTable.ProductList[index].ProductBrilliantPrice, 
                SaveTable.ProductList[index].IsPurchased);
            SaveTable = Save.LoadTableOrDefault();
        }
        GameUpdate();
        SetCurrencyUI();

    }    
    public void OnShopItemBtnClickedBrilliant(int index)
    {
        if(Shop.Instance.HasEnoughCurrencyBrilliant(_productCardFactory.ProductDatas[index].ProductBrilliantPrice))
        {
            Shop.Instance.UseBrilliantCurrency(_productCardFactory.ProductDatas[index].ProductBrilliantPrice);
            
            SaveTable.ProductList[index].IsPurchased = false;
            Save.AddHightScoreAndSave(SaveTable.ProductList[index].ProductName,
                SaveTable.ProductList[index].ProductImage,
                SaveTable.ProductList[index].ProductCoinPrice,
                SaveTable.ProductList[index].ProductBrilliantPrice, 
                SaveTable.ProductList[index].IsPurchased);
            SaveTable = Save.LoadTableOrDefault();
        }
        GameUpdate();
        SetCurrencyUI();
    }
}
