using System;

namespace SimpleGraphQLClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new SimpleGraphQLClient("https://tweetserver.dynamic-dns.net/graphql");

            #region Register user

            var query = @"
                mutation signup (
                    $fullName: String!
                    $email: String!
                    $password: String!
                    $username: String!
                    $avatar: String
                ) {
                    signup (
                        fullName: $fullName
                        email: $email
                        password: $password
                        username: $username
                        avatar: $avatar
                    ) {
                        token
                    }
                }
            ";

            try
            {
                dynamic signup = client.Execute(query, new
                {
                    email = "test-user1@domain.com",
                    fullName = "Test User",
                    password = "123456",
                    avatar = "https://randomuser.me/api/portraits/women/0.jpg",
                    username = "testuser9"
                });

                // todo
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
            }

            #endregion

            client = null;

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
