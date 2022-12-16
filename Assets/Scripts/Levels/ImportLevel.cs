using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapCell
{
    /// <summary>
    /// The Unity Game object that represents this cell in the world
    /// </summary>
    public GameObject gameObject;

    /// <summary>
    /// The Script that provides the functionality for this cell
    /// </summary>
    public BaseGridObject gridObject;
}

public struct MapPrefabs
{
    public GameObject wallPrefab;

    public GameObject goalPrefab;

    public GameObject[] obsticles;

    public GameObject startPrefab;

    public GameObject portalPrefab;
}

public class ImportLevel : MonoBehaviour
{
    public GameObject floor;
    public GameObject root;
    public Material livingroomFloorMaterial;
    public Material kitchenFloorMaterial;
    public Material labFloorMaterial;
    public GameObject wallPrefab;
    public GameObject kitchenWallPrefab;
    public GameObject labWallPrefab;
    public GameObject goalPrefab;
    public GameObject startPrefab;
    public GameObject portalPrefab;

    public GameObject[] plantPrefabs;
    public GameObject[] trashcanPrefabs;
    public GameObject[] labObstaclePrefabs;

    // the awake method serves to move things from the unity editor to static variables
    void Awake()
    {
        livingroomPrefabs = new MapPrefabs()
        {
            wallPrefab = wallPrefab,
            goalPrefab = goalPrefab,
            obsticles = plantPrefabs,
            startPrefab = startPrefab,
            portalPrefab = portalPrefab
        };

        kitchenPrefabs = new MapPrefabs()
        {
            wallPrefab = kitchenWallPrefab,
            goalPrefab = goalPrefab,
            obsticles = trashcanPrefabs,
            startPrefab = startPrefab,
            portalPrefab = portalPrefab
        };

        labPrefabs = new MapPrefabs()
        {
            wallPrefab = labWallPrefab,
            goalPrefab = goalPrefab,
            obsticles = labObstaclePrefabs,
            startPrefab = startPrefab,
            portalPrefab = portalPrefab
        };

        _floor = floor;
        _root = root;

        _livingroomFloorMaterial = livingroomFloorMaterial;
        _kitchenFloorMaterial = kitchenFloorMaterial;
        _labFloorMaterial = labFloorMaterial;
    }
    static readonly string[] levelFileNames = {
        "Level01",
        "Level02",
        "Level03",
        "Level04",
        "Level05",
        "Level06",
        "Level07",
        "Level08",
        "Level09",
        "Level10",
        "Level11",
        "Level12",
        "Level13",
        "Level14",
        "Level15",
        "Level16",
        "Level17",
        "Level18",
        "Level19",
        "Level20",
        "Level21",
        "Level22",
        "Level23",
        "Level24",
        "Level25",
        "Level26",
        "Level27",
        "Level28",
        "Level29",
        "Level30",
        "Level31",
        "Level32",
        "Level33",
        "Level34",
        "Level35",
        "Level36",
    };

    static string[,] readCSV(string filename)
    {
        string text = Resources.Load<TextAsset>(filename).text;
        string[] lines = text.Split('\n');

        string[,] data = new string[lines.Length, lines[0].Split(',').Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            for (int j = 0; j < line.Length; j++)
            {
                data[i, j] = line[j];
            }
        }

        return data;
    }

    static MapPrefabs livingroomPrefabs;
    static MapPrefabs kitchenPrefabs;
    static MapPrefabs labPrefabs;
    static GameObject _floor;
    static GameObject _root;
    static Material _livingroomFloorMaterial;
    static Material _kitchenFloorMaterial;
    static Material _labFloorMaterial;

    public static readonly Color[] portalColors = {
        new Color(0.8f, 0.0f, 0.0f), // Red
        new Color(0.8f, 0.3f, 0.0f), // Orange
        new Color(1.0f, 0.8f, 0.0f), // Yellow
        new Color(0.2f, 0.8f, 0.0f), // Light Green
        new Color(0.0f, 0.6f, 0.5f), // Teal
        new Color(0.0f, 0.1f, 0.8f), // Blue
        new Color(0.4f, 0.0f, 0.9f), // Purple
        new Color(1.0f, 1.0f, 1.0f),
        new Color(1.0f, 1.0f, 1.0f),
        new Color(1.0f, 1.0f, 1.0f),
    };

