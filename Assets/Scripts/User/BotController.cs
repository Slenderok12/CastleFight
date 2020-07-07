using System;
using System.Collections.Generic;
using System.Collections;
using CastleFight.Core.EventsBus;
using CastleFight.Core.EventsBus.Events;
using CastleFight.Config;
using UnityEngine;

namespace CastleFight
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private List<BotBuildStep> buildSteps;
        [SerializeField] private CastlesPosProvider castlesPosProvider;
        [SerializeField] private float turnTime;
        [SerializeField] private Vector3 stepOffset;
        [SerializeField] private int gold;
        [SerializeField] private int currBuildIndex;
        private int blocksBuilt = 0;
        [SerializeField] private int maxBlocksBuilt;
        public void Init(RaceConfig config)
        {
            CreateCastle(config.CastleConfig);
            EventBusController.I.Bus.Subscribe<UnitDiedEvent>(OnUnitDie);
            StartGame();
        }
        private void OnUnitDie(UnitDiedEvent unitDiedEvent)
        {
            if (unitDiedEvent.Unit.gameObject.layer == (int)Team.Team1)
            {
                gold += unitDiedEvent.Unit.Config.Cost;
            }
            else
            {
                //DO something when your unit dies
            }
        }
        private void CreateCastle(CastleConfig castleConfig)
        {
            var castleHolder = castlesPosProvider.GetCastlePos(this);
            var castlePos = castleHolder.position;
            castlePos = new Vector3(castlePos.x, castlePos.y, castlePos.z); // TODO: remove magic number
            var castle = castleConfig.Create();
            castle.transform.position = castlePos;
            castle.gameObject.layer = (int)Team.Team2;
        }

        private void PlaceBuilding(int buildInd)
        {
            var building = buildSteps[buildInd].BuildingConfig.Create();
            building.transform.position = buildSteps[buildInd].Point.position + stepOffset*blocksBuilt;
            building.GetComponent<BuildingBehavior>().Place(Team.Team2);
        }

        private IEnumerator BotGameTurn()
        {
            yield return new WaitForSeconds(turnTime);
            if (currBuildIndex >= buildSteps.Count)
            {
                currBuildIndex = 0;
                if (blocksBuilt>=maxBlocksBuilt) yield break;
                blocksBuilt++;
            }
            if (gold >= buildSteps[currBuildIndex].BuildingConfig.Cost)
            {
                PlaceBuilding(currBuildIndex);
                gold -= buildSteps[currBuildIndex].BuildingConfig.Cost;
                currBuildIndex++;
            }
            StartCoroutine(BotGameTurn());
        }

        public void StartGame()
        {
            currBuildIndex = 0;
            StartCoroutine(BotGameTurn());
        }

        public void StopGame()
        {

        }
        public void OnDestroy()
        {
            EventBusController.I.Bus.Unsubscribe<UnitDiedEvent>(OnUnitDie);
        }
    }

    [Serializable]
    public class BotBuildStep
    {
        public Transform Point;
        public BaseBuildingConfig BuildingConfig;
    }
}