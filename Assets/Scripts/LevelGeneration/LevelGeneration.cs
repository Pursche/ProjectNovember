using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public int rooms = 25;
    public int roomsRandomness = 5;

    public int corridorChance = 66;

    public Transform levelRoot;
    public Transform roomRoot;
    public Transform corridorRoot;

    public GameObject[] roomPrefabs;
    public GameObject[] corridorPrefabs;
    public GameObject spawnroomPrefab;
    public GameObject endcapPrefab;

    private List<GameObject> exampleRooms = new List<GameObject>();
    private List<GameObject> exampleCorridors = new List<GameObject>();

    public List<GameObject> roomPieces = new List<GameObject>(); 

    void Start()
    {
        levelRoot = new GameObject("Level Root").transform;
        roomRoot = new GameObject("Rooms").transform;
        roomRoot.transform.SetParent(levelRoot.transform);
        corridorRoot = new GameObject("Corridors").transform;
        corridorRoot.transform.SetParent(levelRoot.transform);

        // Create one of each room and corridor for later collision testing
        exampleRooms = new List<GameObject>();
        foreach (GameObject room in roomPrefabs)
        {
            GameObject exampleRoom = Instantiate(room, new Vector3(0, -250, 0), Quaternion.identity);
            MakeInvisible(exampleRoom.transform);
            exampleRooms.Add(exampleRoom);
        }
        exampleCorridors = new List<GameObject>();
        foreach (GameObject corridor in corridorPrefabs)
        {
            GameObject exampleCorridor = Instantiate(corridor, new Vector3(0, -250, 0), Quaternion.identity);
            MakeInvisible(exampleCorridor.transform);
            exampleCorridors.Add(exampleCorridor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Generate());
        }
    }

    void AddOpenDoors(GameObject obj, ref List<Transform> list, bool ignoreFirst = true)
    {
        foreach (Transform door in obj.transform.FindChild("Doors"))
        {
            if (ignoreFirst)
            {
                ignoreFirst = false;
                continue;
            }
            list.Add(door);
        }
    }
    
    void LinkDoors(Transform door, GameObject newRoom)
    {
        Transform moveDoor = newRoom.transform.FindChild("Doors").GetChild(0);

        Quaternion moveDoorToParentRot = Quaternion.FromToRotation(newRoom.transform.eulerAngles, moveDoor.eulerAngles);
        newRoom.transform.rotation = door.rotation * moveDoorToParentRot;

        Vector3 moveDoorToParentPos = moveDoor.position - newRoom.transform.position;
        newRoom.transform.position = door.position - moveDoorToParentPos;

    }

    void ClearLevel()
    {
        foreach (Transform child in roomRoot)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in corridorRoot)
        {
            Destroy(child.gameObject);
        }
    }

    void MakeInvisible(Transform room)
    {
        MeshRenderer renderer = room.GetComponent<MeshRenderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }

        foreach (Transform child in room)
        {
            MakeInvisible(child);
        }
        
    }

    bool CollisionCheck(Transform door, GameObject newRoom)
    {
        LinkDoors(door, newRoom);
        Collider[] roomBounds = newRoom.transform.FindChild("Bounds").GetComponentsInChildren<Collider>();

        bool noCollision = true;
        foreach (GameObject existingRoom in roomPieces)
        {
            Collider[] existingRoomBounds = existingRoom.transform.FindChild("Bounds").GetComponentsInChildren<Collider>();

            foreach (Collider existingRoomBound in existingRoomBounds)
            {
                foreach (Collider roomBound in roomBounds)
                {
                    if (existingRoomBound.bounds.Intersects(roomBound.bounds))
                    {
                        newRoom.transform.position = new Vector3(0, -250, 0);
                        return true;
                    }
                }
            }
        }

        newRoom.transform.position = new Vector3(0, -250, 0);
        return false;
    }

    bool FindCorridorToSpawn(Transform selectedDoor, ref GameObject corridorToSpawn)
    {
        List<KeyValuePair<int, GameObject>> possibleCorridors = new List<KeyValuePair<int, GameObject>>();
        for (int i = 0; i < corridorPrefabs.Length; i++)
        {
            possibleCorridors.Add(new KeyValuePair<int, GameObject>(i, corridorPrefabs[i]));
        }

        corridorToSpawn = null;
        while (corridorToSpawn == null)
        {
            if (possibleCorridors.Count > 0)
            {
                int possibleCorridorID = Random.Range(0, possibleCorridors.Count);
                int actualCorridorID = possibleCorridors[possibleCorridorID].Key;

                if (!CollisionCheck(selectedDoor, exampleCorridors[actualCorridorID])) // If the randomized corridor didn't collide with anything
                {
                    corridorToSpawn = corridorPrefabs[actualCorridorID];
                    return true;
                }
                else
                {
                    possibleCorridors.RemoveAt(possibleCorridorID);
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    bool FindRoomToSpawn(Transform selectedDoor, ref GameObject roomToSpawn)
    {
        List<KeyValuePair<int, GameObject>> possibleRooms = new List<KeyValuePair<int, GameObject>>();
        for (int i = 0; i < roomPrefabs.Length; i++)
        {
            possibleRooms.Add(new KeyValuePair<int, GameObject>(i, roomPrefabs[i]));
        }

        roomToSpawn = null;
        while (roomToSpawn == null)
        {
            if (possibleRooms.Count > 0)
            {
                int possibleRoomID = Random.Range(0, possibleRooms.Count);
                int actualRoomID = possibleRooms[possibleRoomID].Key;

                if (!CollisionCheck(selectedDoor, exampleRooms[actualRoomID])) // If the randomized corridor didn't collide with anything
                {
                    roomToSpawn = roomPrefabs[actualRoomID];
                    return true;
                }
                else
                {
                    possibleRooms.RemoveAt(possibleRoomID);
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    IEnumerator Generate()
    {
        ClearLevel();
        roomPieces.Clear();

        // Decide how many rooms we need to spawn
        int roomsToSpawn = rooms + Random.Range(-roomsRandomness, roomsRandomness);

        // Initialize lists
        List<GameObject> createdRooms = new List<GameObject>();
        List<GameObject> createdCorridors = new List<GameObject>();
        List<Transform> openDoors = new List<Transform>();

        // Create a room to start in
        GameObject spawnRoom = Instantiate(spawnroomPrefab, Vector3.zero, Quaternion.identity);
        spawnRoom.transform.SetParent(roomRoot);
        AddOpenDoors(spawnRoom, ref openDoors, false);
        createdRooms.Add(spawnRoom);
        roomPieces.Add(spawnRoom);

        while (openDoors.Count > 0)
        {
            // Select which open door to spawn something to
            Transform selectedDoor = openDoors[0];

            if (roomsToSpawn > 0)
            {
                bool spawnCorridor = Random.Range(0, 100) < corridorChance;

                if (openDoors.Count <= 1 && roomsToSpawn > 1) // This check makes sure that we don't close the last door with too many rooms left to spawn
                    spawnCorridor = true;
                    
                if (spawnCorridor)
                {
                    GameObject toSpawn = null;
                    if (!FindCorridorToSpawn(selectedDoor, ref toSpawn))
                    {
                        if (!FindRoomToSpawn(selectedDoor, ref toSpawn))
                        {
                            toSpawn = endcapPrefab;
                        }
                        else
                        {
                            roomsToSpawn--;
                        }
                    }

                    GameObject newCorridor = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
                    AddOpenDoors(newCorridor, ref openDoors);
                    LinkDoors(selectedDoor, newCorridor);
                    newCorridor.transform.SetParent(corridorRoot);
                    roomPieces.Add(newCorridor);
                }
                else
                {
                    GameObject toSpawn = null;
                    if (!FindRoomToSpawn(selectedDoor, ref toSpawn))
                    {
                        if (!FindCorridorToSpawn(selectedDoor, ref toSpawn))
                        {
                            toSpawn = endcapPrefab;
                        }
                    }

                    GameObject newRoom = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
                    AddOpenDoors(newRoom, ref openDoors);
                    LinkDoors(selectedDoor, newRoom);
                    newRoom.transform.SetParent(roomRoot);
                    roomPieces.Add(newRoom);

                    roomsToSpawn--;
                }
            }
            else // If we don't have more rooms to spawn, start closing up doorways.
            {
                // Plug all open doors to finish the level
                GameObject newEndcap = Instantiate(endcapPrefab, selectedDoor.position, selectedDoor.rotation);
                newEndcap.transform.SetParent(corridorRoot);
            }

            openDoors.Remove(selectedDoor);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
