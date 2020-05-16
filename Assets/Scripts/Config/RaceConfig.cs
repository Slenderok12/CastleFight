﻿using UnityEngine;

namespace CastleFight
{
    [CreateAssetMenu(fileName = "RaceConfig", menuName = "Race/RaceConfig", order = 1)]
    public class RaceConfig : ScriptableObject
    {
        [SerializeField] private string raceName;
        [SerializeField] private BuildingSet buildingSet;

        public string RaceName => raceName;
        public BuildingSet BuildingSet => buildingSet;

        public bool Equals(RaceConfig config)
        {
            if (config == null || string.IsNullOrEmpty(config.raceName))
            {
                return false;
            }

            return raceName.Equals(config.raceName);
        }
    }
}