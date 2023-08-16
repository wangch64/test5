using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

class ExampleClass
{
    public void ExampleMethod(string key, string value)
    {
        var cookieOptions = new CookieOptions();
        cookieOptions.Secure = false;
        var responseCookies = new ResponseCookies(null, null);
        responseCookies.Append(key, value, cookieOptions);
    }
}
