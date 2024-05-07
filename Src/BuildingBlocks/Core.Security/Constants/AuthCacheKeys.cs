﻿namespace Core.Security.Constants;

public class AuthCacheKeys
{
    public static string GetAccessTokenKey(object userId) => $"access-token:{userId}";
    public static string GetRevokeAccessTokenKey(string accessToken) => $"revoked-token:{accessToken}";
    public static string GetClientInformationKey(string ip) => $"client-information:{ip}";
}