    public static MapCell[,] Import(uint level)
    {
        MapPrefabs mapPrefabs;

        MeshRenderer floorMeshRenderer = _floor.GetComponent<MeshRenderer>();

        if (level < GameData.GD.getLevelStartID(1))
        {
            mapPrefabs = livingroomPrefabs;
            floorMeshRenderer.material = _livingroomFloorMaterial;
        }
        else if (level < GameData.GD.getLevelStartID(2))
        {
            mapPrefabs = kitchenPrefabs;
            floorMeshRenderer.material = _kitchenFloorMaterial;
        }
        else
        {
            mapPrefabs = labPrefabs;
            floorMeshRenderer.material = _labFloorMaterial;
        }

        GameObject wallPrefab = mapPrefabs.wallPrefab;
        GameObject goalPrefab = mapPrefabs.goalPrefab;
        GameObject[] obsticles = mapPrefabs.obsticles;
        GameObject startPrefab = mapPrefabs.startPrefab;
        GameObject portalPrefab = mapPrefabs.portalPrefab;

        string[,] fileData = readCSV("Levels/" + levelFileNames[level % levelFileNames.Length]);

        int hight = fileData.GetLength(0);
        int width = fileData.GetLength(1);

        _floor.transform.localScale = new Vector3(width / 10.0f, 1, hight / 10.0f);
        floorMeshRenderer.material.mainTextureScale = new Vector2(width, hight);

        MapCell[,] grid = new MapCell[hight, width];

        Vector3 wallOffset = new Vector3(0.5f, 0, -0.5f);

        Vector3 gridOffset = new Vector3(-width / 2.0f, 0, hight / 2.0f);

        _root.transform.position = gridOffset + wallOffset;

        (int x, int z)?[,] portals = new (int x, int z)?[10, 2];

        (int, int)? playerlocation = null;

        int numberOfMesses = 0;

        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject newObject = null;
                switch (fileData[i, j][0])
                {
                    case '#':
                        newObject = Instantiate(wallPrefab);
                        break;
                    case '*':
                        newObject = Instantiate(goalPrefab);
                        numberOfMesses++;
                        break;
                    case 'O':
                        GameObject prefab;
                        if (fileData[i, j].Length > 2 && '0' <= fileData[i, j][2] && fileData[i, j][2] <= '9')
                        {
                            prefab = obsticles[fileData[i, j][2] - '0'];
                        }
                        else
                        {
                            prefab = obsticles[Random.Range(0, obsticles.Length)];
                        }
                        newObject = Instantiate(prefab);
                        break;
                    case 'P':
                        if (fileData[i, j].Length > 2 && '0' <= fileData[i, j][2] && fileData[i, j][2] <= '9')
                        {
                            newObject = Instantiate(portalPrefab);
                            int portalColor = fileData[i, j][2] - '0';

                            Material portalMaterial = newObject.GetComponent<Portal>().coloredPart.GetComponent<Renderer>().material;

                            portalMaterial.color = portalColors[portalColor];
                            portalMaterial.SetColor("_EmissionColor", portalColors[portalColor]);

                            if (portals[portalColor, 0] == null)
                            {
                                portals[portalColor, 0] = (j, i);
                            }
                            else if (portals[portalColor, 1] == null)
                            {
                                portals[portalColor, 1] = (j, i);
                            }
                            else
                            {
                                Debug.LogError("Portal color " + portalColor + " is already taken");
                            }
                        }
                        else
                        {
                            Debug.LogError("Portal color not specified");
                        }

                        break;
                    case 'S':
                        if (playerlocation == null)
                        {
                            playerlocation = (j, i);

                            newObject = Instantiate(startPrefab);
                        }
                        else
                        {
                            Debug.LogError("Multiple player locations found");
                        }
                        break;
                    case ' ':
                        break;
                    default:
                        Debug.LogError("Invalid character in level file");
                        break;
                }

                if (newObject != null)
                {
                    newObject.transform.parent = _root.transform;
                    newObject.transform.localPosition = new Vector3(j, 0, -i);

                    if (fileData[i, j].Length > 1 && '0' <= fileData[i, j][1] && fileData[i, j][1] <= '4')
                    {
                        newObject.transform.eulerAngles = new Vector3(0, (fileData[i, j][1] - '0') * 90, 0);
                    }
                    else
                    {
                        newObject.transform.eulerAngles = new Vector3(0, Random.Range(0, 4) * 90, 0);
                    }

                    grid[i, j] = new MapCell()
                    {
                        gameObject = newObject,
                        gridObject = newObject.GetComponent<BaseGridObject>()
                    };
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (portals[i, 0] != null && portals[i, 1] != null)
            {
                Portal portal1 = (Portal)grid[portals[i, 0].Value.z, portals[i, 0].Value.x].gridObject;
                Portal portal2 = (Portal)grid[portals[i, 1].Value.z, portals[i, 1].Value.x].gridObject;

                portal1.destination = portals[i, 1].Value;
                portal2.destination = portals[i, 0].Value;
            }
            else if (portals[i, 0] != null || portals[i, 1] != null)
            {
                Debug.LogError("Portal color " + i + " is not connected");
            }
        }

        if (playerlocation == null)
        {
            Debug.LogError("No player location found");
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            (int x, int z) = playerlocation.Value;
            player.transform.position = new Vector3(x, 0, -z) + gridOffset + wallOffset;
            Player.P.setPlayerStart(z, x);
            Player.P.playerControl = true;
        }

        Main.S.numMess = numberOfMesses;

        /// the hight of the walls used for camera position
        const float WALL_HIGHT = 3;

        if (hight * Camera.main.aspect > width)
        {
            // hight limited
            Camera.main.transform.position = new Vector3(0.0f, hight * Main.S.cameraDistanceConfidant + WALL_HIGHT, 0.0f);
        }
        else
        {
            // width limited
            Camera.main.transform.position = new Vector3(0.0f, width / Camera.main.aspect * Main.S.cameraDistanceConfidant + WALL_HIGHT, 0.0f);
        }

        return grid;
    }
}
