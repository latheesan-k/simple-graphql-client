using System;
using System.Collections.Generic;

namespace SimpleGraphQLClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new SimpleGraphQLClient("https://tweetserver.dynamic-dns.net/graphql");
            var token = "";

            #region Register user

            var signupQuery = @"
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
                var randomString = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 16);

                var result = client.Execute(signupQuery, new
                {
                    email = "test-"+ randomString + "@domain.com",
                    fullName = "Test User",
                    password = "123456",
                    avatar = "https://randomuser.me/api/portraits/women/0.jpg",
                    username = randomString
                });

                if (result.errors == null)
                {
                    token = result.data["signup"].token;

                    Console.WriteLine("signup: Token = {0}", token);
                }
                else
                {
                    Console.WriteLine("signup Error: {0}", result.errors[0].message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("signup Exception: {0}", exception.Message);
            }

            #endregion

            #region Get tweets

            var getTweetsQuery = @"
                {
                    getTweets {
                        _id
                        text
                        createdAt
                        favoriteCount
                        user {
                            username
                            avatar
                            lastName
                            firstName
                        }
                    }
                }
            ";

            try
            {
                var result = client.Execute(getTweetsQuery, null, new Dictionary<string, string>()
                {
                    { "authorization", "Bearer " + token }
                });

                if (result.errors == null)
                {
                    Console.WriteLine("getTweets: {0}", result.data["getTweets"].ToString());
                }
                else
                {
                    Console.WriteLine("getTweets Error: {0}", result.errors[0].message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("getTweets Exception: {0}", exception.Message);
            }

            #endregion

            client = null;

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
