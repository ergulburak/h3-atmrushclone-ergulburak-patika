using System;
using _Game_.Scripts.Helpers;
using _Game_.Scripts.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game_.Scripts.Player
{
  public class MovementController : Singleton<MovementController>
  {
    [FoldoutGroup("Swerve Input Settings", expanded: false)] [SerializeField] [Range(10f, 150f)]
    private float swerveSpeed;

    [FoldoutGroup("Swerve Input Settings")] [SerializeField] [Range(0.005f, 0.05f)]
    private float minDistanceToMove;

    [FoldoutGroup("Swerve Input Settings")] [SerializeField]
    private float xBound;

    [FoldoutGroup("Swerve Input Settings")] [SerializeField]
    private float minSwerveDistance;

    [FoldoutGroup("Movement Settings")] [SerializeField]
    private float forwardSpeed;


    protected TouchEventArgs _touchEventArgs;

    public virtual void Initialize()
    {
    }

    protected void MoveForward(Transform player)
    {
      var playerPos = player.position;
      playerPos = Vector3.MoveTowards(playerPos, new Vector3(playerPos.x, playerPos.y, playerPos.z + 1f),
        Time.deltaTime * forwardSpeed);
      player.position = playerPos;
    }

    protected void MoveHorizontal(TouchEventArgs args, Transform playerPivot)
    {
      var distance = Vector3.Distance(args.Position,
        _touchEventArgs.Position);

      if (distance < minSwerveDistance) return;

      var pos = args.Position;
      var playerPos = playerPivot.localPosition;


      if (playerPos.x <= xBound && pos.x > _touchEventArgs.Position.x)
      {
        playerPos = Vector3.MoveTowards(playerPos,
          new Vector3(playerPos.x + minDistanceToMove * distance, playerPos.y, playerPos.z),
          Time.fixedDeltaTime * swerveSpeed);

        if (playerPos.x > xBound)
        {
          playerPos =
            new Vector3(xBound, playerPos.y, playerPos.z);
        }
      }

      if (playerPos.x >= -xBound && pos.x < _touchEventArgs.Position.x)
      {
        playerPos = Vector3.MoveTowards(playerPos,
          new Vector3(playerPos.x - minDistanceToMove * distance, playerPos.y, playerPos.z),
          Time.fixedDeltaTime * swerveSpeed);

        if (playerPos.x < -xBound)
        {
          playerPos =
            new Vector3(-xBound, playerPos.y, playerPos.z);
        }
      }

      playerPivot.localPosition = playerPos;

      _touchEventArgs = args;
    }
  }
}
