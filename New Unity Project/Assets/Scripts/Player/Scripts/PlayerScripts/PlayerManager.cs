using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager _PlayerManager;


    public PlayerMovement _playerMovement;

    private void Awake()
    {
        if (_PlayerManager == null)
            _PlayerManager = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
            
    }

    private void Start()
    {
        _playerMovement = GetComponentInChildren<PlayerMovement>();

    }
}
