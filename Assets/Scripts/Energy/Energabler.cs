using UnityEngine;
public class Energabler : MonoBehaviour
{
    public int energy = 0;
    public int max_energy = 100;
    MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.materials[0].SetFloat("_EmissiveIntensity", 0.2f);
    }

    private void Update()
    {
        if(gameObject.tag == "Container")
        {
            Color c = new Color(93, 203, 255);
            renderer.materials[0].SetColor("_EmissiveColor", Color.Lerp(Color.black, c, 0.3f * ((float)energy) / max_energy));
        }
        else if (gameObject.tag == "Player") // players
        {
            float s = 0.5f * ((float)energy) / max_energy + 1;
            transform.localScale = new Vector3(s, s, s);
        }
    }

    public bool AddEnergy(int size_of_energy=1)
    {
        if (IsFull(size_of_energy))
        {
            return false;
        }
        energy += size_of_energy;
        return true;
    }

    public bool RemEnergy(int size_of_energy=1)
    {
        if (IsEmpty(size_of_energy))
        {
            return false;
        }
        energy -= size_of_energy;
        return true;
    }

    public bool IsFull(int size=1)
    {
        return energy+size > max_energy;
    }

    public bool IsEmpty(int size=1)
    {
        return energy-size < 0;
    }

    public static void UpdateEmissiveColorFromIntensityAndEmissiveColorLDR(Material material)
    {
        const string kEmissiveColorLDR = "_EmissiveColorLDR";
        const string kEmissiveColor = "_EmissiveColor";
        const string kEmissiveIntensity = "_EmissiveIntensity";

        if (material.HasProperty(kEmissiveColorLDR) && material.HasProperty(kEmissiveIntensity) && material.HasProperty(kEmissiveColor))
        {
            Color emissiveColorLDR = material.GetColor(kEmissiveColorLDR);
            Color emissiveColorLDRLinear = new Color(Mathf.GammaToLinearSpace(emissiveColorLDR.r), Mathf.GammaToLinearSpace(emissiveColorLDR.g), Mathf.GammaToLinearSpace(emissiveColorLDR.b));
            material.SetColor(kEmissiveColor, emissiveColorLDRLinear * material.GetFloat(kEmissiveIntensity));
        }
    }
}
