using UnityEngine;

public class NPCSharedMemory : MonoBehaviour
{
    public static NPCSharedMemory Instance;

    void Awake()
    {
        Instance = this;
        LoadData(); //load memory on start
    }

    //Shared data
    public Vector3 lastKnownPlayerPos;
    public Vector3 frequentHideSpot;

    public int totalEncounters = 0;
    public int totalHides = 0;

    public float hideSpotConfidence = 0f;

    //SAVE KEYS(fire red type ahh)
    string KEY_X = "hide_x";
    string KEY_Y = "hide_y";
    string KEY_Z = "hide_z";
    string KEY_CONF = "hide_conf";
    string KEY_COUNT = "hide_count";

    // ai Called when player is seen
    public void ReportSeen(Vector3 pos)
    {
        lastKnownPlayerPos = pos;
        totalEncounters++;
    }

    // ai Called when player hides
    public void ReportHide(Vector3 pos)
    {
        lastKnownPlayerPos = pos;

        // Learn hide pattern
        frequentHideSpot = Vector3.Lerp(frequentHideSpot, pos, 0.3f);
        hideSpotConfidence += 0.2f;

        totalHides++;
        totalEncounters++;

        SaveData(); // SAVE every time
    }

    // SAVE FUNCTION
    void SaveData()
    {
        PlayerPrefs.SetFloat(KEY_X, frequentHideSpot.x);
        PlayerPrefs.SetFloat(KEY_Y, frequentHideSpot.y);
        PlayerPrefs.SetFloat(KEY_Z, frequentHideSpot.z);
        PlayerPrefs.SetFloat(KEY_CONF, hideSpotConfidence);
        PlayerPrefs.SetInt(KEY_COUNT, totalHides);
        PlayerPrefs.Save();
        Debug.Log(" Memory Saved");
    }

    //  LOAD FUNCTION
    void LoadData()
    {
        if (PlayerPrefs.HasKey(KEY_X))
        {
            float x = PlayerPrefs.GetFloat(KEY_X);
            float y = PlayerPrefs.GetFloat(KEY_Y);
            float z = PlayerPrefs.GetFloat(KEY_Z);
            frequentHideSpot = new Vector3(x, y, z);
            hideSpotConfidence = PlayerPrefs.GetFloat(KEY_CONF);
            totalHides = PlayerPrefs.GetInt(KEY_COUNT);
            Debug.Log("Memory Loaded");
        }
    }
}