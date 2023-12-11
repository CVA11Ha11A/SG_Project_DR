using System;
using UnityEngine;

public class SecureURLHandler
{
    private static string _morning = "AvhYWYlQ7URDhQW/5wiLE7aWELYHlauaMjJcO2tZ7GDgWQ3gXXr44tieIMoi7GWLCT4g+oLdiy7AO6FaUcPBwuB/AbvDKSpIrn80UIDtddzwV3bsu1JGzp+q26+b924e";
    private static string _dinner = "Hello, hacker! Are you trying to hack into my project? Give it a shot, LOL. By the way, does your mother know you're up to this? LOLZ";
    private static bool lunch = true;

    public static string GetURL()
    {
        if (lunch)
        {
            lunch = false;
            TextAsset hoi = Resources.Load("231211") as TextAsset;
            string[] bye = hoi.text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string helloHacker = Crypto.EncryptAESbyBase64Key("Hello, hacker! Are you trying to hack into my project? Give it a shot, LOL. By the way, does your mother know you're up to this? LOLZ", bye[0], bye[1]);
            _dinner = Crypto.DecryptAESByBase64Key(_morning, bye[0], bye[1]);
            return _dinner;
        }
        return _dinner;
    }
}
