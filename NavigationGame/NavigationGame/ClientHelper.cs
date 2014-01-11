﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using SharedLibrary;
using System.Diagnostics;

namespace NavigationGame
{
    public class ClientHelper
    {
        private GameWcfServiceClient fClient;
        private ClientToken fClientToken;
        internal ClientDetails fClientDetails;
        private Dictionary<int, ClientDetails> fRemoteClients;
        private Random gRandom = new Random();

        public ClientHelper(Dictionary<int, ClientDetails> pGameObjects)
        {
            InstanceContext gInstanceContext = new InstanceContext(new CallbackHandler(pGameObjects));
            fClient = new GameWcfServiceClient(gInstanceContext);
            fRemoteClients = pGameObjects;
            fClientToken = fClient.RegisterClient();
            fClientDetails = new ClientDetails { Id = gRandom.Next(10000), Locations = new List<Location>(), HP = 0 };
        }

        /// <summary>
        /// Send the server updates
        /// </summary>
        /// <param name="pState"></param>
        public void UpdateMyLocation(object pState)
        {
            if (pState != null)
            {
                fClientDetails.Locations = pState as List<Location>;
                fClient.SendClientDetails(fClientToken, fClientDetails);
            }
        }
    }
    public class CallbackHandler : IGameWcfServiceCallback
    {
        private Dictionary<int, ClientDetails> fGameObjects;
        public CallbackHandler(Dictionary<int, ClientDetails> pGameObjects)
        {
            fGameObjects = pGameObjects;
        }
        public void ServerSays(string pServerString)
        {
        }

        public bool IsClientAlive()
        {
            return true;
        }

        public void UpdateClient(ClientDetails pClientDetails)
        {
            if (fGameObjects.ContainsKey(pClientDetails.Id))
            {
                fGameObjects[pClientDetails.Id].Locations.AddRange(pClientDetails.Locations);
            }
            else
            {
                fGameObjects.Add(pClientDetails.Id, pClientDetails);
            }
        }
    }
}
