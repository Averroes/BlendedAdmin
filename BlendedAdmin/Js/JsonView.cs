namespace BlendedAdmin.Js
{
    public class JsonView : View
    {
        public JsonView(object json)
        {
            this["json"] = json;
        }
    }
}
