using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectorType { Horizontal, Vertical }

public class DoorConnector : MonoBehaviour
{
    public ConnectorType type;
    public Vector2Int direction = Vector2Int.up;
}
