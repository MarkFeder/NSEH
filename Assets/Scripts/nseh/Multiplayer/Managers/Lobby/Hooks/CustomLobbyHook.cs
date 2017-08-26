using UnityEngine;
using UnityEngine.Networking;

namespace CustomLobby.Hooks
{
    public class CustomLobbyHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            CustomLobbyPlayer lobby = lobbyPlayer.GetComponent<CustomLobbyPlayer>();
            //NetworkSpaceship spaceship = gamePlayer.GetComponent<NetworkSpaceship>();

            //spaceship.name = lobby.name;
            //spaceship.color = lobby.playerColor;
            //spaceship.score = 0;
            //spaceship.lifeCount = 3;
        }
    }
}
