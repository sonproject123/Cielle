using UnityEngine;
using UnityEngine.UI;

public enum ItemObjectType {
    ITEM_GUN,
    ITEM_BLADE,
    ITEM_ACCESSORY_GUN,
    ITEM_ACCESSORY_BLADE,
    ITEM_ACCESSORY_BODY
}

public abstract class ItemObject : MonoBehaviour {
    [SerializeField] protected GameObject imageObject;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected GameObject keyObject;
    [SerializeField] protected Move playerMove;

    [SerializeField] protected int id;
    [SerializeField] protected string iconPath;
    [SerializeField] protected ItemObjectType type;

    [SerializeField] protected bool isPlayerEnter;

    protected virtual void Awake() {
        spriteRenderer = imageObject.GetComponent<SpriteRenderer>();

        isPlayerEnter = false;
        keyObject.SetActive(false);
    }

    public void Initialize(int id) {
        this.id = id;
        InitializeChild();
        spriteRenderer.sprite = Resources.Load<Sprite>(iconPath);
        if (spriteRenderer.sprite == null)
            spriteRenderer.sprite = Resources.Load<Sprite>("Icons/BLANK");
    }

    protected abstract void InitializeChild();

    public void GetItem() {
        ObjectManager.Instance.ReturnObject(transform.parent.gameObject, type.ToString());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (playerMove == null)
                playerMove = other.gameObject.GetComponent<Move>();
            
            isPlayerEnter = true;
            playerMove.nearObject = this;
            keyObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) { 
            isPlayerEnter = false;
            playerMove.nearObject = null;
            keyObject.SetActive(false);
        }
    }

    public ItemObjectType Type {
        get { return type; }
    }

    public int ID {
        get { return id; }
    }
}
