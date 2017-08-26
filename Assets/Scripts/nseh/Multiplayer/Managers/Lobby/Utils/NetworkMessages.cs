using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace CustomLobby.Utils
{
    public enum Character
    {
        None = 0,
        Wrarr = 1,
        SirProspector = 2,
        Granhilda = 3,
        MySon = 4
    }

    public class KickMsg : MessageBase { }

    public class CustomNetworkMessage : MessageBase
    {
        public Character ChosenCharacter { get; set; }
    }
}
