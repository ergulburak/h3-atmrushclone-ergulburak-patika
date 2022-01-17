using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game_.Scripts.Input;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _Game_.Scripts.Player
{
  public class Player : MovementController
  {
    [FoldoutGroup("Stack Settings")] [SerializeField]
    private Transform playerPivot;

    [FoldoutGroup("Stack Settings")] [SerializeField] [Range(0f, 2f)]
    private float offsetMultiplier;

    [FoldoutGroup("Stack Settings")] [SerializeField] [Range(0f, 2f)]
    private float stackFollowSpeed;

    [FoldoutGroup("Stack Settings")] [SerializeField] [Range(0f, 1f)]
    private float pulseDuration;

    [FoldoutGroup("Stack Settings")] [SerializeField] [Range(0f, 2f)]
    private float pulseScale;

    [FoldoutGroup("Stack Settings")] [SerializeField] [Range(0f, 2f)]
    private float pulseDelay;

    private bool onFirstTouch = true;
    private bool playerCanMove = false;

    [SerializeField] private List<GameObject> collectables = new List<GameObject>();

    public override void Initialize()
    {
      InputManager.Instance.OnStartTouch += MoveStarted;
      InputManager.Instance.OnPerformingTouch += MovePerforming;
      InputManager.Instance.OnEndTouch += MoveReleased;
    }

    private void OnFirstTouch()
    {
      onFirstTouch = false;
      playerCanMove = true;
    }

    private void Update()
    {
      if (playerCanMove)
      {
        MoveForward(transform);
        if (collectables.Count > 0)
        {
          WatchTheFront();
        }
      }
    }

    private void MoveStarted(object sender, TouchEventArgs args)
    {
      if (onFirstTouch)
        OnFirstTouch();
      _touchEventArgs = args;
    }

    private void MovePerforming(object sender, TouchEventArgs args)
    {
      if (!playerCanMove) return;
      MoveHorizontal(args, playerPivot);
    }

    private void MoveReleased(object sender, TouchEventArgs args)
    {
    }

    private void AddCollectable(GameObject collectable)
    {
      if (collectables.Contains(collectable)) return;
      collectables.Add(collectable);
      StartCoroutine(nameof(StackPulse));
      collectable.transform.SetParent(transform);
      collectable.transform.localPosition = collectable.Equals(collectables.First())
        ? new Vector3(playerPivot.localPosition.x, 0, collectables.Count * offsetMultiplier)
        : new Vector3(collectables[collectables.IndexOf(collectable) - 1].transform.localPosition.x, 0,
          collectables.Count * offsetMultiplier);
    }

    private void WatchTheFront()
    {
      foreach (var collectable in collectables.ToList())
      {
        if (collectable.Equals(null)) continue;
        var previous = collectable.Equals(collectables.First())
          ? playerPivot.gameObject
          : collectables[collectables.IndexOf(collectable) - 1];
        var localPosition = collectable.transform.localPosition;
        var newPos = localPosition;
        newPos = new Vector3(
          previous.transform.localPosition.x,
          newPos.y,
          newPos.z);
        newPos = Vector3.LerpUnclamped(localPosition, newPos,
          stackFollowSpeed);

        collectable.transform.localPosition = newPos;
      }
    }

    IEnumerator StackPulse()
    {
      foreach (var collectable in Enumerable.Reverse(collectables).ToList())
      {
        yield return new WaitForSeconds(pulseDelay);
        if (collectable.SafeIsUnityNull()) yield break;
        collectable.transform.DOPunchScale(new Vector3(pulseScale, pulseScale, pulseScale), pulseDuration, 0, .3f)
          .OnComplete(() => collectable.transform.localScale = Vector3.one);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Collectable>())
      {
        AddCollectable(other.gameObject);
      }
    }
  }
}
