using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    ///     根据HealthComponent的变化更新血条
    ///     <see cref="HealthComponent" />
    /// </summary>
    public class HealthBar : MonoBehaviour {
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text healthText;

        private HealthComponent _healthComponent;

        public void SetHealthComponent(HealthComponent healthComponent) {
            if (_healthComponent) _healthComponent.OnHealthChange -= OnHealthChange;

            _healthComponent = healthComponent;
            healthComponent.OnHealthChange += OnHealthChange;
            OnHealthChange(_healthComponent.CurrentHealth);
        }

        /// <summary>
        ///     核心功能 healthComponent.OnHealthChange的响应函数
        /// </summary>
        /// <param name="currentHealth"></param>
        private void OnHealthChange(float currentHealth) {
            var healthPer = currentHealth / _healthComponent.maxHealth;
            healthBar.fillAmount = healthPer;
            healthBar.color = Color.Lerp(Color.red, Color.green, healthPer);
            healthText.text = $"{currentHealth}/{_healthComponent.maxHealth}";
        }
    }
}