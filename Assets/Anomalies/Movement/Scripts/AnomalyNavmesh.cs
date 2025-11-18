using UnityEngine;
using UnityEngine.AI;

public class AnomalyNavmesh : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    public string idleAnimation = "Zombie Idle";
    public string walkAnimation = "Zombie Walk";
    public float stopDistance = 1.5f;
    public float lookRotationSpeed = 5f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();
        if (animator) animator.applyRootMotion = false;
    }

    void Update()
    {
        if (!target || agent == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        bool shouldWalk = distance > stopDistance;

        if (shouldWalk)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            PlayIfNotPlaying(walkAnimation);
        }
        else
        {
            agent.isStopped = true;

            Vector3 lookPos = target.position;
            lookPos.y = transform.position.y;
            Vector3 dir = lookPos - transform.position;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * lookRotationSpeed);
            }

            PlayIfNotPlaying(idleAnimation);
        }
    }

    void PlayIfNotPlaying(string stateName)
    {
        if (!animator) return;

        var info = animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName(stateName))
        {
            animator.Play(stateName, 0);
        }
    }
}
