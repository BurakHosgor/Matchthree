using Events;
using Extensions.Unity;
using Extensions.Unity.MonoHelper;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace Components
{
    public class InputListener : EventListenerMono
    {
        [Inject] private InputEvents InputEvents { get; set; }
        [Inject] private Camera Camera { get; set; }
        [Inject] private GridEvents GridEvents { get; set; }
        private RoutineHelper _inputRoutine;

        // Visual Effect alanları
        public VisualEffect bubbleEffectPrefab;  // Visual Effect prefab'ını bu alanda tanımlayın
        private VisualEffect currentEffect;       // Aktif olan Visual Effect örneği
        private bool _isMouseDown;                // Fare tuşunun basılı olup olmadığını takip eden değişken

        private void Awake()
        {
            _inputRoutine = new RoutineHelper(this, null, InputUpdate);
        }

        private void Start()
        {
            _inputRoutine.StartCoroutine();
        }

        private void InputUpdate()
        {
            Ray inputRay = Camera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0)) // Fare tuşuna ilk kez basıldığında
            {
                _isMouseDown = true;
                HandleMouseDown(inputRay);
            }
            else if (Input.GetMouseButtonUp(0)) // Fare tuşuna bırakıldığında
            {
                _isMouseDown = false;
                HandleMouseUp(inputRay);
            }
            else if (_isMouseDown) // Fare tuşu basılıyken
            {
                // Visual Effect'in pozisyonunu güncelle
                if (currentEffect != null)
                {
                    currentEffect.transform.position = inputRay.origin + inputRay.direction * 100f;
                }
            }
            else
            {
                // Efekti başlat
                UpdateVisualEffect(inputRay);
            }
        }

        private void HandleMouseDown(Ray inputRay)
        {
            RaycastHit[] hits = Physics.RaycastAll(inputRay, 100f);

            Tile firstHitTile = null;

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent(out Tile tile))
                {
                    firstHitTile = tile;
                    break;
                }
            }

            InputEvents.MouseDownGrid?.Invoke(firstHitTile, inputRay.origin + inputRay.direction);
            UpdateVisualEffect(inputRay); // Mouse down olduğunda efektin pozisyonunu güncelle
        }

        private void HandleMouseUp(Ray inputRay)
        {
            InputEvents.MouseUpGrid?.Invoke(inputRay.origin + inputRay.direction);

            // Visual Effect'i durdur ve yok et
            if (currentEffect != null)
            {
                currentEffect.Stop();
                Destroy(currentEffect.gameObject, 0.1f); // Efektin tamamen durması için kısa bir gecikmeyle yok etme
                currentEffect = null;
            }
        }

        private void UpdateVisualEffect(Ray inputRay)
        {
            if (bubbleEffectPrefab != null)
            {
                if (currentEffect == null)
                {
                    currentEffect = Instantiate(bubbleEffectPrefab, inputRay.origin + inputRay.direction * 100f, Quaternion.identity);
                    currentEffect.Play();
                }
                else
                {
                    // Efektin pozisyonunu güncelle
                    currentEffect.transform.position = inputRay.origin + inputRay.direction * 100f;
                }
            }
        }

        protected override void RegisterEvents()
        {
            GridEvents.InputStart += OnInputStart;
            GridEvents.InputStop += OnInputStop;
        }

        private void OnInputStop()
        {
            _inputRoutine.StopCoroutine();
        }

        private void OnInputStart()
        {
            _inputRoutine.StartCoroutine();
        }

        protected override void UnRegisterEvents()
        {
            GridEvents.InputStart -= OnInputStart;
            GridEvents.InputStop -= OnInputStop;
        }
    }
}
