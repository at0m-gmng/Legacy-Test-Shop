using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;
    
    [SerializeField] private ProductCardFactory _productCardFactory; 
    [SerializeField] private ShopField _shopField; 
    [SerializeField] private int _size; // размер поля
    [SerializeField] private int _temporalyProductCard; // количество временных ячеек

    public int TemporalyProductCard => _temporalyProductCard;
    public int Coins ;
    public int Brilliant;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Coins = PlayerPrefs.GetInt("Coins", Coins);
        Brilliant = PlayerPrefs.GetInt("Brilliants", Brilliant);
        _shopField.Init(_size, _productCardFactory);
    }

    public void UseCoinCurrency(int amount)
    {
        Coins -= amount;
        PlayerPrefs.SetInt("Coins", Coins);
    }
    public void UseBrilliantCurrency(int amount)
    {
        Brilliant -= amount;
        PlayerPrefs.SetInt("Brilliants", Brilliant);
    }

    public bool HasEnoughCurrencyCoins(int amount)
    {
        return (Coins >= amount);
    }
    public bool HasEnoughCurrencyBrilliant(int amount)
    {
        return (Brilliant >= amount);
    }
}
