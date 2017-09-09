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

        public SimpleGraphQLClient(string GraphQLApiUrl)
        {
            _GraphQLApiUrl = GraphQLApiUrl;
        }

        public dynamic Execute(string query, object variables = null, Dictionary<string, string> additionalHeaders = null)
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

                return CallApiServer<dynamic>(request, additionalHeaders);
            }
            catch (Exception exception)
            {
                dynamic errorResult = new ExpandoObject();
                errorResult.error = exception.Message;

                return errorResult;
            }
        }

        private T CallApiServer<T>(RestRequest request, Dictionary<string, string> additionalHeaders) where T : new ()
        {
            var client = new RestClient(_GraphQLApiUrl);

            if (additionalHeaders != null && additionalHeaders.Count > 0)
            {
                foreach (var AdditionalHeader in additionalHeaders)
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
