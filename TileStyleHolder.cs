using UnityEngine;

[System.Serializable]
public class TileStyle{
    public int number;
    public Color32 tileColor;
    public Color32 textColor;
    public Sprite sprite;
}

public class TileStyleHolder : MonoBehaviour {
    public bool isStyleNum = true;
    public static TileStyleHolder instance;
    public Sprite defaultSprite;
    public TileStyle[] tileStyles;

    private void Awake() {
        instance = this;
    }
}
