using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGroup : MonoBehaviour {
    public SingleCubeInBackground cubePrefab;
    public int cubeCountX;
    public int cubeCountY;
    public float scaleXZ;
    public MinMaxData scaleY;
    public float distY;

    public Material[] materials;

    bool created = false;
    SingleCubeInBackground[] cubeList;

    private void Awake() {
        CreateCubes();
    }

    public void CreateCubes() {
        if (created) return;
        created = true;

        int cubeCount = cubeCountX * cubeCountY;
        int materialCount = materials.Length;
        cubeList = new SingleCubeInBackground[cubeCountX * cubeCountY];

        for (int x=0; x<cubeCountX; ++x) {
            for (int y = 0; y < cubeCountY; ++y) {
                var cube = Instantiate(cubePrefab, transform);
                cube.transform.localPosition = new Vector3(x - ((float)cubeCountX) / 2.0f,
                    0, y - ((float)cubeCountX) / 2.0f);
                cube.transform.localRotation = Quaternion.identity;
                cube.transform.localScale = new Vector3(1, scaleY.GetRandomBetweenRange(), 1);
                cube.m_Renderer.sharedMaterial = materials[Random.Range(0, materialCount)];
                cubeList[y + x * cubeCountY] = cube;
            }
        }
        transform.localScale = new Vector3(scaleXZ, 1, scaleXZ); 
    }

    private void Update() {
        if(Player.current == null) {
            return;
        }

        Vector3 pos = Player.current.transform.position + new Vector3(0, -distY, 0);
        transform.position = pos;
        // And enable fog
        //RenderSettings.fog = true;

        if (BackgroundMaterial.current && BackgroundMaterial.current.gameObject.activeSelf) {
            Color col = BackgroundMaterial.current.GetCurrentTopColor();
            RenderSettings.fogColor = col;

            for(var i=0; i<materials.Length; ++i) {
                materials[i].color = BackgroundMaterial.current.GetCurrentCubeColor(i);
            }
            //cubeList[0].m_Renderer.sharedMaterial.color = col;
           // m_Renderer.sharedMaterial.SetColor("_EmissionColor", col / 3.0f);`
        }
    }
}
