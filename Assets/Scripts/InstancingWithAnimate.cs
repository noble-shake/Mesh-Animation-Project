using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancingWithAnimate : MonoBehaviour
{
    [SerializeField] BakeScript ScriptableAsset;
    [SerializeField] Mesh CurrentMesh;
    [SerializeField] Mesh[] meshes;
    [SerializeField] Material material;
    [SerializeField] int SpawnCount = 20000;
    [SerializeField] int batchSize = 1000;
    [SerializeField] int batchCounter;
    public List<List<Matrix4x4>> batchPosition;
    [SerializeField] bool Running;
    public RenderParams _rp;
    int count = 0;

    void Start()
    {
        batchPosition = new List<List<Matrix4x4>>();    
        Running = true;
        if (ScriptableAsset != null)
        {
            meshes = ScriptableAsset.meshes;
        }

        CurrentMesh = meshes[0];

        if (material != null)
        {
            _rp = new RenderParams(material);
        }

        int stack = 0;
        batchCounter = 0;
        batchPosition.Add(new List<Matrix4x4>());
        for (int idx = 0; idx < SpawnCount; idx++)
        {
            float x = Random.Range(-50f, 50f);
            float z = Random.Range(-50f, 50f);
            float y = 1f;
            Vector3 trsVec = new Vector3(x, y, z);

            float yaw = Random.Range(0f, 360f);

            if (batchCounter < 1000)
            {
                batchPosition[stack].Add(Matrix4x4.TRS(trsVec, Quaternion.Euler(new Vector3(0f, yaw, 0f)), new Vector3(1f, 1f, 1f)));
                batchCounter++;
            }
            else
            {
                batchPosition.Add(new List<Matrix4x4>());
                batchCounter = 0;
                stack++;
            }
        }

    }

    void Update()
    {
        if (Running)
        {
            for (int idx = 0; idx < batchPosition.Count; idx++)
            {
                Graphics.RenderMeshInstanced(_rp, CurrentMesh, 0, batchPosition[idx]);
            }
        }

    }

    private void FixedUpdate()
    {
        if (Running)
        {
            CurrentMesh = meshes[count++];
            if (count > meshes.Length - 1) count = 0;
        }
    }
}
