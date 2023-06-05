using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class PaperArtGenerator : MonoBehaviour
{
    public string assetSavePath = "Assets/PaperObjs/MeshAndMat";
    public float scaleOffSetRatio;
    public Material defaultMaterial; // 共享的材质

    [Header("Quad Generator")]
    public Texture2D planeTexture; // 自定义贴图数组

    [Header("Box Generator")]
    public Texture2D cubeTexture;
    public Vector3 cubeScale;

    [ContextMenu("Init Paper GameObject")]
    public void InitPlane()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, planeTexture.height/ scaleOffSetRatio);
        vertices[2] = new Vector3(planeTexture.width/ scaleOffSetRatio, planeTexture.height/ scaleOffSetRatio);
        vertices[3] = new Vector3(planeTexture.width/ scaleOffSetRatio, 0);

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

        string gameObjectName = planeTexture.name;
        GameObject newPlane = new GameObject(gameObjectName, typeof(MeshFilter), typeof(MeshRenderer));
        newPlane.GetComponent<MeshFilter>().mesh = mesh;
        Material material = InitNewMaterial(planeTexture);
        newPlane.GetComponent<MeshRenderer>().material = material;

        string meshName = planeTexture.name + ".asset";
        string materialName = planeTexture.name + ".mat";
        // 保存Mesh到资源库
        AssetDatabase.CreateAsset(mesh, $"{assetSavePath}/{meshName}");
        AssetDatabase.SaveAssets();
        // 保存Material到资源库
        AssetDatabase.CreateAsset(material, $"{assetSavePath}/{materialName}");
        AssetDatabase.SaveAssets();
    }

    public Material InitNewMaterial(Texture2D tempTexture)
    {
        Material materialInstance = new Material(defaultMaterial);
        materialInstance.SetTexture("_BaseMap", tempTexture);
        return materialInstance;
    }

    //[ContextMenu("Init Cube GameObject")]
    //public void InitCube()
    //{
    //    Mesh cubeMesh = new Mesh();
    //    Vector3[] vertices = GenerateVertices();
    //    Vector2[] uv = GenerateUV();
    //    int[] triangles = GenerateTriangles();

    //    cubeMesh.vertices = vertices;
    //    cubeMesh.uv = uv;
    //    cubeMesh.triangles = triangles;
    //    cubeMesh.RecalculateNormals();
    //    cubeMesh.RecalculateBounds();
    //    //cubeMesh.nor

    //    string gameObjectName = cubeTexture.name;
    //    GameObject newCube = new GameObject(gameObjectName, typeof(MeshFilter), typeof(MeshRenderer));
    //    newCube.GetComponent<MeshFilter>().mesh = cubeMesh;
    //    newCube.GetComponent<MeshRenderer>().material = InitNewMaterial(cubeTexture);

    //    Vector3[] GenerateVertices()
    //    {
    //        return new Vector3[]
    //        {
    //            //Bottom
    //            new Vector3(-1,0,1),   //0
    //            new Vector3(1,0,1),    //1
    //            new Vector3(1,0,-1),   //2
    //            new Vector3(-1,0,-1),  //3
    //            //Top                
    //            new Vector3(-1,2,1),   //4
    //            new Vector3(1,2,1),    //5
    //            new Vector3(1,2,-1),   //6
    //            new Vector3(-1,2,-1),  //7
    //            //Left               
    //            new Vector3(-1,0,1),   //8
    //            new Vector3(-1,0,-1),  //9
    //            new Vector3(-1,2,-1),  //10
    //            new Vector3(-1,2,1),   //11
    //            //Right               
    //            new Vector3(1,0,1),    //12
    //            new Vector3(1,0,-1),   //13
    //            new Vector3(1,2,-1),   //14
    //            new Vector3(1,2,1),    //15
    //            //Front               
    //            new Vector3(1,0,-1),   //16
    //            new Vector3(-1,0,-1),  //17
    //            new Vector3(-1,2,-1),  //18
    //            new Vector3(1,2,-1),   //19 
    //            //Back              
    //            new Vector3(1,0,1),    //20
    //            new Vector3(-1,0,1),   //21
    //            new Vector3(-1,2,1),   //22
    //            new Vector3(1,2,1),    //23
    //        };
    //    }

    //    Vector2[] GenerateUV()
    //    {
    //        float horiLength = cubeScale.z * 2 + cubeScale.x * 2;
    //        float verLength = cubeScale.z * 2 + cubeScale.y;
    //        float horiUnit = 1 / horiLength;
    //        float verUnit = 1 / verLength;

    //        return new Vector2[]
    //        {
    //        //Bottom
    //        new Vector2(cubeScale.z * horiUnit,(cubeScale.z+cubeScale.y) * verUnit),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)* verUnit),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,1),
    //        new Vector2(cubeScale.z* horiUnit,1),
    //        //Top                
    //        new Vector2(cubeScale.z * horiUnit,0),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,0),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
    //        new Vector2(cubeScale.z* horiUnit,cubeScale.z*verUnit),
    //        //Left               
    //        new Vector2(0,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2(cubeScale.z*horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2(cubeScale.z* horiUnit,cubeScale.z*verUnit),
    //        new Vector2(cubeScale.z*horiUnit,0),
    //        //Right               
    //        new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
    //        new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
    //        //Front               
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2(cubeScale.z* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2(cubeScale.z* horiUnit,cubeScale.z*verUnit),
    //        new Vector2((cubeScale.z+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
    //        //Back              
    //           new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
    //      new Vector2(1,(cubeScale.z+cubeScale.y)*verUnit),
    //        new Vector2(1,cubeScale.z*verUnit),
    //         new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
    //        };
    //    }

    //    int[] GenerateTriangles()
    //    {
    //        return new int[]
    //        {
    //            //Bottom/Top
    //            0,1,2,
    //            0,2,3,
    //            4,5,6,
    //            4,6,7,
    //            //Left/Right
    //            8,9,10,
    //            8,10,11,
    //            12,13,14,
    //            12,14,15,
    //            //Bottom/Top
    //            16,17,18,
    //            16,18,19,
    //            20,21,22,
    //            20,22,23,
    //        };
    //    }
    //}

    [ContextMenu("Init Cube GameObject")]
    public void InitCube()
    {
        float cx = cubeScale.x;
        float cy = cubeScale.y;
        float cz = cubeScale.z;
        Mesh cubeMesh = new Mesh();
        Vector3[] vertices = GenerateVertices();
        Vector2[] uv = GenerateUV();
        int[] triangles = GenerateTriangles();

        cubeMesh.vertices = vertices;
        cubeMesh.uv = uv;
        cubeMesh.triangles = triangles;
        cubeMesh.RecalculateNormals();
        cubeMesh.RecalculateBounds();

        string gameObjectName = cubeTexture.name;
        GameObject newCube = new GameObject(gameObjectName, typeof(MeshFilter), typeof(MeshRenderer));
        newCube.GetComponent<MeshFilter>().mesh = cubeMesh;
        newCube.GetComponent<MeshRenderer>().material = InitNewMaterial(cubeTexture);

        Vector3[] GenerateVertices()
        {
            Vector3[] vertices = new Vector3[24];
            var xyz = new Dictionary<int, float> { { 0, cx }, { 1, cy }, { 2, cz } };
            for (int i = 0, v = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    float[] tempV3 = new float[3];
                    int[] vectorIndex = null;
                    switch (i)
                    {
                        case 0: vectorIndex = new int[2] { 1, 2 }; break;
                        case 1: vectorIndex = new int[2] { 0, 2 }; break;
                        case 2: vectorIndex = new int[2] { 0, 1 }; break;
                    }
                    for (int n = 0; n < 2; n++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            tempV3[vectorIndex[0]] = (n == 0) ? xyz[vectorIndex[0]] : -xyz[vectorIndex[0]];
                            tempV3[vectorIndex[1]] = (m == 0) ? xyz[vectorIndex[1]] : -xyz[vectorIndex[1]];
                            tempV3[i] = (j == 0) ? xyz[i] : -xyz[i];
                            vertices[v] = new Vector3(tempV3[0], tempV3[1], tempV3[2]);
                            v++;
                        }
                    }
                }
            }
            return vertices;
        }

        Vector2[] GenerateUV()
        {
            float horiLength = cubeScale.z * 2 + cubeScale.x * 2;
            float verLength = cubeScale.z * 2 + cubeScale.y;
            float horiUnit = 1 / horiLength;
            float verUnit = 1 / verLength;

            return new Vector2[]
            {
                //Bottom
                new Vector2(cubeScale.z * horiUnit,(cubeScale.z+cubeScale.y) * verUnit),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)* verUnit),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,1),
                new Vector2(cubeScale.z* horiUnit,1),
                //Top                
                new Vector2(cubeScale.z * horiUnit,0),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,0),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
                new Vector2(cubeScale.z* horiUnit,cubeScale.z*verUnit),
                //Left               
                new Vector2(0,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2(cubeScale.z*horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2(cubeScale.z* horiUnit,cubeScale.z*verUnit),
                new Vector2(cubeScale.z*horiUnit,0),
                //Right               
                new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
                new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
                //Front               
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2(cubeScale.z* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2(cubeScale.z* horiUnit,cubeScale.z*verUnit),
                new Vector2((cubeScale.z+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
                //Back              
                   new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,(cubeScale.z+cubeScale.y)*verUnit),
              new Vector2(1,(cubeScale.z+cubeScale.y)*verUnit),
                new Vector2(1,cubeScale.z*verUnit),
                 new Vector2((cubeScale.z*2+cubeScale.x)* horiUnit,cubeScale.z*verUnit),
            };
        }

        int[] GenerateTriangles()
        {
            int[] tempTriVertex = new int[24];
            for (int i = 0, j = 0; i < 6; i++)
            {
                tempTriVertex[j] = j;
                tempTriVertex[j + 1] = j + 1;
                tempTriVertex[j + 2] = j + 2;
                tempTriVertex[j + 3] = j + 1;
                tempTriVertex[j + 4] = j + 2;
                tempTriVertex[j + 5] = j + 3;
                j += 4;
            }

            return tempTriVertex;
            //return new int[]
            //{
            //        //Bottom/Top
            //        0,1,2,
            //        0,2,3,
            //        4,5,6,
            //        4,6,7,
            //        //Left/Right
            //        8,9,10,
            //        8,10,11,
            //        12,13,14,
            //        12,14,15,
            //        //Bottom/Top
            //        16,17,18,
            //        16,18,19,
            //        20,21,22,
            //        20,22,23,
            //};
        }
    }
}
