using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    public int width;
    public bool showNoiseMap;
    //public GameObject dirt, grass, stone, ore;
    public Tilemap dirtTilemap, grassTilemap, stoneTilemap, noiseTilemap;
    public Tile dirt, grass, stone, noise;
    [Range(1, 150)]
    public float height, smoothness;
    float seed;
    // Start is called before the first frame update
    void Start()
    {
        generateSeed();
        generation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            stoneTilemap.ClearAllTiles();
            dirtTilemap.ClearAllTiles();
            grassTilemap.ClearAllTiles();
            noiseTilemap.ClearAllTiles();
            generateSeed();
            generation();
        }
    }

    private void generateSeed()
    {
        seed = Random.Range(-1000000, 1000000);
        Debug.Log("Seed: " + seed);
    }

    private void generation()
    {
        for (int x = 0; x < width; x++) // manages horizontal generation
        {
            // basic procedural generation without noise map
            /*int min_height = height - 1;
            int max_height = height + 2;
            height = Random.Range(min_height, max_height);*/

            // procedural generation with noise map

            int cur_height = Mathf.RoundToInt(height * Mathf.PerlinNoise(x / smoothness, seed));
            //Debug.Log(height * Mathf.PerlinNoise(x / smoothness, 0));

            int max_stone_spawn = cur_height - 5;
            int min_stone_spawn = cur_height - 8;
            int stone_spawn = Random.Range(min_stone_spawn, max_stone_spawn);
            int max_ore_spawn = cur_height - 8;

            if (showNoiseMap)
            {
                for (int n = Mathf.RoundToInt(height + 15); n < Mathf.RoundToInt(height + 55); n++)
                {
                    // tests for fun
                    //int testvalue = Mathf.RoundToInt(height * Mathf.PerlinNoise((x+(n/(height+15))) / smoothness, seed));
                    //float shade = testvalue / height;

                    //spawn noise map tile at x=x, y=n with color = cur_height/height?
                    noiseTilemap.SetTile(new Vector3Int(x, n, 0), noise); // spawning tile
                    noiseTilemap.SetTileFlags(new Vector3Int(x, n, 0), TileFlags.None);
                    float shade = cur_height / height;
                    noiseTilemap.SetColor(new Vector3Int(x, n, 0), new Color(shade, shade, shade));
                }
            }

            for (int y = 0; y < cur_height; y++) // manages vertical generation
            {
                int is_ore = Random.Range(0, 12);
                if (y < stone_spawn)
                {
                    if (is_ore == 1 && y < max_ore_spawn)
                    {
                        //spawnObj(ore, x, y);
                        stoneTilemap.SetTile(new Vector3Int(x, y, 0), stone); // spawning stone instead of ore right now
                    } else
                    {
                        //spawnObj(stone, x, y);
                        stoneTilemap.SetTile(new Vector3Int(x, y, 0), stone);
                    }
                } else
                {
                    //spawnObj(dirt, x, y);
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
                }
            }
            //spawnObj(grass, x, height);
            grassTilemap.SetTile(new Vector3Int(x, cur_height, 0), grass);
        } 
    }

    void spawnObj(GameObject obj, int width, int height) // not used anymore, kept in code for reference
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
