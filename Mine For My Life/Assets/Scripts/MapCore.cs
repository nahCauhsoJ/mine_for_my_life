using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCore : MonoBehaviour
{
    public static MapCore main;

    public Tilemap block_tiles;

    [Header("Block's tile for matching and generating")]
    public List<Tile> stone_tiles = new List<Tile>();
    public List<Tile> dirt_tiles = new List<Tile>();
    public List<Tile> web_tiles = new List<Tile>();

    // Yes, I'm using the sprite to determine the block type.
    List<Sprite> stone_sprites = new List<Sprite>();
    List<Sprite> dirt_sprites = new List<Sprite>();
    List<Sprite> web_sprites = new List<Sprite>();

    void Awake()
    {
        main = this;
        foreach (var i in stone_tiles) stone_sprites.Add(i.sprite);
        foreach (var i in dirt_tiles) dirt_sprites.Add(i.sprite);
        foreach (var i in web_tiles) web_sprites.Add(i.sprite);
    }

    void Start()
    {
        // Here's to pre-generate the tiles on screen.
        for (var i = 4f; i < 14; i+=2 ) GenerateBlocks(Vector2.right * i);
    }

    public void EndStartCutscene()
    {
        AimBarCore.main.StartAimBar();
        PlayerCore.main.score_text.gameObject.SetActive(true);
        PlayerCore.main.game_on = true;
        LavaCore.main.transform.position = new Vector3(-6f,0f,0f);
        LavaCore.main.StartFlow();
    }

    // false = gets nothing. true = NewBlock() in AimBarCore is run.
    public bool SeekBlock(Vector2 pos)
    {
        Sprite block = block_tiles.GetSprite(block_tiles.WorldToCell(pos));
        if (block == null) return false;
        if (stone_sprites.Contains(block)) AimBarCore.main.NewBlock("stone",100f);
        else if (dirt_sprites.Contains(block)) AimBarCore.main.NewBlock("dirt",100f);
        else if (web_sprites.Contains(block)) AimBarCore.main.NewBlock("web",100f);
        else return false;
        return true;
    }

    public bool DestroyBlock(Vector2 pos)
    {
        if (block_tiles.GetSprite(block_tiles.WorldToCell(pos)) == null) return false;
        block_tiles.SetTile(block_tiles.WorldToCell(pos), null);
        return true;
    }

    public void GenerateBlocks(Vector2 pos)
    {
        Vector3Int block_pos = block_tiles.WorldToCell(pos);
        for (var i = -3; i < 4; i++)
        {
            switch (Random.Range(0,1f))
            {
                case <= 0.1f: block_tiles.SetTile(block_pos - Vector3Int.up * i, web_tiles[0]); break;
                case <= 0.5f: block_tiles.SetTile(block_pos - Vector3Int.up * i, dirt_tiles[0]); break;
                case <= 1f: block_tiles.SetTile(block_pos - Vector3Int.up * i, stone_tiles[0]); break;
            }

            
        }
        

        switch (Random.Range(0,3))
        {
            case 0: block_tiles.SetTile(block_pos, stone_tiles[0]); break;
            case 1: block_tiles.SetTile(block_pos, dirt_tiles[0]); break;
            case 2: block_tiles.SetTile(block_pos, web_tiles[0]); break;
        }
    }
}
