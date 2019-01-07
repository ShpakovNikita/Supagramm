using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using IgiLab.LogicHelpers;

namespace IgiLab.TagHelpers
{
    public class PeopleCountTagHelper : TagHelper
    {
        public bool Followers { get; set; }
        public bool Following { get; set; }
        public int Argument { get; set; }

        public int Number { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (((Followers ? 1 : 0) + (Following ? 1 : 0)) > 1)
            {
                throw new FormatException("Too many attributes for this tag-helper");
            }

            string content = (await output.GetChildContentAsync()).GetContent();
            

            if (Followers)
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("href", "/User/Followers/" + Argument.ToString());
            }
            else if (Following)
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("href", "/User/Following/" + Argument.ToString());
            }
            else
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("href", "/User/Profile/" + Argument.ToString());
            }

            output.Content.SetContent(PeopleCount.GetNumber(Number) + " " + content);

        }
    }
}
