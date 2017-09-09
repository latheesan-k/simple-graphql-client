using System;
using RestSharp;
using System.Dynamic;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.Collections.Generic;

namespace SimpleGraphQLClient
{
    public class SimpleGraphQLClient
    {
        private string _GraphQLApiUrl;
        private string _GraphQLApiUser;
        private string _GraphQLApiPass;
        private Dictionary<string, string> _AdditionalHeaders;

        public SimpleGraphQLClient(string GraphQLApiUrl, string GraphQLApiUser = null, string GraphQLApiPass = null, Dictionary<string, string> AdditionalHeaders = null)
        {
            _GraphQLApiUrl = GraphQLApiUrl;
            _GraphQLApiUser = GraphQLApiUser;
            _GraphQLApiPass = GraphQLApiPass;
            _AdditionalHeaders = AdditionalHeaders;
        }

        public dynamic Execute(string query, object variables)
        {
            try
            {
                var request = new RestRequest("/", Method.POST);
                request.RequestFormat = DataFormat.Json;

                var requestBody = JsonConvert.SerializeObject(new GraphQLQuery()
                {
                    query = query,
                    variables = variables,
                });

                request.AddBody(requestBody);

                return CallApiServer<dynamic>(request);
            }
            catch (Exception exception)
            {
                dynamic errorResult = new ExpandoObject();
                errorResult.error = exception.Message;

                return errorResult;
            }
        }

        private T CallApiServer<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(_GraphQLApiUrl);

            if (!string.IsNullOrEmpty(_GraphQLApiUser) && !string.IsNullOrEmpty(_GraphQLApiPass))
            {
                client.Authenticator = new HttpBasicAuthenticator(_GraphQLApiUser, _GraphQLApiPass);
            }

            if (_AdditionalHeaders != null && _AdditionalHeaders.Count > 0)
            {
                foreach (var AdditionalHeader in _AdditionalHeaders)
                {
                    request.AddParameter(AdditionalHeader.Key, AdditionalHeader.Value, ParameterType.GetOrPost);
                }
            }

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            return response.Data;
        }
    }
}
