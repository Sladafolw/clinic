using System;
public static class impFunctions
{
    public static ExpandoObject ToExpando(this object anonymousObject)
    {
        IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
        IDictionary<string, object> expando = new ExpandoObject();
        foreach (var item in anonymousDictionary)
            expando.Add(item);
        return (ExpandoObject)expando;
    }
}