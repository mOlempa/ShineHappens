using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject currentGem;

    [SerializeField]
    public GemParticles gemParticles;


    public int knobPoints = 0;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Game Manager is NULL");
            }
            return _instance;
        }
    }
    public void Awake()
    {
        _instance = this;
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        knobPoints = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    string name = hit.collider.gameObject.name;
                    Debug.Log("Hit " + name);

                    if (hit.collider.gameObject.CompareTag("Client"))
                    {
                        print("Client says hi.");
                        hit.collider.gameObject.GetComponent<ClientInteract>().startInteraction();
                    }
                }
            }
        }
    }


}
