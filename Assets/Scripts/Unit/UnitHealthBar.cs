﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleFight
{
    public class UnitHealthBar : HealthBar
    {
        [SerializeField]
        private UnitStats stats;
        private Stat maxHp;

        void Start()
        {
            stats.OnHpChanged += UpdateHealthBar;
            maxHp = (Stat)stats.GetStat(StatType.MaxHealth);
            SetTeam((Team)stats.gameObject.layer);
        }

        private void UpdateHealthBar(Stat hpStat)
        {
            Debug.Log(gameObject.name + " : ");
            SetBarValue(hpStat.Value / maxHp.Value);
        }
        
        public void Update()
        {
            var cameraPosition = Camera.main.transform.position;
            var barPosition = transform.position;
                       
            transform.LookAt(new Vector3(cameraPosition.x, cameraPosition.y, barPosition.z));
        }
    }
}