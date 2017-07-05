using nseh.Managers;
using UnityEngine;

public class TEST : MonoBehaviour
{
    MenuManager _MenuManager;
    GameObject _prospector;
    // Use this for initialization
    void Start ()
    {
        _MenuManager = nseh.Managers.Main.GameManager.Instance.Find<MenuManager>();
        //_prospector = Resources.Load("SirProspector") as GameObject;
        Debug.Log(_MenuManager);
        _MenuManager.ChangePlayers(1);
        _MenuManager.Adding("SirProspector");
        _MenuManager.ChangeStates();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
