using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JoaoSantos.General;
using System;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class PlaneGenerator : SingletonBehaviour<PlaneGenerator>
    {
        [Header("Values")]
        [SerializeField]
        private float destroyPreviouslyPlaneDelay;

        [Header("References")]

        [SerializeField]
        private List<Plane> planePrefabs;

        [SerializeField]
        private Transform planeArea;

        #region Local Variable        

        private float nextPlanePosition;

        private List<Plane> spawnedPlanes;

        #endregion

        #region  Unity Methods

        protected override void Awake()
        {
            base.Awake();
            InitVariables();
        }

        private void Start()
        {
            FindSpawnedPlanes();
            CalculateNextPlanePosition();
            SetLastTrigger();
        }

        #endregion

        #region Private Methods

        private void InitVariables()
        {
            this.spawnedPlanes = new List<Plane>();
        }

        private void FindSpawnedPlanes()
        {
            this.spawnedPlanes.AddRange(planeArea.GetComponentsInChildren<Plane>());
        }

        private void CalculateNextPlanePosition()
        {
            for (int i = 0; i < this.spawnedPlanes.Count; i++)
            {
                this.nextPlanePosition += this.spawnedPlanes[i].Size;
            }
        }

        private void SetLastTrigger()
        {
            for (int i = 0; i < this.spawnedPlanes.Count; i++)
            {
                SetPlaneTrigger(this.spawnedPlanes[i]);
            }
        }

        private void SetPlaneTrigger(Plane plane)
        {
            plane.spawnNextPlaneEvent.AddListener(OnSpawnNextPlane);
            plane.destroyPlaneEvent.AddListener(OnDestroyPlane);
        }

        private void OnSpawnNextPlane()
        {
            var planePrefab = planePrefabs.GetRandomItem();

            var plane = Instantiate(planePrefab, this.planeArea);
            plane.transform.localPosition = new Vector3(0, 0, this.nextPlanePosition);

            this.spawnedPlanes.Add(plane);
            this.nextPlanePosition += plane.Size;
            SetPlaneTrigger(plane);
        }

        private void OnDestroyPlane(Plane plane)
        {
            StartCoroutine(DestroyPlaneRoutine(plane));
        }

        private IEnumerator DestroyPlaneRoutine(Plane plane)
        {
            this.spawnedPlanes.Remove(plane);

            yield return new WaitForSeconds(this.destroyPreviouslyPlaneDelay);
            plane.RemoveEvents();
            Destroy(plane.gameObject);
        }

        #endregion
    }
}