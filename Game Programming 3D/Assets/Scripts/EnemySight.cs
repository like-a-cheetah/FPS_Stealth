using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public PlayerController P1;
    public PlayerController P2;

    private Enemy enemyUnit;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask, obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    private void Start()
    {
        enemyUnit = GetComponentInParent<Enemy>();
        StartCoroutine(FindTargetsDelay(0.2f));
    }

    IEnumerator FindTargetsDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    void FindTargets()
    {
        visibleTargets.Clear();
        // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if(target.name == "P1sight")
                    {
                        if (P1.canSee)
                            visibleTargets.Add(target);
                    }
                    if(target.name == "P2sight")
                    {
                        if (P2.canSee)
                            visibleTargets.Add(target);
                    }
                }
            }
        }

        if (visibleTargets.Count > 0)
            enemyUnit.canAttack = true;
        else
            enemyUnit.canAttack = false;
    }
}
