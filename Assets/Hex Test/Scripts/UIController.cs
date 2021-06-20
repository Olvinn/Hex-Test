using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text _money;

    public void SetInfo(string txt)
    {
        _money.text = txt;
    }
}
