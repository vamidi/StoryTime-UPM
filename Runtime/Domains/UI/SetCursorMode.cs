using UnityEngine;

namespace StoryTime.Domains.UI
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
