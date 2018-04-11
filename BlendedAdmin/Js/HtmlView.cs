namespace BlendedAdmin.Js
{
    public class HtmlView : View
    {
        public HtmlView(object html)
        {
            this["html"] = html;
        }
    }
}
