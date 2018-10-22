using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Characters
{

    public class Energy : MonoBehaviour
    {

        [SerializeField] RawImage energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 5f;
   
        float currentEnergyPoints;

        private void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        private void Update()
        {
            if(currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        public void ConsumeEnergy(float amount)
        {
            if (IsEnergyAvailable(amount))
            {
                float newEnergyPoints = currentEnergyPoints - amount;
                currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            }
            UpdateEnergyBar();
        }
        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }
        private void UpdateEnergyBar()
        {
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
        float EnergyAsPercent()
        {
            return currentEnergyPoints / maxEnergyPoints;
        }

        void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

    }
}