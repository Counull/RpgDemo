using System;
using UnityEngine;

namespace UI {
    /// <summary>
    ///     血条在GO上的挂载点，用于转换为屏幕坐标进而设置血条位置
    ///     <see cref="HealthBar" />
    /// </summary>
    public class HealthBarMountPoint : MonoBehaviour {
        private static Camera _mainCamera;
        private static Canvas _healthBarCanvas;
        [SerializeField] private HealthBar healthBarPrefab;
        [SerializeField] private HealthComponent healthComponent;
        private HealthBar healthBar;

        private void Awake() {
            if (!_healthBarCanvas) {
                var camGo = GameObject.FindGameObjectWithTag("HealthBarCanvas");
                if (!camGo) throw new Exception("No UICamera found");
                _healthBarCanvas = camGo.GetComponent<Canvas>();
            }

            if (!_mainCamera) _mainCamera = Camera.main;
            healthBar = Instantiate(healthBarPrefab, _healthBarCanvas.transform);
            healthBar.SetHealthComponent(healthComponent);
            healthBar.enabled = false;
        }

        


        /// <summary>
        ///     转换屏幕坐标并设置血条位置，如果血条在屏幕外则隐藏
        /// </summary>
        private void Update() {
            var screenPoint = _mainCamera.WorldToScreenPoint(transform.position);
            var onScreen = screenPoint is {z: > 0, x: >= 0} && screenPoint.x <= Screen.width &&
                           screenPoint.y >= 0 && screenPoint.y <= Screen.height;


            //切换血条激活状态
            if (!healthBar.gameObject.activeInHierarchy && onScreen && !healthComponent.IsDead) {
                healthBar.gameObject.SetActive(true);
            }
            else if (healthBar.gameObject.activeInHierarchy && !onScreen) {
                healthBar.gameObject.SetActive(false);
                return;
            }

            //设置血条位置
            if (onScreen) healthBar.transform.position = screenPoint;
        }
    }
}