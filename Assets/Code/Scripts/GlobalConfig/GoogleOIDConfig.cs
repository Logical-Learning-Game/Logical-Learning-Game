using System.Collections;
using UnityEngine;

namespace GlobalConfig
{
    public static class GoogleOIDConfig
    {
        public static string OID_METADATA_URL { get; } = "https://accounts.google.com/.well-known/openid-configuration";
        public static string PROVIDER_NAME { get; } = "oidc-google";
        public static string CLIENT_ID { get; } = "136131822768-2b5gt80rvohd4jq03dbkh8bl2910v18k.apps.googleusercontent.com";
        public static string SECRET { get; } = "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";
    }
}