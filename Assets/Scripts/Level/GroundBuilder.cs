using System.Collections.Generic;
using UnityEngine;

public class GroundBuilder : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    private GroundSegment startSegment;

    [SerializeField]
    private List<GroundSegment> segmentPrefabs;

    [SerializeField]
    private float visibleDistanceAhead = 150f;

    [SerializeField]
    private float visibleDistanceBehind = 50f;

    [SerializeField]
    private List<GroundSegment> activeSegments = new();

    private void Start()
    {
        activeSegments.Clear();
        activeSegments.Add(startSegment);

        BuildAhead();
    }

    private void Update()
    {
        BuildAhead();
        RemoveBehind();
    }

    private void BuildAhead()
    {
        float requiredDistanceZ = target.position.z + visibleDistanceAhead;

        while (activeSegments[activeSegments.Count - 1].EndPoint.position.z < requiredDistanceZ)
        {
            AddSegment();
        }
    }

    private void RemoveBehind()
    {
        float removeBeforeZ = target.position.z - visibleDistanceBehind;

        while (activeSegments.Count > 1)
        {
            GroundSegment firstSegment = activeSegments[0];

            if (firstSegment.EndPoint.position.z >= removeBeforeZ)
                break;

            activeSegments.RemoveAt(0);
            Destroy(firstSegment.gameObject);
        }
    }

    private void AddSegment()
    {
        GroundSegment previousSegment = activeSegments[activeSegments.Count - 1];
        GroundSegment prefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Count)];
        GroundSegment newSegment = Instantiate(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            transform
        );

        Vector3 offset = previousSegment.EndPoint.position - newSegment.StartPoint.position;
        newSegment.transform.position += offset;
        activeSegments.Add(newSegment);
    }
}
