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
                mutation {
                    signup(
                        email: $email
                        fullName: $fullName
                        password: $password
                        avatar: $avatar
                        username: $username
                    ) {
                        token
                    }
                }
            ";

            dynamic signup = client.Execute(query, new
            {
                email = "test-user1@domain.com",
                fullName = "Test User",
                password = "123456",
                avatar = "https://randomuser.me/api/portraits/women/0.jpg",
                username = "testuser1"
            });

            if (signup != null && signup.error == null)
            {
                Console.WriteLine((string)signup.token);
            }
            else
            {
                Console.WriteLine("Error - {0}", signup.error);
            }

            #endregion

            client = null;

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
