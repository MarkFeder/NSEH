using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

namespace CustomLobby
{
    public class CustomLobbyServerList : MonoBehaviour
    {
        #region Public Properties



        #endregion
        
        #region Private Properties

        [SerializeField]
        private CustomLobbyManager _lobbyManager;

        [SerializeField]
        private RectTransform _serverListRect;
        [SerializeField]
        private GameObject _serverEntryPrefab;
        [SerializeField]
        private GameObject _noServerFound;

        [SerializeField]
        private static Color OddServerColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        [SerializeField]
        private static Color EvenServerColor = new Color(.94f, .94f, .94f, 1.0f);

        #endregion

        #region Protected Properties

        protected int _currentPage;
        protected int _previousPage;

        #endregion

        #region Public Methods

        //public void OnGUIMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        //{
        //    if (matches.Count == 0)
        //    {
        //        if (_currentPage == 0)
        //        {
        //            _noServerFound.SetActive(true);
        //        }

        //        _currentPage = _previousPage;
        //        return;
        //    }

        //    _noServerFound.SetActive(false);
        //    foreach (Transform t in _serverListRect)
        //    {
        //        Destroy(t.gameObject);
        //    }

        //    for (int i = 0; i < matches.Count; ++i)
        //    {
        //        GameObject o = Instantiate(_serverEntryPrefab) as GameObject;

        //        o.GetComponent<CustomLobbyServerEntry>().Populate(matches[i], _lobbyManager, (i % 2 == 0) ? OddServerColor : EvenServerColor);

        //        o.transform.SetParent(_serverListRect, false);
        //    }
        //}

        //public void ChangePage(int dir)
        //{
        //    int newPage = Mathf.Max(0, _currentPage + dir);

        //    //if we have no server currently displayed, need we need to refresh page0 first instead of trying to fetch any other page
        //    if (_noServerFound.activeSelf)
        //    {
        //        newPage = 0;
        //    }

        //    RequestPage(newPage);
        //}

        //public void RequestPage(int page)
        //{
        //    _previousPage = _currentPage;
        //    _currentPage = page;
        //    _lobbyManager.matchMaker.ListMatches(page, 6, "", true, 0, 0, OnGUIMatchList);
        //}


        #endregion

        #region Private Methods

        //private void OnEnable()
        //{
        //    _currentPage = 0;
        //    _previousPage = 0;

        //    foreach (Transform t in _serverListRect)
        //    {
        //        Destroy(t.gameObject);
        //    }

        //    _noServerFound.SetActive(false);

        //    RequestPage(0);
        //}

        #endregion
    }
}