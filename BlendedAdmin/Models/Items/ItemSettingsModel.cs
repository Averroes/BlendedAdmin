using Microsoft.AspNetCore.Mvc;

namespace BlendedAdmin.Models.Items
{
    public class ItemSettingsModel
    {
        [FromQuery(Name = "_renderAs")]
        public string RenderAs { get; set; }

        [FromQuery(Name = "_httpMethod")]
        public string HttpMethod { get; set; }
    }
}
