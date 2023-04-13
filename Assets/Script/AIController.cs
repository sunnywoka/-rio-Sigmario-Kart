using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject checkPoints;
    public NavMeshAgent m_agent;
    List<GameObject> checkPointList;
    Transform m_target;
    int i, count;
    // Start is called before the first frame update

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = 50;
    }
    void Start()
    {
        count = -1;
        checkPointList = new List<GameObject>();
        foreach (Transform cp in checkPoints.transform)
        {
            checkPointList.Add(cp.gameObject);
            count++;
        }
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_target = checkPointList[i].transform;
        if ((Vector3.Distance(transform.position, m_target.position) < 5) && (i<count))
        {
            i++;
        }
        m_target = checkPointList[i].transform;
        m_agent.SetDestination(m_target.position);
    }
}
