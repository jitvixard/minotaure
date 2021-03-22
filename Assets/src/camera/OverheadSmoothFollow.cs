using System;
using System.Collections;
using src.services;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.camera
{
    [RequireComponent(typeof(Camera))]
    public class OverheadSmoothFollow : MonoBehaviour
    {
        PlayerService playerService;

        Coroutine smoothRoutine;

        float yValue;
        float smoothDistance;
        int smoothTime;
        
        void Awake()
        {
            playerService = Environment.PlayerService;

            smoothDistance = Environment.CAMERA_SMOOTH_DIST;
            yValue = transform.position.y;
        }

        void Update()
        {
            var player = playerService.Player;
            if (player)
            {
                var cameraPos = GetNormalizedPosition(transform.position);
                var playerPos = GetNormalizedPosition(player.transform.position);
                
                var distance = Vector3.Distance(cameraPos, playerPos);
                
                if (distance > smoothDistance)
                {
                    if (smoothRoutine != null) return;
                    smoothRoutine = StartCoroutine(SmoothRoutine());
                }
            }
        }

        IEnumerator SmoothRoutine()
        {
            var currentPosition = transform.position;
            var playerPosition = playerService.Player.transform.position;
            var distance = 0f;

            var t = 0f;
            
            while (distance > 1f && !(playerService.Player is null))
            {
                t += Time.deltaTime;
                
                distance = Vector3.Distance(
                    GetNormalizedPosition(currentPosition),
                    GetNormalizedPosition(playerPosition));

                transform.position = new Vector3(
                    Mathf.Lerp(currentPosition.x, playerPosition.x, t / smoothTime),
                    yValue,
                    Mathf.Lerp(currentPosition.z, playerPosition.z, t / smoothTime));
                
                yield return null;
            }
        }

        Vector3 GetNormalizedPosition(Vector3 position)
        {
            return new Vector3(position.x, 0, position.y);
        }
    }
}