using UnityEngine;
public class Energabler : MonoBehaviour
{
    public int energy = 0;
    public int max_energy = 100;
    public bool switchesOnFull = true;
    MeshRenderer renderer;

    public MeshRenderer diodeRenderer;
    Transform[] children;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.materials[0].SetFloat("_EmissiveIntensity", 0.2f);
        children = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform T in transform)
        {
            children[i] = T;
            i++;
        }
    }

    private void Update()
    {
        if(gameObject.tag == "Container")
        {
            Color cEmissive = new Color(93, 203, 255);

            renderer.materials[0].SetColor("_EmissiveColor", Color.Lerp(Color.black, cEmissive, 0.032f * ((float)energy) / max_energy));
            
            if(diodeRenderer != null)
            {
                if (switchesOnFull)
                {
                    diodeRenderer.materials[0].SetColor("_BaseColor", Color.Lerp(Color.red, Color.green, ((float)energy) / max_energy));
                }
                else
                {
                    diodeRenderer.materials[0].SetColor("_BaseColor", Color.Lerp(Color.green, Color.red, ((float)energy) / max_energy));
                }
            }
        }
        else if (gameObject.tag == "Player") // players
        {
            float s = 1f * ((float)energy) / max_energy + 1;
            Transform camera = transform.Find("Camera");
            transform.DetachChildren();
            transform.localScale = new Vector3(s, s, s);
            AttachChildren();
        }
    }

    public void AttachChildren()
    {
        for(int i=0; i<children.Length; i++)
        {
            children[i].parent = transform;
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

    public bool isSwitchedOn()
    {
        if (switchesOnFull && IsFull()) return true;
        if (!switchesOnFull && IsEmpty()) return true;
        return false;
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
