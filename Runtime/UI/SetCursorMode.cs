﻿using UnityEngine;

namespace DatabaseSync.Components
{
    public class SetCursorMode : MonoBehaviour
    {
        public bool visible = true;
        public CursorLockMode lockMode = CursorLockMode.None;

        void Start()
        {
            Cursor.visible = visible;
            Cursor.lockState = lockMode;
        }
    }
}
