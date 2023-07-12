using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static bool Exists = false;

    public static GameObject PlayerObject { get; private set; }
    public static Transform TargetForEnemies { get; private set; }
    public static GameObject AxeObject { get; private set; }



    private void Awake()
    {
        if (Exists)
        {
            Destroy(this);
            return;
        }

        Exists = true;

        PlayerObject = GameObject.Find("Player");
        TargetForEnemies = PlayerObject.transform.Find("Target For Enemies");
        AxeObject = GameObject.Find("Axe");
    }

}
