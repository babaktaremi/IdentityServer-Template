// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>()
        {
            new ApiResource("MicroService","Api Access")
            {
                Scopes = {"fullaccess","Read"} ,
                Name = "MicroService"
            },
            new ApiResource("ReadWrite","Api Access")
            {
                Scopes = {"Read","Write"} ,
            },
            new ApiResource("Gateway")
            {
                Scopes = { "Gateway.fullaccess" }
            }

        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("fullaccess"),
                new ApiScope("Read"),
                new ApiScope("Write"),
                new ApiScope("Gateway.fullaccess"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Api to Api Access",
                    ClientId = "APIID",
                    ClientSecrets = {new Secret("790681d3-0078-48fd-a3fd-2a2eb66a52f3".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "fullaccess"}
                }, 
                new Client
                {
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {"openid","profile","Read",},
                    ClientSecrets = {new Secret("bb4cfb89-de96-4980-80e7-dbdbbe9d7a9f".Sha256()) },
                    ClientName = "Api Client Name",
                    RedirectUris = { "https://localhost:5101/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5101/signout-callback-oidc" },
                    ClientId = "APICLIENT"
                },
                new Client
                {
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    AllowedScopes = {"openid","profile","Gateway.FullAccess",},
                    ClientSecrets = {new Secret("bb4cfb89-de96-4980-80e7-dbdbbe9d7a9d".Sha256()) },
                    ClientName = "Api Gateway Name",
                    RedirectUris = { "https://localhost:5101/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5101/signout-callback-oidc" },
                    ClientId = "Gateway"
                },
            };
    }
}