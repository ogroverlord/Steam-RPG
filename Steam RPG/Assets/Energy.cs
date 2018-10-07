using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{

    public class Energy : MonoBehaviour
    {

        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float pointPerHit = 10f;

        float currentEnergyPoints;
        CameraRaycaster cameraRaycaster;

        void Start()
        {
            cameraRaycaster = GameObject.FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyRightClickObservers += ProcessRightClick;
            currentEnergyPoints = maxEnergyPoints;
        }


        void ProcessRightClick(RaycastHit raycast, int layerHit)
        {
            float newEnergyPoints = currentEnergyPoints - pointPerHit;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);     
            UpdateEnergyBar();
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

    }
}