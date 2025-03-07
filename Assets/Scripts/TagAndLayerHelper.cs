using UnityEngine;

public class TagAndLayerHelper : MonoBehaviour {
    private static TagAndLayerHelper _instance;

    public static TagAndLayerHelper Instance {
        get {
            if (_instance == null) {
                _instance = FindFirstObjectByType<TagAndLayerHelper>();
                if (_instance != null) return _instance;
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<TagAndLayerHelper>();
                singletonObject.name = typeof(TagAndLayerHelper) + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
            }

            return _instance;
        }
    }

    public int EnemyLayerMask { get; private set; }
    public int PlayerLayerMask { get; private set; }

    private void Awake() {
        if (!_instance) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) {
            Destroy(gameObject);
            return;
        }

        Init();
    }

    private void Init() {
        PlayerLayerMask = LayerMask.GetMask("Player");
        EnemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public static class Tags {
        public const string Player = "Player";
        public const string Enemy = "Enemy";
        public const string Weapon = "Weapon";
    }
}