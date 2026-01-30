using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public partial class PlayerInputSystem
    {
        private static PlayerInputSystem _instance;

        public static PlayerInputSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerInputSystem();
                    _instance.Enable();
                }

                return _instance;
            }

            private set => _instance = value;
        }
    }
}