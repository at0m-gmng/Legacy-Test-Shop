using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductCard : MonoBehaviour
{
    public GameObject _lockWindow;
    public Button _coinPrice;
    public Button _brilliantPrice;
    public TextMeshProUGUI _productName;
    public Sprite _productImage;
    public bool IsPurchased;
    private int _productCoinPrice;
    private int _productBrilliantPrice;

}
