using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSpawner : MonoBehaviour
{
    [SerializeField] private WordConstructor _constructor = null;
    [SerializeField] private GameObject _letterBlockPrefab = null;
    [SerializeField] private float _range = 3f;
    [SerializeField] private float _height = 3f;
    [Header("Cylinder mesh")]
    [SerializeField] private Mesh _cylinder = null;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 position = transform.position;
        Quaternion rotation = Quaternion.identity;
        Vector3 scale = new Vector3(_range * 2f, _height / 2f, _range * 2f);
        Gizmos.DrawWireMesh(_cylinder, position, rotation, scale);
    }

    private void Start()
    {
        
    }
}
