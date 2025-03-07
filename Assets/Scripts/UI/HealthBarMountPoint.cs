using System;
using UnityEngine;

namespace UI {
    public class HealthBarMountPoint : MonoBehaviour {
        static Camera mainCamera;
        static Canvas HealthBarCanvas;
        [SerializeField] private HealthBar healthBarPrefab;

        [SerializeField] HealthComponent healthComponent;
        private HealthBar healthBar;

        private void Awake() {
            if (!HealthBarCanvas) {
                var camGo = GameObject.FindGameObjectWithTag("HealthBarCanvas");
                if (!camGo) throw new Exception("No UICamera found");
                HealthBarCanvas = camGo.GetComponent<Canvas>();
            }

            if (!mainCamera) mainCamera = Camera.main;


            healthBar = Instantiate(healthBarPrefab, HealthBarCanvas.transform);
            healthBar.SetHealthComponent(healthComponent);
            healthBar.enabled = false;
        }

        private void Start() { }


        private void Update() {
            var screenPoint = mainCamera.WorldToScreenPoint(transform.position);
            var onScreen = screenPoint is {z: > 0, x: >= 0} && screenPoint.x <= Screen.width &&
                           screenPoint.y >= 0 && screenPoint.y <= Screen.height;


            if (!healthBar.gameObject.activeInHierarchy && onScreen && !healthComponent.IsDead) {
                healthBar.gameObject.SetActive(true);
            }
            else if (healthBar.gameObject.activeInHierarchy && !onScreen) {
                healthBar.gameObject.SetActive(false);
            }


            if (onScreen) {
                healthBar.transform.position = screenPoint;
            }
        }
    }
}