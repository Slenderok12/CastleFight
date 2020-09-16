using System;
using CastleFight.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CastleFight.MainMenu
{
    public class RaceButton : MonoBehaviour
    {
        public event Action<RaceConfig> Click;

        [SerializeField] private Button btn;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image foreGroungImg;
        [SerializeField] private Text txt;

        private RaceConfig config;
        private AudioManager audioManager;

        private void Awake()
        {
            btn.onClick.AddListener(OnBtnClick);
        }

        private void Start()
        {
            audioManager = ManagerHolder.I.GetManager<AudioManager>();
        }
        private void OnDestroy()
        {
            btn.onClick.RemoveAllListeners();
        }

        private void OnBtnClick()
        {
            audioManager.Play("Click");
            Click?.Invoke(config);
            backgroundImg.sprite = config.EnabledRaceSprite;
            foreGroungImg.sprite = config.EnForeGroundSprite;
        }

        public void SetDisabled()
        {
            backgroundImg.sprite = config.DisabledRaceSprite;
            foreGroungImg.sprite = config.DisForeGroundSprite;
        }

        public void Init(RaceConfig config)
        {
            this.config = config;
            backgroundImg.sprite = config.DisabledRaceSprite;
            foreGroungImg.sprite = config.DisForeGroundSprite;
           // SetText(config.RaceName);
        }

        private void SetText(string text)
        {
            txt.text = text;
        }
    }
}