using UnityEngine;
using UnityEngine.UI;

public enum ItemObjectType {
    GUN,
    BLADE,
    ACCESSORY_GUN,
    ACCESSORY_BLADE,
    ACCESSORY_BODY
}

public abstract class ItemObject : MonoBehaviour {
    [SerializeField] protected GameObject imageObject;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Move playerMove;

    [SerializeField] protected int id;
    [SerializeField] protected string iconPath;
    [SerializeField] protected ItemObjectType type;

    [SerializeField] protected bool isPlayerEnter;

    protected virtual void Awake() {
        spriteRenderer = imageObject.GetComponent<SpriteRenderer>();

        isPlayerEnter = false;
    }

    public void Initialize(int id) {
        this.id = id;
        InitializeChild();
        spriteRenderer.sprite = Resources.Load<Sprite>(iconPath);
        if (spriteRenderer.sprite == null)
            spriteRenderer.sprite = Resources.Load<Sprite>("Icons/BLANK");
    }

    protected abstract void InitializeChild();

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (playerMove == null)
                playerMove = other.gameObject.GetComponent<Move>();
            
            isPlayerEnter = true;
            playerMove.nearObject = this.gameObject;
            Debug.Log(playerMove.nearObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) { 
            isPlayerEnter = false;
            playerMove.nearObject = null;
            Debug.Log(playerMove.nearObject);
        }
    }
}
