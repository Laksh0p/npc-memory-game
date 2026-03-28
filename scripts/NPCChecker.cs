using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class NPCChecker : MonoBehaviour
{
    public Transform player;
    public PlayerHide playerHide;
    public GameOverUI gameOverUI;
    private NavMeshAgent agent;
    public float detectionDistance = 6f;
    private bool isDead = false;//check for public as well crashing else
    //Featherless AI
    string aiDecision = "SEARCH";
    bool hasAskedAI = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);

        //this kills player
        if (dist < 1.5f && (playerHide == null || !playerHide.isHidden))
        {
            isDead = true;
            agent.isStopped = true;
            Debug.Log("💀 PLAYER CAUGHT (CHECKER)");

            if (gameOverUI != null)
                gameOverUI.ShowGameOver();

            return;
        }

        //dynamic programmimng type shi
        if (dist < detectionDistance && (playerHide == null || !playerHide.isHidden))
        {
            NPCSharedMemory.Instance.ReportSeen(player.position);
        }

        //they know when we hide
        if (playerHide != null && playerHide.isHidden && dist < detectionDistance)
        {
            NPCSharedMemory.Instance.ReportHide(player.position);
        }

        //reset ai when sees me
        if (playerHide == null || !playerHide.isHidden)
        {
            hasAskedAI = false;
        }

        //if player under bed ask ai
        if (playerHide != null && playerHide.isHidden)
        {
            if (!hasAskedAI)
            {
                hasAskedAI = true;

                //instant fallback
                aiDecision = "SEARCH";

                StartCoroutine(CallAI());
            }

            SmartCheck();
            return;
        }

        //else search
        HandleSearch();
    }

    void SmartCheck()
    {
        var memory = NPCSharedMemory.Instance;

        Vector3 target;
        if (aiDecision == "CHECK" && memory.hideSpotConfidence > 0.3f)
        {
            Debug.Log("AI: CHECK hide spot");
            target = memory.frequentHideSpot;
        }
        else
        {
            Debug.Log("AI: SEARCH area");

            Vector3 direction = (memory.lastKnownPlayerPos - transform.position).normalized;
            target = memory.lastKnownPlayerPos + direction * 3f;
        }

        target.y = transform.position.y;
        agent.SetDestination(target);
    }

    IEnumerator CallAI()
    {
        Debug.Log(" Calling Featherless AI...");

        string prompt = "Player disappeared near a hiding spot. Answer ONLY one word: CHECK or SEARCH.";

        string json = "{ \"model\": \"featherless-ai/Qwerky-72B\", \"messages\": [{\"role\":\"user\",\"content\":\"" + prompt + "\"}], \"max_tokens\": 5 }";

        UnityWebRequest request = new UnityWebRequest("https://api.featherless.ai/v1/chat/completions", "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer rc_c3bfd4464b0e38a227f5ac50c98538fff7ec8677048457fe8563aaba96a2c805");//tera api mat daal

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string res = request.downloadHandler.text.ToUpper();

            if (res.Contains("CHECK"))
                aiDecision = "CHECK";
            else
                aiDecision = "SEARCH";

            Debug.Log("AI DECISION: " + aiDecision);
        }
        else
        {
            Debug.Log("AI FAILED");
            aiDecision = "SEARCH";
        }
    }

    void HandleSearch()
    {
        if (!agent.hasPath || agent.remainingDistance < 1f)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * 6f;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 6f, 1))
            {
                agent.SetDestination(hit.position);
            }
        }
    }
}