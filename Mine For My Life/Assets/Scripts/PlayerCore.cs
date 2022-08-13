using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCore : MonoBehaviour
{
    public static PlayerCore main;
    void Awake() { main = this; }

    public Text score_text;
    public List<Transform> mine_positions;
    public List<Transform> generate_positions; // Make sure it's parallel to mine_positions.
    public Animator player_animator;
    public int cur_mine_pos; // This is the index of mine_positions, make sure it's not out of range.
    public float move_time; // Time in seconds to move to the next block.

    bool moving;
    Vector3[] move_pos = new Vector3[2];
    float move_elapsed; // It's now set that each movement takes 0.5 second.

    public bool game_on;
    float seconds_elapsed;

    void Start()
    {
        cur_mine_pos = mine_positions.Count-1;
    }

    void Update()
    {
        if (moving)
        {
            move_elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(move_pos[0],move_pos[1],move_elapsed / move_time);
            if (move_elapsed >= move_time)
            {
                moving = false;
                move_elapsed = 0f;
                transform.position = move_pos[1];
                AimBarCore.main.StartAimBar();
            }
        }

        if (game_on)
        {
            seconds_elapsed += Time.deltaTime;
            score_text.text = string.Format("Survived: {0}m {1}s",Mathf.FloorToInt(seconds_elapsed / 60), Mathf.FloorToInt(seconds_elapsed % 60));
        }
    }

    public void GotoNextBlock() { MoveToPos(transform.position, mine_positions[cur_mine_pos].position); }

    void MoveToPos(Vector3 start, Vector3 end)
    {
        moving = true;
        move_pos[0] = start;
        move_pos[1] = end;
    }

    public void Dead()
    {
        print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        game_on = false;
        AimBarCore.main.StopAimBar();
    }
}
