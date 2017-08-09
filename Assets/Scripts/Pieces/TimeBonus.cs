using UnityEngine;

[RequireComponent(typeof(GamePiece))]
public class TimeBonus : MonoBehaviour {

    [Range(0, 5)]
    public int bonusValue = 0;

    [Range(0, 1)]
    public float chanceForBonus = 0f;

    public GameObject bonusGlow;
    public Material[] bonusMaterials;

    public GameObject ringGlow;

    void Start() {
        float random = Random.Range(0f, 1f);

        if (random > chanceForBonus) {
            bonusValue = 0;
        }

        if (GameManager.Instance.IsMovesLevel()) {
            bonusValue = 0;
        }

        SetActive(bonusValue != 0);
        if (bonusValue != 0) {
            SetupMaterial(bonusValue - 1, bonusGlow);
        }
    }

    void SetActive(bool state) {
        if (bonusGlow != null) {
            bonusGlow.SetActive(state);
        }

        if (ringGlow != null) {
            ringGlow.SetActive(state);
        }
    }

    void SetupMaterial(int value, GameObject bonusGlow) {
        int clampedValue = Mathf.Clamp(value, 0, bonusMaterials.Length - 1);

        if (bonusMaterials[clampedValue] != null && bonusGlow != null) {
            ParticleSystemRenderer bonusGlowRenderer = bonusGlow.GetComponent<ParticleSystemRenderer>();
            bonusGlowRenderer.material = bonusMaterials[clampedValue];
        }
    }

}
