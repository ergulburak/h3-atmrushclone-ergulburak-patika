using System;
using _Game_.Scripts.Helpers;
using _Game_.Scripts.Input;
using UnityEngine;

namespace _Game_.Scripts
{
  public class GameManager : Singleton<GameManager>
  {
    private bool isInitialize = false;

    private void Awake()
    {
      if (isInitialize) return;
      InputManager.Instance.Initialize();
      Player.Player.Instance.Initialize();

      isInitialize = true;
    }
  }
}
