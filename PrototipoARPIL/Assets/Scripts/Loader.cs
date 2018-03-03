using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour 
{
    static Loader _instance;
    public static Loader Instance{get{_instance;}}


    const string PATH       = "Path";
    const string LEVEL      = "LevelDesign";
    const string AR         = "AR";
    const string MECHANICS  = "Mechanics"; 
    const string SOUNDS     = "Sounds";

    public bool Path = true;
    public bool Level = true;
    public bool Ar = true;
    public bool Mechanics = true;
    public bool Sounds = true;

    public System.EventHandler OnAllScenesLoaded;

    void Awake()
    {
        _instance = this;
    }


	void Start () 
    {
        if(Path) {
            SceneManager.LoadScene(PATH, LoadSceneMode.Additive);
            Debug.Log("PATH Scene Loaded!");
        }

        if(Level)
        {
            SceneManager.LoadScene(LEVEL, LoadSceneMode.Additive);
            Debug.Log("Level Design Scene Loaded!");
        }

        if(Ar){
            SceneManager.LoadScene(AR, LoadSceneMode.Additive);
            Debug.Log("AR Scene Loaded!");
        }

        if(Mechanics){
            SceneManager.LoadScene(MECHANICS, LoadSceneMode.Additive);
            Debug.Log("Mechanics Scene Loaded!");
        }

        if(Sounds){
            SceneManager.LoadScene(SOUNDS, LoadSceneMode.Additive);
            Debug.Log("Sounds Scene Loaded!");
        }

        if(OnAllScenesLoaded!=null)
            OnAllScenesLoaded(this, System.EventArgs.Empty);

        Destroy(gameObject,1f);
	}


    /*
     * MODO DE USO: 
    void Start()
    {
        Loader.Instance.OnAllScenesLoaded += CuandoSeCargaronTodasLasEscenas;
    }

    void CuandoSeCargaronTodasLasEscenas(object source, System.EventArgs args) {
        // poner codigo aqui
    }
    */
}
