using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class StarObject : MonoBehaviour
{
    private bool isCollision = false;
    private Rigidbody rigid;
    private MeshRenderer meshRenderer;
    private new Collider collider;

    [SerializeField] private UnityEvent onGetStar;

    private Vector3 originScale;
    private bool skipRegister;
    public bool SkipRegister
    {
        get { return skipRegister; }
        set
        {
            skipRegister = value;
            if (skipRegister == true)
                GameManager.Instance.StarCount -= 1;
        }
    }

    private void Awake()
    {
        EventManager.StartListening(Constant.RESET_GAME_EVENT, RegisterStarCount);

        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        originScale = transform.localScale;
    }

    private void OnEnable()
    {
        BackPosition backPosition = GetComponent<BackPosition>();

        if (!SkipRegister)
        {
            RegisterStarCount();

            if (!backPosition)
            {
                gameObject.AddComponent<BackPosition>();
            }
        }
        else
        {
            if (backPosition)
            {
                Destroy(backPosition);
            }
        }
    }

    private void Start()
    {
        EventManager.StartListening(Constant.CLEAR_STAGE_EVENT, DestroyObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(Constant.PLAYER_TAG) && GameManager.GameState == GameState.Play && !isCollision)
        {
            onGetStar.Invoke();
            SetData(false);

            GameManager.Instance.StarCount -= 1;
            isCollision = true;

            if (GameManager.Instance.StarCount == 0)
            {
                EventManager.TriggerEvent(Constant.GET_STAR_EVENT);
            }
        }
    }

    private void RegisterStarCount()
    {
        if (rigid == null) return;

        SetData(true);
        rigid.velocity = Vector3.zero;
        isCollision = false;

        if (skipRegister)
        {
            DestroyObject();
            return;
        }

        GameManager.Instance.StarCount += 1;
        Debug.Log($"Star Count = {GameManager.Instance.StarCount}");
    }

    private void SetData(bool isEnabled)
    {
        collider.enabled = isEnabled;
        meshRenderer.enabled = isEnabled;

        if (isEnabled)
        {
            transform.DOKill();
            transform.DOScale(originScale, 0.3f);
        }
    }

    private void DestroyObject()
    {
        skipRegister = false;
        PoolManager.Push(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Constant.RESET_GAME_EVENT, RegisterStarCount);
        EventManager.StopListening(Constant.CLEAR_STAGE_EVENT, DestroyObject);
    }
}
