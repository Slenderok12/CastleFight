﻿using System.Collections;
using System;
using System.Collections.Generic;
using CastleFight.Core;
using UnityEngine;

namespace CastleFight
{
    public class BuildingsLimitManager : MonoBehaviour
    {
        public Action UserTryWithMaximum;
        public Action UserUpdateLabel;
        public BuildingLimitConfig BuildingLimitConfig => buildingLimitConfig;
        public int BuildingsCount(Team team) => buildingsBuilt[team];

        [SerializeField] private BuildingLimitConfig buildingLimitConfig;

        private Dictionary<Team, int> buildingsBuilt;

        public void Awake()
        {
            ManagerHolder.I.AddManager(this);
            buildingsBuilt = new Dictionary<Team, int>();
            buildingsBuilt[Team.Team1] = 0;
            buildingsBuilt[Team.Team2] = 0;
        }
        public void AddBuilding(Team team)
        {
            if (!buildingsBuilt.ContainsKey(team))
            {
                buildingsBuilt.Add(team, 1);
            }
            else
            {
                buildingsBuilt[team]++;
            }

            if(team == Team.Team1)
            {
                UserUpdateLabel?.Invoke();
            }
        }
        public void DeleteBuilding(Team team)
        {
            if (buildingsBuilt.ContainsKey(team))
            {
                if (buildingsBuilt[team] > 0)
                {
                    buildingsBuilt[team]--;
                    UserUpdateLabel?.Invoke();
                }
            }
        }

        public bool CanBuild(Team team)
        {
            bool canBuild = true;
            if (buildingsBuilt.ContainsKey(team))
            {
                canBuild = buildingsBuilt[team] < buildingLimitConfig.MaxBuildingsPerTeam;
                if (buildingsBuilt[team] == buildingLimitConfig.MaxBuildingsPerTeam && team == Team.Team1)
                {
                    UserTryWithMaximum?.Invoke();
                }         
            }
            return canBuild;
        }
    }
}