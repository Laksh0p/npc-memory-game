using UnityEngine;
using UnityEngine.AI;

public class NPCChaser : MonoBehaviour
{
    public Transform player;
    public PlayerHide playerHide;
    public GameOverUI gameOverUI;

    private NavMeshAgent agent;

    public float detectionDistance = 12f;

    private bool isDead = false;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // 💀 Kill player
        if (dist < 1.5f && (playerHide == null || !playerHide.isHidden))
        {
            isDead = true;
            agent.isStopped = true;
            Debug.Log("💀 PLAYER CAUGHT (CHASER)");
            gameOverUI.ShowGameOver();
            return;
        }

        // 👀 Player visible
        if (dist < detectionDistance && (playerHide == null || !playerHide.isHidden))
        {
            isChasing = true;

            // 📡 SHARE info
            NPCSharedMemory.Instance.ReportSeen(player.position);
        }

        // 🛏️ Player hides
        if (playerHide != null && playerHide.isHidden && dist < detectionDistance)
        {
            NPCSharedMemory.Instance.ReportHide(player.position);
        }

        // 🎯 CHASING behavior
        if (isChasing && (playerHide == null || !playerHide.isHidden))
        {
            Vector3 target = player.position;
            target.y = transform.position.y;
            agent.SetDestination(target);
            return;
        }

        // 🧠 If player hidden → go to last known
        if (playerHide != null && playerHide.isHidden)
        {
            Vector3 target = NPCSharedMemory.Instance.lastKnownPlayerPos;
            target.y = transform.position.y;
            agent.SetDestination(target);
            return;
        }

        // 🔍 Idle search
        HandleSearch();
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