using System;
using DG.Tweening;
using Extensions.DoTween;
using Extensions.Unity;
using UnityEngine;
using UnityEngine.VFX;

namespace Components
{
    public class Tile : MonoBehaviour, ITileGrid, IPoolObj, ITweenContainerBind
    {
        public Vector2Int Coords => _coords;
        public int ID => _id;
        [SerializeField] private Vector2Int _coords;
        [SerializeField] private int _id;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _transform;
        [SerializeField] private Sprite _tileSprites;
        [SerializeField] private Sprite _bombSprite; // Bomb tile sprite
        [SerializeField] private VisualEffect _tileVFX;
        public MonoPool MyPool { get; set; }
        public ITweenContainer TweenContainer { get; set; }
        public bool ToBeDestroyed { get; set; }

        private bool _isBomb;
        public bool IsBomb
        {
            get => _isBomb;
            set
            {
                _isBomb = value;
                UpdateTileAppearance();
            }
        }

        private void Awake()
        {
            TweenContainer = TweenContain.Install(this);
        }

        
        private void OnDisable()
        {
            TweenContainer.Clear();
        }

        private void OnMouseDown() {}

        void ITileGrid.SetCoord(Vector2Int coord)
        {
            _coords = coord;
        }

        void ITileGrid.SetCoord(int x, int y)
        {
            _coords = new Vector2Int(x, y);
        }

        public void AfterCreate()
        {
            ToBeDestroyed = false;
            IsBomb = false;
            UpdateTileAppearance();
        }

        public void BeforeDeSpawn() {}

        public void TweenDelayedDeSpawn(Func<bool> onComplete) {}

        public void AfterSpawn()
        {
            ToBeDestroyed = false;
            IsBomb = false;
            UpdateTileAppearance();
            //RESET METHOD (Resurrect)
        }

        public void Teleport(Vector3 worldPos)
        {
            _transform.position = worldPos;
        }

        public void Construct(Vector2Int coords)
        {
            _coords = coords;
        }
        

        public Tween DoMove(Vector3 worldPos, TweenCallback onComplete = null)
        {
            TweenContainer.AddTween = _transform.DOMove(worldPos, 1f);
            TweenContainer.AddedTween.onComplete += onComplete;
            return TweenContainer.AddedTween;
        }

        public Sequence DoHint(Vector3 worldPos, TweenCallback onComplete = null)
        {
            _spriteRenderer.sortingOrder = EnvVar.HintSpriteLayer;

            Vector3 lastPos = _transform.position;

            TweenContainer.AddSequence = DOTween.Sequence();

            TweenContainer.AddedSeq.Append(_transform.DOMove(worldPos, 1f));
            TweenContainer.AddedSeq.Append(_transform.DOMove(lastPos, 1f));

            TweenContainer.AddedSeq.onComplete += onComplete;
            TweenContainer.AddedSeq.onComplete += delegate
            {
                _spriteRenderer.sortingOrder = EnvVar.TileSpriteLayer;
            };
            return TweenContainer.AddedSeq;
        }

        private void UpdateTileAppearance()
        {
            if (IsBomb)
            {
                // Use a sequence to wait before changing the sprite
                DOTween.Sequence()
                    .AppendInterval(.4f) // Wait for 1 second
                    .AppendCallback(() => _spriteRenderer.sprite = _bombSprite); // Change the sprite to bomb
            }
            else
            {
                _spriteRenderer.sprite = _tileSprites;
            }
        }
       
    }

    public interface ITileGrid
    {
        void SetCoord(Vector2Int coord);
        void SetCoord(int x, int y);
    }
}
