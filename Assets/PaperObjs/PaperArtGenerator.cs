using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class PaperArtGenerator : MonoBehaviour
{
    public Material defaultMaterial; // ����Ĳ���
    public Texture2D customTexture; // �Զ�����ͼ����
    public Texture2D backTexture; // �Զ�����ͼ����
    public float scaleOffSetRatio;
    public string assetSavePath = "Assets/PaperObjs/MeshAndMat";

    [ContextMenu("Init Paper GameObject")]
    public void InitPaperGameObject()
    {
        //InitPlane(customTexture);
        InitPlane(customTexture,backTexture);
    }

    public void InitPlane(Texture2D tempTexture)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, tempTexture.height/ scaleOffSetRatio);
        vertices[2] = new Vector3(tempTexture.width/ scaleOffSetRatio, tempTexture.height/ scaleOffSetRatio);
        vertices[3] = new Vector3(tempTexture.width/ scaleOffSetRatio, 0);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        string gameObjectName = customTexture.name;
        GameObject newPlane = new GameObject(gameObjectName, typeof(MeshFilter), typeof(MeshRenderer));
        newPlane.GetComponent<MeshFilter>().mesh = mesh;
        Material material = InitNewMaterial(tempTexture);
        newPlane.GetComponent<MeshRenderer>().material = material;

        string meshName = customTexture.name + ".asset";
        string materialName = customTexture.name + ".mat";
        // ����Mesh����Դ��
        AssetDatabase.CreateAsset(mesh, $"{assetSavePath}/{meshName}");
        AssetDatabase.SaveAssets();
        // ����Material����Դ��
        AssetDatabase.CreateAsset(material, $"{assetSavePath}/{materialName}");
        AssetDatabase.SaveAssets();
    }

    public void InitPlane(Texture2D frontTexture,Texture2D backTexture)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, frontTexture.height / scaleOffSetRatio);
        vertices[2] = new Vector3(frontTexture.width / scaleOffSetRatio, frontTexture.height / scaleOffSetRatio);
        vertices[3] = new Vector3(frontTexture.width / scaleOffSetRatio, 0);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        string gameObjectName = "2Sided"+customTexture.name;
        GameObject newPlane = new GameObject(gameObjectName, typeof(MeshFilter), typeof(MeshRenderer));
        newPlane.GetComponent<MeshFilter>().mesh = mesh;
        Material material = new Material(defaultMaterial);
        material.doubleSidedGI = true;
        material.SetTexture("_MainTex", frontTexture);
        material.SetTexture("_BackTex", backTexture);

        newPlane.GetComponent<MeshRenderer>().material = material;

        string meshName = "2Sided" + customTexture.name + ".asset";
        string materialName = "2Sided" + customTexture.name + ".mat";
        // ����Mesh����Դ��
        AssetDatabase.CreateAsset(mesh, $"{assetSavePath}/{meshName}");
        AssetDatabase.SaveAssets();
        // ����Material����Դ��
        AssetDatabase.CreateAsset(material, $"{assetSavePath}/{materialName}");
        AssetDatabase.SaveAssets();
    }


    public Material InitNewMaterial(Texture2D tempTexture)
    {
        Material materialInstance = new Material(defaultMaterial);
        materialInstance.SetTexture("_BaseMap", tempTexture);
        return materialInstance;
    }

    public Material frontMaterial;
    public Material backMaterial;

    [ContextMenu("Init 2Side Paper GameObject")]
    void Init2Side()
    {
        // ����ƽ��
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        // ��ȡƽ�����Ⱦ�����
        Renderer renderer = plane.GetComponent<Renderer>();

        // ����˫�����
        Material twoSidedMaterial = new Material(frontMaterial);
        twoSidedMaterial.doubleSidedGI = true;

        // ��������ͱ������ͼ
        twoSidedMaterial.SetTexture("_MainTex", frontMaterial.mainTexture);
        twoSidedMaterial.SetTexture("_BackTex", backMaterial.mainTexture);

        // Ӧ��˫����ʵ�ƽ�����Ⱦ��
        renderer.sharedMaterial = twoSidedMaterial;
    }

    void Start()
    {
        // ����ƽ��
        GameObject plane = new GameObject("Plane");
        MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();

        // ��������
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, 0, -0.5f),
            new Vector3(0.5f, 0, -0.5f),
            new Vector3(0.5f, 0, 0.5f),
            new Vector3(-0.5f, 0, 0.5f)
        };
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        // ������������
        Mesh backMesh = Instantiate(mesh);
        backMesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };

        // ��������
        Material frontMatInstance = new Material(frontMaterial);
        Material backMatInstance = new Material(backMaterial);

        // ������ͼ
        frontMatInstance.SetTexture("_MainTex", frontMaterial.mainTexture);
        backMatInstance.SetTexture("_MainTex", backMaterial.mainTexture);

        // ������Ͳ���Ӧ�õ�������Ⱦ��
        meshFilter.mesh = mesh;
        meshRenderer.sharedMaterials = new Material[] { frontMatInstance, backMatInstance };
    }
}
