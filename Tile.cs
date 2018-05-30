using System;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour , ICloneable {

    public bool isMerged = false;
    public int indexRow;
    public int indexColumn;

    private int num;
    [SerializeField]
    public int Number {
        get { return num; }
        set {
            num = value;
            if (num == 0) { SetEmpty(); } else {
                ApplyStyle(num);
                SetVisible();
            }
        }
    }

    private Text tileText;
    private Image tileImage;
    private Animator animator;

    private void Awake() {
        tileText = GetComponentInChildren<Text>();
        tileImage = transform.Find("NumbCell").GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    private void ApplyStyleFromeHolder(int index) {
        tileText.text = TileStyleHolder.instance.tileStyles[index].number.ToString();
        tileText.color = TileStyleHolder.instance.tileStyles[index].textColor;
        Color32 col = tileText.color;
        col.a = 255;
        tileText.color = col;
        tileImage.color = TileStyleHolder.instance.tileStyles[index].tileColor;
        if (!TileStyleHolder.instance.isStyleNum) {
            tileImage.sprite = TileStyleHolder.instance.tileStyles[index].sprite;
        } else {
            tileImage.sprite = TileStyleHolder.instance.defaultSprite;
        }
    }

    public void ApplyStyle(int num) {
        switch (num) {
            case 2:
                ApplyStyleFromeHolder(0);
                break;
            case 4:
                ApplyStyleFromeHolder(1);
                break;
            case 8:
                ApplyStyleFromeHolder(2);
                break;
            case 16:
                ApplyStyleFromeHolder(3);
                break;
            case 32:
                ApplyStyleFromeHolder(4);
                break;
            case 64:
                ApplyStyleFromeHolder(5);
                break;
            case 128:
                ApplyStyleFromeHolder(6);
                break;
            case 256:
                ApplyStyleFromeHolder(7);
                break;
            case 512:
                ApplyStyleFromeHolder(8);
                break;
            case 1024:
                ApplyStyleFromeHolder(9);
                break;
            case 2048:
                ApplyStyleFromeHolder(10);
                break;
            case 4096:
                ApplyStyleFromeHolder(11);
                break;
            case 8192:
                ApplyStyleFromeHolder(12);
                break;
            case 16384:
                ApplyStyleFromeHolder(13);
                break;
            case 32768:
                ApplyStyleFromeHolder(14);
                break;
            case 65536:
                ApplyStyleFromeHolder(15);
                break;
            case 131072:
                ApplyStyleFromeHolder(16);
                break;
            case 262144:
                ApplyStyleFromeHolder(17);
                break;
            case 524288:
                ApplyStyleFromeHolder(18);
                break;
            case 1048576:
                ApplyStyleFromeHolder(19);
                break;
            case 2097152:
                ApplyStyleFromeHolder(20);
                break;
            case 4194304:
                ApplyStyleFromeHolder(21);
                break;
            case 8388608:
                ApplyStyleFromeHolder(22);
                break;
            case 16777216:
                ApplyStyleFromeHolder(23);
                break;
            case 33554432:
                ApplyStyleFromeHolder(24);
                break;
            default:
                Debug.LogError("Check the numbers that you pass to ApplyStyle!");
                break;
        }
    }

    private void SetVisible() {
        tileImage.enabled = true;
        tileText.enabled = true;
    }

    public void DoMergeAnimation() {
        animator.SetTrigger("merge");
    }

    public void DoEmergenceAnimation() {
        animator.SetTrigger("emergence");
    }

    private void SetEmpty() {
        tileImage.enabled = false;
        tileText.enabled = false;
    }

    public object Clone() {
        return this.MemberwiseClone();
    }

    public override bool Equals(object obj) {
        var tile = obj as Tile;
        return tile != null &&
               base.Equals(obj) &&
               Number == tile.Number;
    }

    public override int GetHashCode() {
        var hashCode = -2028225194;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + Number.GetHashCode();
        return hashCode;
    }
}
