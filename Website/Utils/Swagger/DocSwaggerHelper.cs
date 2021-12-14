using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Utils.Swagger
{
    public class DocSwaggerHelper
    {

        public static OpenApiInfo ApiInfo => new OpenApiInfo
        {
            Version = "v1",
            Title = "Journals Recommender API",
            Description = @"
<h1>Introduction</h1>
<p>This is a simple guide on how to use the Journals Recommender API.</p>
<p><em>Note that this API uses JTK authentication and not all methods may be available for all users. For any inconvenience please contact <a href='mailto:jr10@gmail.com'>jr10@gmail.com</a>.</em></p>
<h2>Authentication</h2>
<p>The API use JWT for authentication:</p>
<table>
    <thead><tr><th>HEADER</th><th>EXAMPLE</th><th>COMMENT</th></tr></thead>
    <tbody><tr><td>Bearer</td><td>Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiQWlzIn0.Zqg6tO_TyX-WFP06l3ujlQ_5a4XZKtg8tck6I8Ubixg</td><td>The Token is given by the login</td></tr></tbody>
</table>
<h3>Username and password, huh? Where do I get that?</h3>
<p>To get an Username and password, you must send an email requesting it to <a href='mailto:jr10@gmail.com'>mailto:jr10@gmail.com</a>.</p>

"
        };
    }

}
