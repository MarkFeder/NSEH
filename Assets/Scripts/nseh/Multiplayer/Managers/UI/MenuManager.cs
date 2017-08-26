using nseh.Managers.Audio;
using nseh.Managers.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Multiplayer.Managers.UI
{
    public class MenuManager : Singleton<MenuManager>
    {
        [Space(10)]
        [Header("UI - Canvas")]

        [SerializeField]
        private GameObject _selectedGO;

        [Space(10)]
        [Header("UI - Buttons & Texts")]

        [SerializeField]
        private Text _topText;
        [SerializeField]
        private List<MyButton> _playerPortraits;
        [SerializeField]
        private Button _startButton;
        [SerializeField]
        private Button _backButton;

        [Space(10)]
        [Header("Audio - Clips")]

        [SerializeField]
        private AudioClip _startClip;
        [SerializeField]
        private AudioClip _backClip;
        [SerializeField]
        private AudioClip _selectClip;

        [SerializeField]
        public List<MyEventSystem> _eventSystem;

        private List<string> _firstInputs;
        private bool _firstLoad;


        private void Start()
        {
            _firstInputs = new List<string>()
            {
                String.Format("{0}{1}", Inputs.JUMP, 1),
                String.Format("{0}{1}", Inputs.JUMP, 2),
                String.Format("{0}{1}", Inputs.JUMP, 3),
                String.Format("{0}{1}", Inputs.JUMP, 4)
            };

            _firstLoad = true;
        }

        private void Update()
        {
            if (InputReceived() && _firstLoad)
            {
                // _selectedGO.SetActive(true);
                _firstLoad = false;
            }
        }

        private bool InputReceived()
        {
            bool triggered = false;

            for (int i = 0; i < _firstInputs.Count && !triggered; ++i)
            {
                if (Input.GetButtonDown(_firstInputs[i]))
                {
                    triggered = true;
                }
            }

            return triggered;
        }

        private void ChangeSelectedGO(GameObject newSelectedGO)
        {
            _selectedGO.SetActive(false);
            _selectedGO = newSelectedGO;
            _selectedGO.SetActive(true);
        }

        public void OnPlayGame()
        {

        }

        public void OnExitGame()
        {

        }

        public void OnChangeSelectedGO(GameObject newGO)
        {
            _selectedGO.SetActive(false);
            _selectedGO = newGO;
            _selectedGO.SetActive(true);
        }

        public void OnSelectCharacter(int index)
        {
            if (index < 0 || index >= _playerPortraits.Count)
            {
                Debug.LogWarning("OnSelectCharacter:: index is not valid");
                return;
            }

            // SoundManager.Instance.PlayAudioFX(_selectClip, 0.5f, false, Vector3.zero, 0);

            //_selectedGO.SetActive(false);
            //_selectedGO = _playerPortraits[index].gameObject;
            //_selectedGO.SetActive(true);
        }
    }
}
