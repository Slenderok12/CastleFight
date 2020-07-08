﻿using System.Collections;
using CastleFight.Core;
using CastleFight.Core.EventsBus;
using CastleFight.Core.EventsBus.Events;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace CastleFight
{
    public class GoldManager : MonoBehaviour
    {
        public int BotGoldAmount => botGoldAmount;
   
        [SerializeField]
        public GoldAnim goldAnimPrefab;

        [SerializeField]
        private Text goldText;
        [SerializeField]
        private int userGoldAmount = 0;
        [SerializeField]
        private int botGoldAmount = 0;
        [SerializeField]
        private Text notEnoghGoldText;
        [SerializeField]
        private UserController userController;

        public void Awake()
        {
            ManagerHolder.I.AddManager(this);
        }
        public void Start()
        {
            EventBusController.I.Bus.Subscribe<GameSetReadyEvent>(OnGameStart);
        }

        private void OnGameStart(GameSetReadyEvent gameSetReadyEvent)
        {
            goldText = FindObjectOfType<GoldText>().GetComponent<Text>();
            goldText.text = userGoldAmount.ToString();
        }
        

        private void OnDestroy()
        {
            EventBusController.I.Bus.Unsubscribe<GameSetReadyEvent>(OnGameStart);
        }

        public bool IsEnoughToBuild(BuildingBehavior buildingBehavior)
        {
            bool canPlace = userGoldAmount >= buildingBehavior.Building.Config.Cost;
            if (!canPlace)
            { 
                NotEnoughGold();
            }
            return canPlace;
        }

        public void MakeGoldChange(int gold, Team team)
        {
            if (team == Team.Team1)
            {
                userGoldAmount += gold;
                goldText.text = userGoldAmount.ToString();
            }
            else if(team == Team.Team2)
            {
                botGoldAmount += gold;
            }
        }

        private IEnumerator DestroyText(Text text)
        {
            yield return new WaitForSeconds(1);
            Destroy(text.gameObject);
        }
        
        private void NotEnoughGold()
        {
            Text text = Instantiate(notEnoghGoldText, userController.gameUIHolder);
            StartCoroutine(DestroyText(text));
            //Debug.Log("Not Enough Gold!!");
        }
    }
}
