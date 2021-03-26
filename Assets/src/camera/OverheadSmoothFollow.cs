using System.Collections;
using src.actors.controllers.impl;
using src.services.impl;
using src.util;
using UnityEngine;

namespace src.camera
{
    [RequireComponent(typeof(Camera))]
    public class OverheadSmoothFollow : MonoBehaviour
    {
        PlayerService playerService;
        Transform     playerTransform;
        float         smoothDistance;

        Coroutine smoothRoutine;
        int       smoothTime;
        Coroutine trackingRoutine;

        float yValue;

        void Awake()
        {
            playerService = Environment.PlayerService;
            playerService.Player += PlayerChanged;

            smoothDistance = Environment.CAMERA_SMOOTH_DIST;
            smoothTime = Environment.CAMERA_SMOOTH_TIME;
            yValue = transform.position.y;
        }

        void PlayerChanged(PawnActorController pawnActorController)
        {
            var shouldTrack = pawnActorController != null;
            if (!shouldTrack) return;
            if (smoothRoutine != null) StopCoroutine(smoothRoutine);
            if (trackingRoutine != null) StopCoroutine(trackingRoutine);
            playerTransform = pawnActorController.transform;
            trackingRoutine = StartCoroutine(TrackingRoutine());
        }

        IEnumerator TrackingRoutine()
        {
            var distance = Vector3.Distance(
                GetNormalizedPosition(transform.position),
                GetNormalizedPosition(playerTransform.position));

            while (distance < smoothDistance)
            {
                distance = Vector3.Distance(
                    GetNormalizedPosition(transform.position),
                    GetNormalizedPosition(playerTransform.position));
                yield return null;
            }

            trackingRoutine = null;
            smoothRoutine = StartCoroutine(SmoothRoutine());
        }

        IEnumerator SmoothRoutine()
        {
            float distance;
            var t = 0f;

            do
            {
                var currentPosition = transform.position;
                var playerPosition = playerTransform.position;

                t += Time.deltaTime;

                distance = Vector3.Distance(
                    GetNormalizedPosition(currentPosition),
                    GetNormalizedPosition(playerPosition));

                transform.position = new Vector3(
                    Mathf.Lerp(currentPosition.x, playerPosition.x, t / smoothTime),
                    yValue,
                    Mathf.Lerp(currentPosition.z, playerPosition.z, t / smoothTime));

                yield return null;
            } while (distance > 1f);

            smoothRoutine = null;
            trackingRoutine = StartCoroutine(TrackingRoutine());
        }

        Vector3 GetNormalizedPosition(Vector3 position)
        {
            return new Vector3(position.x, yValue, position.z);
        }
    }
}