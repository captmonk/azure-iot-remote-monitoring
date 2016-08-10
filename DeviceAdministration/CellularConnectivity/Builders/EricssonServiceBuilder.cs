﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DeviceManagement.Infrustructure.Connectivity.Models.Security;
using System.ServiceModel.Channels;
using System.ServiceModel;
using DeviceManagement.Infrustructure.Connectivity.EricssonApiService;

namespace DeviceManagement.Infrustructure.Connectivity.Builders
{
    public class EricssonServiceBuilder
    {

        public static ApiStatusClient GetApiStatusClient(ICredentials credentials)
        {
            var apiStatusClient = new ApiStatusClient();
            apiStatusClient.Endpoint.Address = GetAuthorizedEndpoint(credentials, $"{credentials.BaseUrl}/dcpapi/ApiStatus");
            return apiStatusClient;
        }


        private static EndpointAddress GetAuthorizedEndpoint(ICredentials credentials, string endpointUrl)
        {
            //Create wsse security object
            var usernameToken = new EricssonUsernameToken { Password = credentials.Password, Username = credentials.Password };
            var security = new EricssonSecurity { UsernameToken = usernameToken };

            //Serialize object to xml
            var xmlObjectSerializer = new DataContractSerializer(typeof(EricssonSecurity), "Security", "");

            //Create address header with security header
            var addressHeader = AddressHeader.CreateAddressHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", security, xmlObjectSerializer);
            return new EndpointAddress(new Uri(endpointUrl), new[] { addressHeader });
        }
    }


   
}
