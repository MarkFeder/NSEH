using nseh.Managers;
using nseh.Managers.Main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ScoreMenu_Component : MonoBehaviour {


    MenuManager _MenuManager;
    public GameObject current;
    public GameObject twoPlayerScore;
    public GameObject fourPlayerScore;
    public GameObject CanvasTwoPlayerScore;
    public GameObject CanvasFourPlayerScore;
    // Use this for initialization
    void Start ()
    {
        _MenuManager = GameManager.Instance.Find<MenuManager>();
        if(_MenuManager.MyGame._numberPlayers == 2)
        {
            current.SetActive(false);
            current = CanvasTwoPlayerScore;
            current.SetActive(true);
            StartCoroutine("Score_two");
        }

        else if (_MenuManager.MyGame._numberPlayers == 4)
        {
            current.SetActive(false);
            current = CanvasFourPlayerScore;
            current.SetActive(true);
            StartCoroutine("Score_four");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    IEnumerator Score_two()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {

            for (int j = 0; j < _MenuManager.MyGame._numberPlayers; j++)
            {
                twoPlayerScore.gameObject.transform.GetChild(j).transform.GetChild(i).GetComponent<Text>().text = _MenuManager.MyGame._score[j, i].ToString();

            }
            yield return new WaitForSeconds(1f);
            Debug.Log("DSADAS " + i);
            //Invoke("Wait", 10);
        }
        for (int j = 0; j < _MenuManager.MyGame._numberPlayers; j++)
        {
            twoPlayerScore.gameObject.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = (_MenuManager.MyGame._score[j, 0] + _MenuManager.MyGame._score[j, 1] + _MenuManager.MyGame._score[j, 2]).ToString();

        }
        yield return new WaitForSeconds(1f);
        Debug.Log("DSADAS " + 3);

    }


    IEnumerator Score_four()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < _MenuManager.MyGame._numberPlayers; j++)
            {
                Debug.Log(fourPlayerScore.gameObject.transform.GetChild(j).transform.GetChild(i).name);
                fourPlayerScore.gameObject.transform.GetChild(j).transform.GetChild(i).GetComponent<Text>().text = _MenuManager.MyGame._score[j, i].ToString();


            }
            yield return new WaitForSeconds(1f);

        }
        for (int j = 0; j < _MenuManager.MyGame._numberPlayers; j++)
        {
            fourPlayerScore.gameObject.transform.GetChild(j).transform.GetChild(3).GetComponent<Text>().text = (_MenuManager.MyGame._score[j, 0] + _MenuManager.MyGame._score[j, 1] + _MenuManager.MyGame._score[j, 2]).ToString();


        }
        yield return new WaitForSeconds(1f);
    }


    public void Restart()
    {
        _MenuManager.MyGame.ChangeState(GameManager.States.Playing);
    }

    public void MainMenu()
    {
        _MenuManager.MyGame.ChangeState(GameManager.States.MainMenu);
    }

}
