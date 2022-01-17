using System;
using _Game_.Scripts.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace _Game_.Scripts.Input
{
  public class InputManager : Singleton<InputManager>
  {
    private TouchControls _touchControls;
    public TouchEventArgs TouchEventArgs { get; private set; }

    private bool _touchStarted;

    public void Initialize()
    {
      EnhancedTouchSupport.Enable();
      TouchSimulation.Enable();

      Touch.onFingerDown += FingerDown;
      Touch.onFingerMove += FingerTouching;
      Touch.onFingerUp += FingerUp;
    }

    private void OnDisable()
    {
      Touch.onFingerDown -= FingerDown;
      Touch.onFingerMove -= FingerTouching;
      Touch.onFingerUp -= FingerUp;

      TouchSimulation.Disable();
      EnhancedTouchSupport.Disable();
    }

    public event EventHandler<TouchEventArgs> OnStartTouch;
    public event EventHandler<TouchEventArgs> OnEndTouch;
    public event EventHandler<TouchEventArgs> OnPerformingTouch;

    #region Touch System

    public void FingerDown(Finger finger)
    {
      if (finger.index != 0) return;

      _touchStarted = true;

      TouchEventArgs =
        new TouchEventArgs(
          finger.screenPosition,
          Time.time);

      OnStartTouch?.Invoke(this, TouchEventArgs);
    }

    public void FingerTouching(Finger finger)
    {
      if (_touchStarted == false) return;
      if (finger.index != 0) return;

      TouchEventArgs =
        new TouchEventArgs(
          finger.screenPosition,
          Time.time);

      OnPerformingTouch?.Invoke(this, TouchEventArgs);
    }

    public void FingerUp(Finger finger)
    {
      if (_touchStarted == false) return;
      if (finger.index != 0) return;

      TouchEventArgs =
        new TouchEventArgs(
          finger.screenPosition,
          Time.time);


      OnEndTouch?.Invoke(this, TouchEventArgs);
    }

    #endregion
  }
}
