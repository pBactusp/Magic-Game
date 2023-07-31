using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GravitySpellsManager : MonoBehaviour
{
    private static GravitySpellsManager instance;
    private List<Transform> spellsTransforms;
    private List<float> spellsPullForces;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GravitySpellsManager already exists");
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        spellsTransforms = new List<Transform>();
        spellsPullForces = new List<float>();
    }

    public static int AddSpell(BaseGravitySpell spell)
    {
        instance.spellsTransforms.Add(spell.transform);
        instance.spellsPullForces.Add(spell.PullForce);

        return instance.spellsPullForces.Count - 1;
    }

    public static void RemoveSpell(int index)
    {
        instance.spellsTransforms.RemoveAt(index);
        instance.spellsPullForces.RemoveAt(index);
    }

    public static PullData GetPullData()
    {
        int count = instance.spellsTransforms.Count;

        //var data = new List<PullData>();
        //var data = new PullData[count];

        var data = new PullData(count);


        for (int i = 0; i < count; i++)
        {
            if (instance.spellsTransforms[i] != null)
            {
                data.Position[i] = instance.spellsTransforms[i].position;
                data.Strength[i] = instance.spellsPullForces[i];
                //data[i].Position = instance.spellsTransforms[i].position;
                //data[i].Strength = instance.spellsPullForces[i];

                //data.Add(new PullData
                //{
                //    Position = instance.spellsTransforms[i].position,
                //    Strength = instance.spellsPullForces[i]
                //});
            }
        }

        return data;
    }
}
