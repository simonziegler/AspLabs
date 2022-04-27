using System.Text;
using System.Web;

namespace ClassLibrary;

public class RequestInfo
{
    public static void WriteRequestInfo(bool suppress)
    {
        var context = HttpContext.Current;

        context.Response.ContentType = "text/html";

        using (var writer = new SimpleJsonWriter(context.Response))
        {
            writer.Write("RawUrl", context.Request.RawUrl);
            writer.Write("Path", context.Request.Path);
            writer.Write("Length", context.Request.InputStream.Length);
            writer.Write("Charset", context.Response.Charset);
            writer.Write("ContentType", context.Response.ContentType);
            writer.Write("ContentEncoding", context.Response.ContentEncoding.WebName);
            context.Response.Output.Flush();

            // Status code
            writer.Write("StatusCode", context.Response.StatusCode);
            writer.Write("StatusDescription", context.Response.StatusDescription);
        }

        context.Response.End();

        context.Response.SuppressContent = suppress;
        context.Response.End();
    }
}
