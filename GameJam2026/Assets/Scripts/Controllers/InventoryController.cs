using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;

    private void Start()
    {
        _inventory.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _inventory.SetActive(!_inventory.gameObject.activeSelf);
        }
    }
}
