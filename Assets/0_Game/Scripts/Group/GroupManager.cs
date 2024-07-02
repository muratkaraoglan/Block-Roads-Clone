using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    private NodeBase _endNode;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
       SetEndNode();
    }

    public void SetEndNode()
    {
        _endNode = GridManager.Instance.GetRandomEndNode();
        GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position = _endNode.transform.position;
    }

    public void OnTilePlaced(Void v)
    {
        var path = GridManager.Instance.TryFindPath(_endNode);

    }

}
