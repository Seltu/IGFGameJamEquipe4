using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Transform player;
    public int length, width, roomCount;
    public GameObject startRoom;
    public List<GameObject> rooms;
    public GameObject finalRoom;
    private List<Vector2Int> positions;
    public GameObject doorPrefab;
    public GameObject wallPrefab;
    public float roomsize;
    [SerializeField]
    private bool[][] roomPos;
    public int roomLength = 30;
    public int roomWidth = 30;
    private void Awake()
    {
        roomPos =  new bool[length][];
        for (int i = 0; i < roomPos.Length; i++)
        {
            roomPos[i] = new bool[width];
        }
        positions = new List<Vector2Int>();
        for(int i = 0; i < roomCount; i++)
        {
            Vector2Int position = new Vector2Int();
            if (i == 0)
            {
                position = new Vector2Int(Random.Range(0, length), Random.Range(0, width));
                roomPos[position.x][position.y] = true;
                positions.Add(position);
            }
            else
            {
                List<Vector2Int> adjacent = new List<Vector2Int>();
                while (adjacent.Count == 0)
                {
                    var index = Random.Range(0, positions.Count);
                    position = positions[index];
                    if (position.x + 1 < length)
                        if (roomPos[position.x+1][position.y] == false)
                        adjacent.Add(new Vector2Int(position.x+1, position.y));
                    if (position.x - 1 >= 0)
                        if (roomPos[position.x-1][position.y] == false)
                        adjacent.Add(new Vector2Int(position.x-1, position.y));
                    if (position.y + 1 < width)
                        if (roomPos[position.x][position.y+1] == false)
                        adjacent.Add(new Vector2Int(position.x, position.y+1));
                    if (position.y - 1 >= 0)
                        if (roomPos[position.x][position.y-1] == false)
                        adjacent.Add(new Vector2Int(position.x, position.y-1));
                    if (adjacent.Count == 0)
                    {
                        positions.RemoveAt(index);
                    }
                }
                Vector2Int newPos = adjacent[Random.Range(0, adjacent.Count)];
                roomPos[newPos.x][newPos.y] = true;
                positions.Add(newPos);
            }
        }
        GameObject last = new GameObject();
        bool flag = true;
        for (int i = 0; i < roomPos.Length; i++)
        {
            for (int j = 0; j < roomPos[i].Length; j++)
            {
                if (roomPos[i][j] == true)
                {
                    GameObject room;

                    if (flag)
                    {
                        room = Instantiate(finalRoom, transform);
                        flag = false;
                    }
                    else
                    {
                        var index = Random.Range(0, rooms.Count);
                        room = Instantiate(rooms[index], transform);
                    }

                    room.transform.position = new Vector3(roomLength * i * roomsize, 0, roomWidth * j * roomsize);
                    last = room.gameObject;

                    // Check adjacent rooms and instantiate doors or walls accordingly

                    // Check left
                    if (i > 0 && roomPos[i - 1][j]) // Room to the left
                    {
                        Instantiate(doorPrefab, room.transform.position + Vector3.left * roomLength / 2, Quaternion.identity, transform);
                    }
                    else if (i >= 0) // No room to the left
                    {
                        Instantiate(wallPrefab, room.transform.position + Vector3.left * roomLength / 2, Quaternion.identity, transform);
                    }

                    // Check down
                    if (j > 0 && roomPos[i][j - 1]) // Room below
                    {
                        var door = Instantiate(doorPrefab, room.transform.position + Vector3.back * roomWidth / 2, Quaternion.identity, transform);
                        door.transform.rotation = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees
                    }
                    else if (j >= 0) // No room below
                    {
                        var wall = Instantiate(wallPrefab, room.transform.position + Vector3.back * roomWidth / 2, Quaternion.identity, transform);
                        wall.transform.rotation = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees
                    }

                    // Check right (place wall only if no adjacent room exists or if it's the last column)
                    if (i < roomPos.Length - 1)
                    {
                        if (!roomPos[i + 1][j]) // No room to the right
                        {
                            Instantiate(wallPrefab, room.transform.position + Vector3.right * roomLength / 2, Quaternion.identity, transform);
                        }
                    }
                    else
                        Instantiate(wallPrefab, room.transform.position + Vector3.right * roomLength / 2, Quaternion.identity, transform);

                    // Check up (place wall only if no adjacent room exists or if it's the last row)
                    if (j < roomPos[i].Length - 1)
                    {
                        if (!roomPos[i][j + 1]) // No room above
                        {
                            var wall = Instantiate(wallPrefab, room.transform.position + Vector3.forward * roomWidth / 2, Quaternion.identity, transform);
                            wall.transform.rotation = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees
                        }
                    }
                    else
                    {
                        var wall = Instantiate(wallPrefab, room.transform.position + Vector3.forward * roomWidth / 2, Quaternion.identity, transform);
                        wall.transform.rotation = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees
                    }
                }
            }
        }
        var firstRoom = Instantiate(startRoom, transform);
        firstRoom.transform.position = last.transform.position;
        last.transform.position = Vector3.up * 9999;
        Destroy(last);
        player.position = firstRoom.transform.position;
    }
}