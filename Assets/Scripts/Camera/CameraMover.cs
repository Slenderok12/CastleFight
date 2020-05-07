﻿using CastleFight.Core;
using CastleFight.Core.Extensions;
using Core;
using UnityEngine;

namespace CastleFight
{
    public class CameraMover : UserAbility
    {
        [SerializeField] private Transform camTr;
        [SerializeField] private CameraMoverSettings settings;

        private IUpdateManager updateManager;
        private Vector3 cursorPos;
        private float xPercentCursorPos;
        private float yPercentCursorPos;

        private void Start()
        {
            updateManager = ManagerHolder.I.GetManager<IUpdateManager>();
        }

        private void OnDestroy()
        {
            UnsubscribeToUpdate();
        }

        private void SubscribeToUpdate()
        {
            updateManager.OnUpdate += UpdateHandler;
        }

        private void UnsubscribeToUpdate()
        {
            updateManager.OnUpdate -= UpdateHandler;
        }

        private void UpdateHandler()
        {
            CheckCursorPosition();
        }

        private void CheckCursorPosition()
        {
            cursorPos.Set(Input.mousePosition);
            xPercentCursorPos = 1f / Screen.width * cursorPos.x;
            yPercentCursorPos = 1f / Screen.height * cursorPos.y;

            if (xPercentCursorPos <= settings.ScreenSensibility)
            {
                MoveCamera(Vector3.left);
            }
            else if (xPercentCursorPos >= 1f - settings.ScreenSensibility)
            {
                MoveCamera(Vector3.right);
            }

            if (yPercentCursorPos <= settings.ScreenSensibility)
            {
                MoveCamera(Vector3.back);
            }
            else if (yPercentCursorPos >= 1f - settings.ScreenSensibility)
            {
                MoveCamera(Vector3.forward);
            }
        }

        private void MoveCamera(Vector3 direction)
        {
            camTr.Translate(direction * settings.Speed * Time.deltaTime);
        }

        public override void Unlock()
        {
            SubscribeToUpdate();
        }

        public override void Lock()
        {
            UnsubscribeToUpdate();
        }
    }
}