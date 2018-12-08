using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Characters
{

    public class SpecialAbilities : MonoBehaviour
    {

        [SerializeField] Image energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 5f;
        [SerializeField] SpecialAbilty[] abilities;
        [SerializeField] AudioClip outOfEnergySound;


        AudioSource audioSource;
        float currentEnergyPoints;

        float EnergyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;

            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        private void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void AttachInitialAbilities()
        {
            for (int abiltyIndex = 0; abiltyIndex < abilities.Length; abiltyIndex++)
            {
                abilities[abiltyIndex].AttachAbiltyTo(gameObject);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        public void UseSpecialAbilty(int abiltyIndex, GameObject target = null)
        {
            var energyCost = abilities[abiltyIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abiltyIndex].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergySound);
            }
        }

        public void ConsumeEnergy(float amount)
        {
            if (amount <= currentEnergyPoints)
            {
                float newEnergyPoints = currentEnergyPoints - amount;
                currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            }
            UpdateEnergyBar();
        }


        private void UpdateEnergyBar()
        {
            energyBar.fillAmount = EnergyAsPercent;
        }


        void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

    }
}