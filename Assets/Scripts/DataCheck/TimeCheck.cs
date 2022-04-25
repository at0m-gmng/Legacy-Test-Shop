using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TimeCheck : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _productAlive;
    [Space]
    [SerializeField] private string _startIvent;// = "30-03-2022 07:50:00:AM";
    [SerializeField] private string _endIvent; //= "30-04-2022 07:50:00:AM";

    private void FixedUpdate()
    {
        if ((_startIvent != null) && (_endIvent != null)) 
            ProductAliveTimeCheck();
    }

    private void ProductAliveTimeCheck()
    {
        DateTime startIvent = DateTime.ParseExact(_startIvent, "dd-MM-yyyy HH:mm:ss:tt", CultureInfo.InvariantCulture);
        DateTime endIvent = DateTime.ParseExact(_endIvent, "dd-MM-yyyy HH:mm:ss:tt", CultureInfo.InvariantCulture);
        DateTime now = DateTime.Now;

        TimeSpan diff1 = endIvent - now;
        _productAlive.text = diff1.ToString();
        if (endIvent < now)
        {
            Destroy(gameObject);
        }
    }
}
